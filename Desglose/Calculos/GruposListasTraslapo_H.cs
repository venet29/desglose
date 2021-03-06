using Desglose.Ayuda;
using Desglose.Model;
using Desglose.Extension;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Desglose.DTO;
using Autodesk.Revit.DB;
using System.Diagnostics;

namespace Desglose.Calculos
{
    class GruposListasTraslapo_H
    {
        private UIApplication _uiapp;
        private View _view;
        private List<RebarDesglose> lista_RebarDesglose;
        private Config_EspecialElev config_EspecialElv;
        private TraslapoBarrasH _newTraslapoBarrasH;

        public List<RebarDesglose_GrupoBarras_H> GruposRebarMismaLinea_Colineal { get; set; }
        public List<TraslapoBarrasH> ListaTraslapoBarrasH { get; set; }
        public GruposListasTraslapo_H(UIApplication uiapp, List<RebarDesglose> lista_RebarDesglose, Config_EspecialElev _Config_EspecialElv)
        {
            this._uiapp = uiapp;
            this._view = uiapp.ActiveUIDocument.ActiveView;
            this.lista_RebarDesglose = lista_RebarDesglose;
            config_EspecialElv = _Config_EspecialElv;
            this.GruposRebarMismaLinea_Colineal = new List<RebarDesglose_GrupoBarras_H>();
            ListaTraslapoBarrasH = new List<TraslapoBarrasH>();
        }

        public bool ObtenerGruposTraslapos()
        {
            List<RebarDesglose_Barras_H> listaBArras_sinLat = lista_RebarDesglose.Where(c => c._tipoBarraEspecifico == TipoRebar.ELEV_BA_H &&
                                                                                             config_EspecialElv.DiamtroLateralMax < c.Diametro_MM)
                                                                         .Select(c => new RebarDesglose_Barras_H(c, _uiapp)).ToList();
            //obtener RebarDesglose_Barras
            foreach (RebarDesglose_Barras_H item in listaBArras_sinLat)
            {
                item.Ordenar_UltimaCurvaMayorZ();
                item.Ordenar_Analizar();
            }



            // ordenar de los inicial menor y solo verticales

            listaBArras_sinLat = listaBArras_sinLat.Where(c => c._direccion == Ayuda.direccionBarra.Horizontal).OrderBy(c => c.ptoInicial.Z).ToList();



            //a) buscar las barras y agrupar iguales en el plano entrando en view
            //b)  asignar  posicon de lineas
            //c) rotar y buscar lineas de barras con tralpaos 
            //d) genrar barras por lineas con dimensiones


            try
            {
                for (int i = 0; i < listaBArras_sinLat.Count; i++)
                {
                    RebarDesglose_Barras_H item = listaBArras_sinLat[i];

                    RebarDesglose_Barras_H BarraAnalizada = item;
                    List<RebarDesglose_Barras_H> NuevoGrupoBarras_Colineal = new List<RebarDesglose_Barras_H>();
                    if (item.analizadasuperior) continue;

                    item.analizadasuperior = true;
                    NuevoGrupoBarras_Colineal.Add(item);
                    RebarDesglose_GrupoBarras_H _RebarDesglose_GrupoBarrasNew = null;
                    if (item.IsTraslapable == false || !item.SepuedeTraslaparSUperior())
                    {
                        _RebarDesglose_GrupoBarrasNew = RebarDesglose_GrupoBarras_H.Creador_RebarDesglose_GrupoBarras(NuevoGrupoBarras_Colineal, config_EspecialElv.Trasform_);
                        _RebarDesglose_GrupoBarrasNew.ObtenerCAntidadBArras();
                        GruposRebarMismaLinea_Colineal.Add(_RebarDesglose_GrupoBarrasNew);
                        continue;
                    }

                    var listaGrupo_Colineal = listaBArras_sinLat
                        .Where(c => (!c.ptoInicial.IsAlmostEqualTo(item.ptoInicial)) && // para no selecionar el mismo
                                    c.IsTraslapable &&
                                    UtilDesglose.IsCollinear_barraDesglose(item.curvePrincipal, c.curvePrincipal, Util.MmToFoot(Math.Max(item.diametroMM, c.diametroMM))))
                        .OrderBy(c => c.ptoInicial.Z)
                        .ToList();


                    //busca dentro del gurpo colineal
                    for (int j = 0; j < listaGrupo_Colineal.Count; j++)
                    {
                        RebarDesglose_Barras_H barra_colineales = listaGrupo_Colineal[j];

                        // cuando el pto inicial de la sigueinte barra no esta contendia en la actual
                        if (!BarraAnalizada.curvePrincipal.Contains((barra_colineales.ptoFinal.Z> barra_colineales.ptoInicial.Z? barra_colineales.ptoInicial: barra_colineales.ptoFinal),
                                                                    Util.MmToFoot(Math.Max(barra_colineales.diametroMM, item.diametroMM)))) continue;

                        if (AgregarTraslapoToLista(BarraAnalizada, barra_colineales))
                            BarraAnalizada._rebarDesglose.TraslapoCOnbarras = _newTraslapoBarrasH;
                        //cambiar barra sigueinte a actual
                        BarraAnalizada = barra_colineales;

                        barra_colineales.analizadasuperior = true;
                        NuevoGrupoBarras_Colineal.Add(barra_colineales);
                    }

                    _RebarDesglose_GrupoBarrasNew = RebarDesglose_GrupoBarras_H.Creador_RebarDesglose_GrupoBarras(NuevoGrupoBarras_Colineal, config_EspecialElv.Trasform_);
                    _RebarDesglose_GrupoBarrasNew.ObtenerCAntidadBArras();
                    GruposRebarMismaLinea_Colineal.Add(_RebarDesglose_GrupoBarrasNew);
                }
            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener grupos de barras  ex:{ ex.Message} ");
                return false;
            }
            return true;
        }

        private bool AgregarTraslapoToLista(RebarDesglose_Barras_H barraAnalizada_inf, RebarDesglose_Barras_H barra_colineales_supe)
        {
            _newTraslapoBarrasH = null;
            try
            {
                CrearTrasformadaSobreVectorDesg Trasform = barraAnalizada_inf._rebarDesglose.trasform;
                XYZ ptoInicial_trans = (barraAnalizada_inf.ptoInicial.Z < barraAnalizada_inf.ptoFinal.Z ? barraAnalizada_inf.ptoFinal : barraAnalizada_inf.ptoInicial); 
                XYZ ptofinal_trans = (barra_colineales_supe.ptoInicial.Z< barra_colineales_supe.ptoFinal.Z? barra_colineales_supe.ptoInicial: barra_colineales_supe.ptoFinal);

                XYZ ptoInicial = Trasform.EjecutarTransformInvertida(ptoInicial_trans);
                XYZ ptofinal = Trasform.EjecutarTransformInvertida(ptofinal_trans);

                ptoInicial = _view.NH_ObtenerPtoSObreVIew(ptoInicial);
                ptofinal = _view.NH_ObtenerPtoSObreVIew(ptofinal);
                TipobarraH tipobarr = barraAnalizada_inf._rebarDesglose.TipobarraH_;
                double largoTraslapo = ptoInicial.DistanceTo(ptofinal);

                if (largoTraslapo < Util.CmToFoot(3)) return false;

                 _newTraslapoBarrasH = new TraslapoBarrasH(ptoInicial_trans, ptofinal_trans, ptoInicial, ptofinal, tipobarr, largoTraslapo);

                if (_newTraslapoBarrasH == null) return false;

                ListaTraslapoBarrasH.Add(_newTraslapoBarrasH);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
    }
}
