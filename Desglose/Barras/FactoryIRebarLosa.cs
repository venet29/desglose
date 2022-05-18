using Desglose.Ayuda;
using Desglose.Calculos.Tipo;
using Desglose.Calculos.Tipo.ParaElevEstriboPilar;
using Desglose.Calculos.Tipo.ParaPlanta;
using Desglose.DTO;
using Autodesk.Revit.UI;
using Desglose.Tag;
using Desglose.Calculos.Tipo.ParaElevVigas;
using Desglose.Calculos.Tipo.ParaElevPilar;

namespace Desglose.Calculos
{
    public class FactoryIRebarDesglose
    {

        public static IRebarLosa_Desglose CrearIRebarLosa(UIApplication _uiapp, RebarElevDTO _RebarElevDTO, IGeometriaTag _newIGeometriaTag)
        {

            
            switch (_RebarElevDTO.tipoBarra)
            {
                case TipoRebarElev.Sinpata:
                    return new BarraSinPatas(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.PataInferior:
                    return new BarraPataInicial(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.PataSuperior:
                    return new BarraPataSuperior(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.AmbasPata:
                    return new BarraPataAmbos(_uiapp, _RebarElevDTO, _newIGeometriaTag);


                    //para corte los 4 inferiores
                case TipoRebarElev.Estribo:
                    return new BarraEstriboTransConCurva_Plata(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.EstriboTraba:
                    return new BarraTrabaEstriboConCurva_Plata(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.EstriboViga:
                    return new BarraEstriboTransConCurva_Elev(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.EstriboVigaTraba:
                    return new BarraTrabaEstriboConCurva_Elev(_uiapp, _RebarElevDTO, _newIGeometriaTag);

                    // elevacion viga
                case TipoRebarElev.SinpataH:
                    return new BarraSinPatasH(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.PataInferiorH:
                    return new BarraPataInicialH(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.PataSuperiorH:
                    return new BarraPataSuperiorH(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.AmbasPataH:
                    return new BarraPataAmbosH(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.EstriboVigaElv:
                    return new EstriboVigaElv(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.EstriboVigaLatelaElev:
                    return new EstriboVigaLatelaElev(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.EstriboVigaTrabaElev:
                    return new EstriboVigaTrabaElev(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                default:
                    return new fx_null();

            }

    }



}
}
