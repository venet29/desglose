using Desglose.DTO;
using Desglose.Tag;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using Desglose.Ayuda;


namespace Desglose.Calculos.Tipo.ParaVigasElev

{
    public class EstriboVigaTrabaElev_VigaElev : ARebarLosa_desgloseEstribo_VigaElev, IRebarLosa_Desglose
    {

       public double mayorDistancia { get; set; }
     

        public EstriboVigaTrabaElev_VigaElev(UIApplication uiapp, RebarElevDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
        {
            _newGeometriaTag = newGeometriaTag;

        }

        public bool M1A_IsTodoOK()
        {
            CargarPAratrosSHAR_Estribo();

            ObtenerPAthSymbolTAG();

            return true;
        }





    }
}
