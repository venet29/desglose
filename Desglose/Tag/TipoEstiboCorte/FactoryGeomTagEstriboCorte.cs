using Desglose.Ayuda;
using Desglose.DTO;
using Desglose.Tag;
using Autodesk.Revit.UI;
using Desglose.Tag.TipoBarraH;
using Desglose.Tag.TipoBarraV;
using Desglose.Tag.TipoEstiboCorte;

namespace Desglose.Tag
{
    //public interface IGeometriaTag
    //{
    //    List<TagBarra> listaTag { get; set; }
    //    void CAlcularPtosDeTAg(bool IsGarficarEnForm=false);
    //}
    public class FactoryGeomTagEstriboCorte
    {

        public static IGeometriaTag CrearIGeomTagRebarEstriboCorte(UIApplication _uiapp, RebarElevDTO _RebarElevDTO)
        {
       
            switch (_RebarElevDTO.tipoBarra)
            {

                case TipoRebarElev.Estribo:
                    return new GeomeTagEstriboCorte(_uiapp, _RebarElevDTO);
                case TipoRebarElev.EstriboTraba:
                    return new GeomeTagTrabaCorte(_uiapp, _RebarElevDTO);
                case TipoRebarElev.EstriboViga:
                    return new GeomeTagEstriboVigaCorte(_uiapp, _RebarElevDTO);
                case TipoRebarElev.EstriboVigaTraba:
                    return new GeomeTagTrabaVigaCorte(_uiapp, _RebarElevDTO);


                default:
                    return  new GeomeTagNull(); 

            }
        }

    }

}
