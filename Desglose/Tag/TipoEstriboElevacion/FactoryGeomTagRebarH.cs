using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Desglose.Ayuda;
using Desglose.DTO;
using System;

namespace Desglose.Tag.TipoEstriboElevacion
{
    //public interface IGeometriaTag
    //{
    //    List<TagBarra> listaTag { get; set; }
    //    void CAlcularPtosDeTAg(bool IsGarficarEnForm=false);
    //}
    public class FactoryGeomTagRebarEstriboElev
    {

     
        public static IGeometriaTag CrearGeometriaTagEstriboElev(UIApplication _uiapp,RebarElevDTO _RebarElevDTO)
        {
 
            switch (_RebarElevDTO.tipoBarra)
            {
          
                case TipoRebarElev.EstriboViga:
                    return new GeomeTagEstriboVigaElev(_uiapp, _RebarElevDTO);
                case TipoRebarElev.EstriboVigaLatelaElev:
                    return new GeomeTagLateralesVigaElev(_uiapp, _RebarElevDTO);
                case TipoRebarElev.EstriboVigaTraba:
                    return new GeomeTagTrabaVigaElev(_uiapp, _RebarElevDTO);
                default:
                    return new GeomeTagNull();
            }
     
        }


    }

}
