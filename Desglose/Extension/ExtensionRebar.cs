using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Desglose.Ayuda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Extension
{
    public static class ExtensionRebar
    {

        public static double ObtenerDiametroFoot(this Rebar rebar)
        {
            string diamString = rebar.get_Parameter(BuiltInParameter.REBAR_BAR_DIAMETER).AsValueString().Replace("mm", "").Trim();
            double diamInt = rebar.get_Parameter(BuiltInParameter.REBAR_BAR_DIAMETER).AsDouble();
            //if (Util.IsNumeric(diamString))
            //{
            //    diamInt = Util.MmToFoot(Util.ConvertirStringInDouble(diamString));
            //}
            return diamInt;
        }

        public static int ObtenerDiametroInt(this Rebar rebar)
        {
            string diamString = rebar.get_Parameter(BuiltInParameter.REBAR_BAR_DIAMETER).AsValueString().Replace("mm", "").Trim();
            int diamInt = 0;
            if (Util.IsNumeric(diamString))
            {
                diamInt = Util.ConvertirStringInInteger(diamString);
            }
            return diamInt;
        }


        public static double ObtenerEspaciento_cm(this Rebar rebar)
        {
            double espa = rebar.get_Parameter(BuiltInParameter.REBAR_ELEM_BAR_SPACING).AsDouble();
            if (espa == 0)
                espa = rebar.MaxSpacing;
            return Util.FootToCm(espa);
        }
        public static XYZ ObtenerInicioCurvaMasLarga(this Rebar rebar)
        {
            var getdrive = rebar.GetCenterlineCurves(false, false, true, MultiplanarOption.IncludeOnlyPlanarCurves, 0).MinBy(c => -c.Length);

            return getdrive.GetEndPoint(0);
        }

        public static string ObtenerTipoBArra_string(this Rebar rebar)
        {
            var nombreBarraTipo = ParameterUtil.FindParaByName(rebar.Parameters, "BarraTipo")?.AsString();

            if (nombreBarraTipo == null)
            {
                Util.ErrorMsg("Error al obtener 'BarraTipo'");
                return "";
            }
            return nombreBarraTipo;
        }


        public static TipoRebar ObtenerTipoBArra_TipoRebar(this Rebar rebar)
        {
            var valorstring = ObtenerTipoBArra_string(rebar);
            var _barraTipo = EnumeracionBuscador.ObtenerEnumGenerico(TipoRebar.NONE, valorstring);

            return _barraTipo;
        }

    }
}
