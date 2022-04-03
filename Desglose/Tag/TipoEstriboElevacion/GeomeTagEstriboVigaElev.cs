
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Desglose.Ayuda;
using Desglose.DTO;
using System;

namespace Desglose.Tag.TipoEstriboElevacion
{
    public class GeomeTagEstriboVigaElev : GeomeTagEstriboBase, IGeometriaTag
    {
        private readonly TipoEstriboConfig tipoEstriboConfig;

        public GeomeTagEstriboVigaElev(UIApplication _uiapp, RebarElevDTO _RebarElevDTO) :
            base(_uiapp, _RebarElevDTO )
        {
          //  this.tipoEstriboConfig = tipoEstriboConfig;
        }

        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                double AnguloRadian = args.angulorad;
                M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian);
                M2_CAlcularPtosDeTAg();
                M3_DefinirRebarShape();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar TagEstrivoViga  ex:${ex.Message}");
                return false;
            }
            return true;

        }

        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagEstriboVigaElev> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagEstriboBase _geomeTagBase)
        {

            _geomeTagBase.TagP0_Lateral.IsOk = false;
            _geomeTagBase.TagP0_Traba.IsOk = false;
         

        }


    }
}
