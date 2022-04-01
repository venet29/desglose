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
 
            //string nombreFasmiliaBase = "";
            //switch (_RebarElevDTO.tipobarraV)
            //{
            //    case TipoPataBarra.BarraVPataInicial:
            //        return new GeomeTagPataInicialH(_uiapp, _RebarElevDTO);
            //    case TipoPataBarra.BarraVPataFinal:
            //        return new GeomeTagPataFinalH(_uiapp, _RebarElevDTO);
            //    case TipoPataBarra.BarraVSinPatas:
            //        return new GeomeTagSinPataH(_uiapp, _RebarElevDTO);
            //    case TipoPataBarra.BarraVPataAmbos:
            //        return new GeomeTagPataAmbosH(_uiapp, _RebarElevDTO);
            //    default:
            //        return new GeomeTagNull();
            //}



            switch (_RebarElevDTO.tipoBarra)
            {
                case TipoRebarElev.Sinpata:
                    return new GeomeTagSinPataH(_uiapp, _RebarElevDTO);
                case TipoRebarElev.PataInferior:
                    return new GeomeTagPataInicialH(_uiapp, _RebarElevDTO);
                case TipoRebarElev.PataSuperior:
                    return new GeomeTagPataFinalH(_uiapp, _RebarElevDTO);
                case TipoRebarElev.AmbasPata:
                    return new GeomeTagPataAmbosH(_uiapp, _RebarElevDTO);
                default:
                    return new GeomeTagNull();
            }
     
        }


    }

}
