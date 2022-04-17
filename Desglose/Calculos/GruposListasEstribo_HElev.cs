
using Desglose.Ayuda;
using Desglose.Model;

using Desglose.Extension;
using Desglose.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADesglose.Ayuda;

namespace Desglose.Calculos
{
    public class GruposListasEstribo_HElev
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;
        private List<RebarDesglose> lista_RebarDesglose;

        public DatosHost _DatosHost { get; set; }

        public List<RebarDesglose_GrupoBarras_H> GruposRebarMismaLinea { get; set; }
        //  public PlanarFace CaraInferior { get; private set; }
        //  public XYZ CentroHost { get; private set; }
        // public double LargoMAximoHost_foot { get; private set; }
        public List<XYZ> ListaPtoSeccion { get; private set; }
        public List<RebarDesglose_Barras_H> listaBArrasEnElev { get; private set; }

        public GruposListasEstribo_HElev(UIApplication uiapp, List<RebarDesglose> lista_RebarDesglose)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this.lista_RebarDesglose = lista_RebarDesglose;
            this.GruposRebarMismaLinea = new List<RebarDesglose_GrupoBarras_H>();
        }


        public bool ObtenerGruposEstribo_Viga()
        {

            List<RebarDesglose_Barras_H> listaBArras_pertenecenEstriboViga = lista_RebarDesglose.Where(c => c._tipoBarraEspecifico == TipoRebar.ELEV_ES_VT || c._tipoBarraEspecifico == TipoRebar.ELEV_ES_VL ||
                                                                                    c._tipoBarraEspecifico == TipoRebar.ELEV_ES_V)
                                                                         .Select(c => new RebarDesglose_Barras_H(c, _uiapp)).ToList();

            if (listaBArras_pertenecenEstriboViga.Count == 0) return true;
            //obtener RebarDesglose_Barras
            foreach (RebarDesglose_Barras_H item in listaBArras_pertenecenEstriboViga)
            {

                // item.Ordenar_UltimaCurvaMayorZ();
                // item.Ordenar_Analizar();
                if (item._tipoBarraEspecifico == TipoRebar.ELEV_ES_VL)
                    item.Ordenar_Analizar();
                else
                    item.Ordenar_AnalizarEstribo();
            }

            //

            var unEstribo = listaBArras_pertenecenEstriboViga.Where(c => c._tipoBarraEspecifico == TipoRebar.ELEV_ES_V).FirstOrDefault();

            if (unEstribo == null) return false; //no encuentra estribo

            _DatosHost = new DatosHost(_uiapp, unEstribo._rebarDesglose);
            if (!_DatosHost.ObtenerPtoMedio_conestribo()) return false;
            if (!_DatosHost.ObtenerCentroPilarOmUro()) return false;

            //ObtenerCentroPilarOmUro();
            Obtener2PTOSCrearSeccion(unEstribo);
            // ordenar de los inicial menor y solo verticales

            //listaBArras_pertenecenEstriboViga = listaBArras_pertenecenEstriboViga.Where(c=> c._tipoBarraEspecifico== TipoRebar.ELEV_ES_V)
            //                                                                     .OrderBy(c => c.ptoInicial.Z).ToList();

            listaBArras_pertenecenEstriboViga = listaBArras_pertenecenEstriboViga.OrderBy(c => c.ptoInicial.Z).ToList();

            try
            {
                for (int i = 0; i < listaBArras_pertenecenEstriboViga.Count; i++)
                {
                    RebarDesglose_Barras_H EstriboAnalizado = listaBArras_pertenecenEstriboViga[i];

                    RebarDesglose_Barras_H BarraAnalizada = EstriboAnalizado;
                    List<RebarDesglose_Barras_H> NuewGrupoEstribo = new List<RebarDesglose_Barras_H>();  /// si el grupo tiene mas estribo es pq son E.D. o E.T.
                    if (EstriboAnalizado.analizadasuperior) continue;

                    if (EstriboAnalizado._tipoBarraEspecifico != TipoRebar.ELEV_ES_V) continue; //salta si  no es estribo

                    EstriboAnalizado.analizadasuperior = true;
                    NuewGrupoEstribo.Add(EstriboAnalizado);
                    RebarDesglose_GrupoBarras_H _RebarDesglose_GrupoBarrasNew = null;

                    //busca dentro del gurpo colineal
                    for (int j = 0; j < listaBArras_pertenecenEstriboViga.Count; j++)
                    {
                        RebarDesglose_Barras_H Elemento_de_estriboAnalizado = listaBArras_pertenecenEstriboViga[j];
                        if (Elemento_de_estriboAnalizado.analizadasuperior) continue;
                        // cuando el pto inicial de la sigueinte barra no esta contendia en la actual

                        if (Elemento_de_estriboAnalizado._tipoBarraEspecifico == TipoRebar.ELEV_ES_VL)
                        {
                            double zmax = Math.Max(Elemento_de_estriboAnalizado.ptoFinal.Z, Elemento_de_estriboAnalizado.ptoInicial.Z);
                            double zmin = Math.Min(Elemento_de_estriboAnalizado.ptoFinal.Z, Elemento_de_estriboAnalizado.ptoInicial.Z);

                            //if (EstriboAnalizado.ptoInicial.Z < zmax && EstriboAnalizado.ptoFinal.Z > zmin)
                            if (zmin<EstriboAnalizado.ptoInicial.Z  && EstriboAnalizado.ptoFinal.Z < zmax)
                            {
                                //Elemento_de_estriboAnalizado.analizadasuperior = true;
                                NuewGrupoEstribo.Add(Elemento_de_estriboAnalizado);
                            }
                        }
                        else if (EstriboAnalizado.ptoInicial.Z < Elemento_de_estriboAnalizado.ptoMedio.Z && EstriboAnalizado.ptoFinal.Z > Elemento_de_estriboAnalizado.ptoMedio.Z)
                        {
                            Elemento_de_estriboAnalizado.analizadasuperior = true;
                            NuewGrupoEstribo.Add(Elemento_de_estriboAnalizado);
                        }
                    }

                    _RebarDesglose_GrupoBarrasNew = RebarDesglose_GrupoBarras_H.Creador_RebarDesglose_GrupoBarras(NuewGrupoEstribo, EstriboAnalizado._rebarDesglose.trasform);
                    GruposRebarMismaLinea.Add(_RebarDesglose_GrupoBarrasNew);
                }
            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener grupos de barras  ex:{ ex.Message} ");
                return false;
            }
            return true;
        }

        public bool ObteneGruposDeBarraEnELev_corte()
        {
            try
            {
                listaBArrasEnElev = lista_RebarDesglose.Where(c => c._tipoBarraEspecifico == TipoRebar.ELEV_BA_H)
                                                                         .Select(c => new RebarDesglose_Barras_H(c, _uiapp)).ToList();
                //obtener RebarDesglose_Barras
                foreach (RebarDesglose_Barras_H item in listaBArrasEnElev)
                {
                    item.Ordenar_UltimaCurvaMayorZ();
                    item.Ordenar_Analizar();
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                UtilDesglose.ErrorMsg($"Error al obtener barras en elevacion de corte ex:{ex.Message}");
                return false;
            }
            return true;

        }
        private void Obtener2PTOSCrearSeccion(RebarDesglose_Barras_H _RebarDesglose_Barras)
        {

            XYZ ptoInicia = _DatosHost.CentroHost - _DatosHost.Direccion_ParalelaView * _DatosHost.LargoMAximoHost_foot * 1;
            XYZ ptoFin = _DatosHost.CentroHost + _DatosHost.Direccion_ParalelaView * _DatosHost.LargoMAximoHost_foot * 3;

            if (AyudaCurveRebar.GetMitadRebarCurves(_RebarDesglose_Barras._rebarDesglose._rebar))
            {
                List<Curve> listaCurva = AyudaCurveRebar.curvaMedia;

                XYZ centroSUma = XYZ.Zero;
                foreach (Curve item in listaCurva)
                {
                    centroSUma = (centroSUma + item.ComputeDerivatives(0.5, false).Origin);
                }
                centroSUma = centroSUma / listaCurva.Count;

                ptoInicia = centroSUma - _DatosHost.Direccion_ParalelaView * _DatosHost.LargoMAximoHost_foot * 1;
                ptoFin = centroSUma + _DatosHost.Direccion_ParalelaView * _DatosHost.LargoMAximoHost_foot * 1;

                // double zaltura = AyudaCurveRebar.curvaMedia[0].GetEndPoint(0).Z;
                //ptoInicia = ptoInicia.AsignarZ(zaltura);
                // ptoFin = ptoFin.AsignarZ(zaltura);
            }
            ListaPtoSeccion = new List<XYZ>();
            ListaPtoSeccion.Add(ptoInicia);
            ListaPtoSeccion.Add(ptoFin);
        }
    }
}
