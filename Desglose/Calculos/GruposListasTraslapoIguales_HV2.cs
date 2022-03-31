using Desglose.Ayuda;
using Desglose.DTO;
using Desglose.Model;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Desglose.Calculos
{
    public class GruposListasTraslapoIguales_HV2
    {
        private UIApplication uiapp;
        private Document _doc;
        private List<RebarDesglose_GrupoBarras_H> GruposRebarMismaLinea;

        public List<RebarDesglose_GrupoBarras_H> soloListaPrincipales { get; set; }
        public GruposListasTraslapoIguales_HV2(UIApplication uiapp, List<RebarDesglose_GrupoBarras_H> lista_RebarDesglose)
        {
            this.uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this.GruposRebarMismaLinea = lista_RebarDesglose;
        }



        public bool ObtenerGruposTraslaposIgualesV2(Config_EspecialElev _Config_EspecialElv)
        {
            try
            {

                for (int k = 0; k < GruposRebarMismaLinea.Count; k++)
                {
                    RebarDesglose_GrupoBarras_H itemGrup = GruposRebarMismaLinea[k];
                    if (itemGrup._casoAgrupar != casoAgrupar.NoAnalizada) continue;
                    List<RebarDesglose_GrupoBarras_H> _ListRebarDesglose_GrupoBarras =
                        GruposRebarMismaLinea.Where(c=> Util.IsEqual(c._ptoInicial.Z,itemGrup._ptoInicial.Z, c._curvePrincipal.Length * ConstNH.CONST_porcentajeErrorRespectoLArgoBArras) &&
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
                }

                soloListaPrincipales = GruposRebarMismaLinea.Where(c => c._casoAgrupar == casoAgrupar.Principal).ToList();

                ParametroShareNhDTO _Paratipobarra = _Config_EspecialElv.tipoBarraElev;
                char letra = char.Parse(_Config_EspecialElv.tipoBarraElev.valor);
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
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener grupos de barras  ex:{ ex.Message} ");
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
