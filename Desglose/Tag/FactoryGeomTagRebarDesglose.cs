using Desglose.Ayuda;
using Desglose.DTO;
using Desglose.Tag;
using Autodesk.Revit.UI;
using Desglose.Tag.TipoBarraH;
using Desglose.Tag.TipoBarraV;

namespace Desglose.Tag
{
    //public interface IGeometriaTag
    //{
    //    List<TagBarra> listaTag { get; set; }
    //    void CAlcularPtosDeTAg(bool IsGarficarEnForm=false);
    //}
    public class FactoryGeomTagRebarDesglose
    {

        public static IGeometriaTag CrearIGeomTagRebarDesaglose(UIApplication _uiapp, RebarElevDTO _RebarElevDTO)
        {
       
            switch (_RebarElevDTO.tipoBarra)
            {

                case TipoRebarElev.Estribo:
                    return new GeomeTagEstribo(_uiapp, _RebarElevDTO);
                case TipoRebarElev.EstriboTraba:
                    return new GeomeTagTraba(_uiapp, _RebarElevDTO);
                case TipoRebarElev.EstriboViga:
                    return new GeomeTagEstriboViga(_uiapp, _RebarElevDTO);
                case TipoRebarElev.EstriboVigaTraba:
                    return new GeomeTagTrabaViga(_uiapp, _RebarElevDTO);


                default:
                    return  new GeomeTagNull(); 

            }
        }

    }

}
