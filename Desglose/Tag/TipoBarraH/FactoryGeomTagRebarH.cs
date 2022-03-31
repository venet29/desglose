using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Desglose.Ayuda;
using System;

namespace Desglose.Tag.TipoBarraH
{
    //public interface IGeometriaTag
    //{
    //    List<TagBarra> listaTag { get; set; }
    //    void CAlcularPtosDeTAg(bool IsGarficarEnForm=false);
    //}
    public class FactoryGeomTagRebarH
    {

     
        public static IGeometriaTag CrearGeometriaTagH(UIApplication uiapp, TipoPataBarra tipoBarrav, XYZ ptoIni, XYZ ptoFin, XYZ posiciontag,  XYZ DireccionEfierrado)
        {
            Document doc = uiapp.ActiveUIDocument.Document;
  

            //string nombreFasmiliaBase = "";
            switch (tipoBarrav)
            {
                case TipoPataBarra.BarraVPataInicial:
                    return new GeomeTagPataInicialH(doc, ptoIni + DireccionEfierrado, ptoFin + DireccionEfierrado, posiciontag + DireccionEfierrado);
                case TipoPataBarra.BarraVPataFinal:
                    return new GeomeTagPataFinalH(doc, ptoIni + DireccionEfierrado, ptoFin + DireccionEfierrado, posiciontag + DireccionEfierrado);
                case TipoPataBarra.BarraVSinPatas:
                    return new GeomeTagSinPataH(doc, ptoIni + DireccionEfierrado, ptoFin + DireccionEfierrado, posiciontag + DireccionEfierrado);
                case TipoPataBarra.BarraVPataAmbos:
                    return new GeomeTagPataAmbosH(doc, ptoIni + DireccionEfierrado, ptoFin + DireccionEfierrado, posiciontag + DireccionEfierrado);
                default:
                    return new GeomeTagNull();
            }

        }

        internal static IGeometriaTag CrearGeometriaTagH(UIApplication uiapp, object tipobarraV, XYZ ptoini, XYZ ptofinal, object ptoPosicionTAg, object p)
        {
            throw new NotImplementedException();
        }
    }

}
