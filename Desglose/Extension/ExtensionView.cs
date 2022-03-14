using Autodesk.Revit.DB;
using Desglose.Ayuda;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Extension
{
    public static class ExtensionView
    {

        public static string ObtenerNombreIsDependencia(this View _view)
        {
            string nombreActua = _view.Name;
            Parameter _paraDependency = _view.GetParameter2("Dependency");

            if (_paraDependency == null) return nombreActua;

            string IsDepend = _paraDependency.AsString();
            if (IsDepend == null) return nombreActua;
            if (IsDepend == "Primary" || IsDepend == "Independent" || (!IsDepend.Contains("Dependent on"))) return nombreActua;

            nombreActua = _paraDependency.AsString().Replace("Dependent on ", "");

            return nombreActua;
        }
        public static bool IsDependencia(this View _view)
        {
            string nombreActua = _view.Name;
            Parameter _paraDependency = _view.GetParameter2("Dependency");

            if (_paraDependency == null) return false;

            string IsDepend = _paraDependency.AsString();
            if (IsDepend == null) return false;
            if (IsDepend == "Primary" || IsDepend == "Independent" || (!IsDepend.Contains("Dependent on"))) return false;

            return true;

        }

        public static XYZ ViewDirection6(this View _view) => _view.ViewDirection.Redondear(8);
        public static XYZ RightDirection6(this View _view) => _view.RightDirection.Redondear(8);

        public static string ObtenerNombre_ViewNombre(this View _ViewMantenerBArras)
        {
            string NombreAntiguoVista = "";
            try
            {
                Parameter nombrePara = ParameterUtil.FindParaByName(_ViewMantenerBArras, "ViewNombre");
                if (nombrePara != null)
                {
                    NombreAntiguoVista = (nombrePara.AsString() == null ? "" : nombrePara.AsString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"'ObtenerNombreAntiguoVista' ex:{ex.Message}");

            }
            return NombreAntiguoVista;
        }
        public static string ObtenerNombre_TipoEstructura(this View _ViewMantenerBArras)
        {
            string TipoEstructura = "";
            try
            {
                Parameter nombrePara = ParameterUtil.FindParaByName(_ViewMantenerBArras, "TIPO DE ESTRUCTURA (VISTA)");
                if (nombrePara != null)
                {
                    TipoEstructura = (nombrePara.AsString() == null ? "" : nombrePara.AsString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"'ObtenerNombreAntiguoVista' ex:{ex.Message}");

            }
            return TipoEstructura;
        }



        public static XYZ NH_ObtenerPtoSObreVIew(this View _view, XYZ ptoInicia)
        {
            XYZ resul = XYZ.Zero;
            try
            {
                resul = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano(_view.ViewDirection, _view.Origin, ptoInicia);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'obtenerPtoSObreVIew' ex: {ex.Message}");

            }
            return resul;
        }

        public static void DesactivarViewTemplate(this View _view)
        {
            try
            {
                Document _doc = _view.Document;

                Parameter par = _view.GetParameters("View Template").FirstOrDefault();

                int valor = par.Id.IntegerValue;

                using (Transaction tr = new Transaction(_doc, "NoneView Template"))
                {
                    tr.Start();

                    par.Set(new ElementId(-1));
                    tr.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex.Message} ");
            }
        }
    }
}
