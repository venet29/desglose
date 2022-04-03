using Desglose.Ayuda;
using Desglose.DTO;
using Desglose.Model;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Desglose.Dibujar2D;

namespace Desglose.Calculos
{
    public class GruposListasTraslapoIguales_HV2
    {
        private UIApplication uiapp;
        private Document _doc;
        private List<RebarDesglose_GrupoBarras_H> GruposRebarMismaLinea;
        private readonly Config_EspecialElev _config_EspecialElv;

        public List<RebarDesglose_GrupoBarras_H> soloListaPrincipales { get; set; }


        public List<RebarDesglose_GrupoBarras_H> ListaBarrasSuperiores { get; private set; }
        public List<RebarDesglose_GrupoBarras_H> ListaBarrasInferior { get; private set; }

        public List<RebarDesglose_Barras_H2> listaBArrasEnElev_laterales { get; private set; }


        public GruposListasTraslapoIguales_HV2(UIApplication uiapp, List<RebarDesglose_GrupoBarras_H> lista_RebarDesglose, Config_EspecialElev _Config_EspecialElv)
        {
            this.uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this.GruposRebarMismaLinea = lista_RebarDesglose;
            this._config_EspecialElv = _Config_EspecialElv;
        }

        public GruposListasTraslapoIguales_HV2()
        {
            soloListaPrincipales = new List<RebarDesglose_GrupoBarras_H>();
        }

        public bool M1_ObtenerGruposTraslaposIgualesV2()
        {
            try
            {
                m1_1_crearGruposIgualesDeBarraColineal();

                //M1_2_CopiarPArametros();


            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener grupos de barras  ex:{ ex.Message} ");
                return false;
            }
            return true;
        }

        private void m1_1_crearGruposIgualesDeBarraColineal()
        {
            for (int k = 0; k < GruposRebarMismaLinea.Count; k++)
            {
                RebarDesglose_GrupoBarras_H itemGrup = GruposRebarMismaLinea[k];
                if (itemGrup._casoAgrupar != casoAgrupar.NoAnalizada) continue;
                List<RebarDesglose_GrupoBarras_H> _ListRebarDesglose_GrupoBarras =
                    GruposRebarMismaLinea.Where(c => Util.IsEqual(c._ptoInicial.Z, itemGrup._ptoInicial.Z, c._curvePrincipal.Length * ConstNH.CONST_porcentajeErrorRespectoLArgoBArras) &&
                                                    Util.IsEqual(c._ptoFinal.Z, itemGrup._ptoFinal.Z, c._curvePrincipal.Length * ConstNH.CONST_porcentajeErrorRespectoLArgoBArras)).ToList();

                // foreach principal
                for (int j = 0; j < _ListRebarDesglose_GrupoBarras.Count; j++)
                {

                    var GrupoBarrasPrincipa = _ListRebarDesglose_GrupoBarras[j];
                    // si se analizo continuar
                    if (GrupoBarrasPrincipa._casoAgrupar != casoAgrupar.NoAnalizada) continue;

                    GrupoBarrasPrincipa._casoAgrupar = casoAgrupar.Principal;
                    List<RebarDesglose_GrupoBarras_H> _ListRebarDesglose_GrupoBarrasRepetidas = new List<RebarDesglose_GrupoBarras_H>();

                    //foreach iterativo de revisio
                    for (int i = 0; i < _ListRebarDesglose_GrupoBarras.Count; i++)
                    {
                        var GrupoBarraRepetido = _ListRebarDesglose_GrupoBarras[i];
                        // si se analizo continuar
                        if (GrupoBarraRepetido._casoAgrupar != casoAgrupar.NoAnalizada) continue;

                        if (VerificarGrupoBArrasRepetidas(GrupoBarrasPrincipa, GrupoBarraRepetido))
                        {
                            //asignar como repetida
                            GrupoBarraRepetido._casoAgrupar = casoAgrupar.Repetido;
                            _ListRebarDesglose_GrupoBarrasRepetidas.Add(GrupoBarraRepetido);
                        }
                    }
                    if (_ListRebarDesglose_GrupoBarrasRepetidas.Count > 0)
                        GrupoBarrasPrincipa._ListaRebarDesglose_GrupoBarrasRepetidas.AddRange(_ListRebarDesglose_GrupoBarrasRepetidas);
                }

                soloListaPrincipales = GruposRebarMismaLinea.Where(c => c._casoAgrupar == casoAgrupar.Principal).ToList();
            }
        }

        private void M1_2_CopiarPArametros()
        {

            

            ParametroShareNhDTO _Paratipobarra = _config_EspecialElv.tipoBarraElev;
            char letra = char.Parse(_config_EspecialElv.tipoBarraElev.valor);
            string idLetra = letra + DateTime.Now.ToString("yyyyMMddHHmmss");

            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Crar parametros compartidos");

                    foreach (var itemREpetidas in soloListaPrincipales)
                    {
                        foreach (var SUBitempRIN in itemREpetidas._GrupoRebarDesglose)
                            AgregarPAra(_Paratipobarra, letra, idLetra, SUBitempRIN);

                        foreach (var item in itemREpetidas._ListaRebarDesglose_GrupoBarrasRepetidas)
                        {
                            //REPETIDAS
                            foreach (var SUBitem in item._GrupoRebarDesglose)
                                AgregarPAra(_Paratipobarra, letra, idLetra, SUBitem);
                        }
                        letra++;
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                TaskDialog.Show("Error DibujarBarraRebarShape", msj);
            }
        }


        internal bool PreDibujar(View viewOriginal)
        {

            try
            {


                double zMax = GruposRebarMismaLinea.Max(c => c.Trasform.EjecutarTransformInvertida(c._ptoFinal).Z);
                double zmin = GruposRebarMismaLinea.Min(c => c.Trasform.EjecutarTransformInvertida(c._ptoFinal).Z);
                //double zmin = listaBArrasEnElev_NOlaterales.Min(c => c.RebarDesglose_Barras_H_._rebarDesglose.trasform.EjecutarTransformInvertida(c.RebarDesglose_Barras_H_.ptoMedio).Z);
                double Zmedio = (zMax + zmin) / 2;
                ListaBarrasSuperiores = GruposRebarMismaLinea.Where(c => c.Trasform.EjecutarTransformInvertida(c._ptoFinal).Z > Zmedio)
                                                                     .OrderByDescending(c => c.Trasform.EjecutarTransformInvertida(c._ptoFinal).Z).ToList();
                ListaBarrasInferior = GruposRebarMismaLinea.Where(c => c.Trasform.EjecutarTransformInvertida(c._ptoFinal).Z < Zmedio)
                                                                     .OrderByDescending(c => c.Trasform.EjecutarTransformInvertida(c._ptoFinal).Z).ToList();


                //Lista SUperior
                for (int i = 0; i < ListaBarrasSuperiores.Count; i++)
                {
                    // if ()
                }

                //Lista Inferior
                for (int i = 0; i < ListaBarrasInferior.Count; i++)
                {
                    // if ()
                }


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error  Dibujar2D_Barras_Corte_H_PreDibujar  ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private static void AgregarPAra(ParametroShareNhDTO _Paratipobarra, char letar, string idLetra, RebarDesglose_Barras_H SUBitem)
        {
            Rebar _rebar = SUBitem._rebarDesglose._rebar;

            if (ParameterUtil.FindParaByName(_rebar, _Paratipobarra.NombrePAra) != null)
                ParameterUtil.SetParaInt(_rebar, _Paratipobarra.NombrePAra, letar.ToString());//(30+100+30)

            if (ParameterUtil.FindParaByName(_rebar, "IdMLB") != null)
                ParameterUtil.SetParaInt(_rebar, "IdMLB", idLetra);//(30+100+30)
        }

        private bool VerificarGrupoBArrasRepetidas(RebarDesglose_GrupoBarras_H GrupoBarrasPrincipa, RebarDesglose_GrupoBarras_H GrupoBarraRepetido)
        {
            try
            {
                if (GrupoBarrasPrincipa._GrupoRebarDesglose.Count != GrupoBarraRepetido._GrupoRebarDesglose.Count) return false;

                for (int i = 0; i < GrupoBarrasPrincipa._GrupoRebarDesglose.Count; i++)
                {
                    RebarDesglose_Barras_H _barraPrincipal = GrupoBarrasPrincipa._GrupoRebarDesglose[i];
                    RebarDesglose_Barras_H _barraRepetida = GrupoBarraRepetido._GrupoRebarDesglose[i];

                    if (_barraPrincipal.diametroMM != _barraRepetida.diametroMM) return false;

                    if (_barraPrincipal._rebarDesglose.ListaCurvaBarras.Count != _barraRepetida._rebarDesglose.ListaCurvaBarras.Count) return false;

                    if (!Util.IsSimilarValor(_barraPrincipal.ptoInicial.Z, _barraRepetida.ptoInicial.Z, _barraPrincipal.curvePrincipal.Length * ConstNH.CONST_porcentajeErrorRespectoLArgoBArras)) return false;

                    if (!Util.IsSimilarValor(_barraPrincipal.ptoFinal.Z, _barraRepetida.ptoFinal.Z, _barraPrincipal.curvePrincipal.Length * ConstNH.CONST_porcentajeErrorRespectoLArgoBArras)) return false;

                }
            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error en 'VerificarGrupoBArrasRepetidas'  ex:{ ex.Message} ");
                return false;
            }
            return true;
        }
    }
}
