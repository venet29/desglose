using Desglose.Ayuda;
using Desglose.Entidades;
using Desglose.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Model
{
    public class DatosHost
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;
        private RebarDesglose rebarDesglose;
        private XYZ aux_ptoMedio;
        private XYZ aux_direccion;

        public Element host { get; set; }
        public PlanarFace CaraCentral { get; set; }
        public XYZ CentroHost { get; set; }
        public double LargoMAximoHost_foot { get; set; }
        public XYZ Direccion_ParalelaView { get; internal set; }
        public XYZ ptoInicia_CentroHost { get; private set; }
        public XYZ ptoFin_CentroHost { get; private set; }

        public DatosHost(UIApplication _uiapp, RebarDesglose rebarDesglose)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this.rebarDesglose = rebarDesglose;
        }




        public bool ObtenerPtoMedioYDireccion()
        {
            try
            {
                WraperRebarLargo curvaPrinciplar = rebarDesglose.ListaCurvaBarras.Find(c => c.IsBarraPrincipal);
                aux_ptoMedio = curvaPrinciplar.ptoMedio;
                aux_direccion = rebarDesglose.CurvaMasLargo_WraperRebarLargo.direccion;
                //en pilares carainferior: caraabajo  //  en vigas cara vertical inicial izquioerda o derecha final

            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener datos de host de barras  ex:{ex.Message} ");
                return false; ;
            }
            return true;
        }
        public bool ObtenerPtoMedio_conestribo()
        {
            try
            {
                WraperRebarLargo curvaPrinciplar = rebarDesglose.ListaCurvaBarras.Find(c => c.IsBarraPrincipal);
                // aux_ptoMedio = curvaPrinciplar._Trasform.EjecutarTransformInvertida(curvaPrinciplar.ptoMedio);

                aux_direccion = rebarDesglose._normal;
                //en pilares carainferior: caraabajo  //  en vigas cara vertical inicial izquioerda o derecha final

            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener datos de host de barras  ex:{ex.Message} ");
                return false; ;
            }
            return true;
        }

        public bool ObtenerCentroPilarOmUro(XYZ direcionRAyo)
        {
            aux_direccion = direcionRAyo;
            return ObtenerCentroPilarOmUro();
        }

        public bool ObtenerCentroPilarOmUro()
        {
            try
            {
                //WraperRebarLargo curvaPrinciplar =rebarDesglose.ListaCurvaBarras.Find(c=>c.IsBarraPrincipal);
                if (!ObtenerHost()) return false;

                //XYZ _ptoMedio=rebarDesglose.trasform.EjecutarTransformInvertida(curvaPrinciplar.ptoMedio);
                //en pilares carainferior: caraabajo  //  en vigas cara vertical inicial izquioerda o derecha final
                CaraCentral = host.ObtenerCaraSegun_Direccion(aux_direccion);
                XYZ _aux_p1 = CaraCentral.GetCenterOfFace();


                var Cara2 = host.ObtenerCaraSegun_Direccion(-aux_direccion);
                XYZ _aux_pt2 = Cara2.ProjectNH(_aux_p1);



                if (CaraCentral == null)
                {
                    UtilDesglose.ErrorMsg($"No se pudo obtenerDatos de cara de muro ");
                    return false;
                }
                CentroHost = CaraCentral.GetCenterOfFace();
                LargoMAximoHost_foot = CaraCentral.MaximoladoLArgo();

                var listacurva = CaraCentral.ObtenerListaCurvas();

                var aux_Direccion_ParalelaView = CaraCentral.ObtenerListaCurvas().Where(c => !UtilDesglose.IsParallel(((Line)c).Direction, _view.ViewDirection)).FirstOrDefault();
                Direccion_ParalelaView = ((Line)aux_Direccion_ParalelaView).Direction;


                ptoInicia_CentroHost = CentroHost - Direccion_ParalelaView * LargoMAximoHost_foot / 2;
                ptoFin_CentroHost = CentroHost + Direccion_ParalelaView * LargoMAximoHost_foot / 2;
            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener datos de host de barras  ex:{ex.Message} ");
                return false; ;
            }
            return true;
        }


        public bool ObtenerHost()
        {
            try
            {
                if (_doc == null) return false;
                if (rebarDesglose == null) return false;

                host = _doc.GetElement(rebarDesglose._rebar?.GetHostId());
            }
            catch (Exception)
            {

                return false; ;
            }
            return true;
        }
    }
}
