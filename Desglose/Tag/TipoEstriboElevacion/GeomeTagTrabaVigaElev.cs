
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Desglose.Ayuda;
using Desglose.DTO;
using System;

namespace Desglose.Tag.TipoEstriboElevacion
{
    public class GeomeTagTrabaVigaElev : GeomeTagEstriboBase, IGeometriaTag
    {

        private readonly TipoEstriboConfig _tipoEstriboConfig;

        public GeomeTagTrabaVigaElev(UIApplication _uiapp, RebarElevDTO _RebarElevDTO) :
            base(_uiapp, _RebarElevDTO)
        {
 
        }

        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                double AnguloRadian = args.angulorad;
                M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian);
                M2_CAlcularPtosDeTAg();
                M2_RECAlcularPtosDeTAg();
                M3_DefinirRebarShape();
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
        public void M2_RECAlcularPtosDeTAg(bool IsGraficarEnForm = false)
        {

            XYZ p0_Trabas = null;

            switch (_tipoEstriboConfig)
            {
                case TipoEstriboConfig.E:
                    return;
                case TipoEstriboConfig.EL:
                    return;
                case TipoEstriboConfig.ET:
                    p0_Trabas = CentroBarra - new XYZ(0, 0, Util.CmToFoot(5));
                    break;
                case TipoEstriboConfig.ELT:
                    return;
                case TipoEstriboConfig.L:
                    return;
                case TipoEstriboConfig.LT:
                    p0_Trabas = CentroBarra - new XYZ(0, 0, Util.CmToFoot(5));
                    break;
                case TipoEstriboConfig.T:
                    p0_Trabas = CentroBarra + new XYZ(0, 0, Util.CmToFoot(10));
                    break;

            }

            AgregaroEditaPosicionTAgLitsta("TRABA", p0_Trabas);

        }
        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagTrabaVigaElev> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagEstriboBase _geomeTagBase)
        {
            _geomeTagBase.TagP0_Estribo.IsOk = false;
            _geomeTagBase.TagP0_Lateral.IsOk = false;

        }


    }
}
