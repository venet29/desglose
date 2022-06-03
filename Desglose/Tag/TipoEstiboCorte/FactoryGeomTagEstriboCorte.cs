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

                case TipoRebarElev.Estribo_ColumnaCorte:
                    return new GeomeTagEstriboCorte(_uiapp, _RebarElevDTO);
                case TipoRebarElev.EstriboTraba_ColumnaCorte:
                    return new GeomeTagTrabaCorte(_uiapp, _RebarElevDTO);
                case TipoRebarElev.Estribo_VigaCorte:
                    return new GeomeTagEstriboVigaCorte_H(_uiapp, _RebarElevDTO);
                case TipoRebarElev.EstriboTraba_VigaCorte:
                    return new GeomeTagTrabaVigaCorte_H(_uiapp, _RebarElevDTO);


                default:
                    return  new GeomeTagNull(); 

            }
        }

    }

}
