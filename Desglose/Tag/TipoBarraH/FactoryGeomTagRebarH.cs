using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Desglose.Ayuda;
using Desglose.DTO;
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

     
        public static IGeometriaTag CrearGeometriaTagH(UIApplication _uiapp,RebarElevDTO _RebarElevDTO)
        {
 
            switch (_RebarElevDTO.tipoBarra)
            {
                case TipoRebarElev.SinpataH:
                    return new GeomeTagSinPataH(_uiapp, _RebarElevDTO);
                case TipoRebarElev.PataInferiorH:
                    return new GeomeTagPataInicialH(_uiapp, _RebarElevDTO);
                case TipoRebarElev.PataSuperiorH:
                    return new GeomeTagPataFinalH(_uiapp, _RebarElevDTO);
                case TipoRebarElev.AmbasPataH:
                    return new GeomeTagPataAmbosH(_uiapp, _RebarElevDTO);
                case TipoRebarElev.EstriboViga:
                    return new GeomeTagPataAmbosH(_uiapp, _RebarElevDTO);
                default:
                    return new GeomeTagNull();
            }
     
        }


    }

}
