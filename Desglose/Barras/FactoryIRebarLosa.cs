using Desglose.Ayuda;
using Desglose.Calculos.Tipo;
using Desglose.DTO;
using Autodesk.Revit.UI;
using Desglose.Tag;
using Desglose.Calculos.Tipo.ParaColumnaCorte;
using Desglose.Calculos.Tipo.ParaColumnaElev;
using Desglose.Calculos.Tipo.ParaVigasCorte;
using Desglose.Calculos.Tipo.ParaVigasElev;

namespace Desglose.Calculos
{
    public class FactoryIRebarDesglose
    {

        public static IRebarLosa_Desglose CrearIRebarLosa(UIApplication _uiapp, RebarElevDTO _RebarElevDTO, IGeometriaTag _newIGeometriaTag)
        {

            
            switch (_RebarElevDTO.tipoBarra)
            {
                // para barras verticales de columna
                case TipoRebarElev.Sinpata:
                    return new BarraSinPatas_ColumnaElev(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.PataInferior:
                    return new BarraPataInicial_ColumnaElev(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.PataSuperior:
                    return new BarraPataSuperior_ColumnaElev(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.AmbasPata:
                    return new BarraPataAmbos_ColumnaElev(_uiapp, _RebarElevDTO, _newIGeometriaTag);


                //para corte de columna 
                case TipoRebarElev.Estribo_ColumnaCorte:
                    return new BarraEstriboConCurva_ColumnaCorte(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.EstriboTraba_ColumnaCorte:
                    return new BarraTrabaEstriboConCurva_ColumnaCorte(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                
                //para corte de viga 
                case TipoRebarElev.Estribo_VigaCorte:
                    return new BarraEstriboConCurva_VigaCorte(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.EstriboTraba_VigaCorte:
                    return new BarraTrabaEstriboConCurva_VigaCorte(_uiapp, _RebarElevDTO, _newIGeometriaTag);

                    // elevacion viga
                case TipoRebarElev.SinpataH:
                    return new BarraSinPatas_VigaElev(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.PataInferiorH:
                    return new BarraPataInicial_VigaElev(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.PataSuperiorH:
                    return new BarraPataSuperior_VigaElev(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.AmbasPataH:
                    return new BarraPataAmbos_VigaElev(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.EstriboVigaElv:
                    return new EstriboVigaElv_VigaElev(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.EstriboVigaLatelaElev:
                    return new EstriboVigaLatelaElev_VigaElev(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.EstriboVigaTrabaElev:
                    return new EstriboVigaTrabaElev_VigaElev(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                default:
                    return new fx_null();

            }

    }



}
}
