using Desglose.DTO;
using Desglose.Tag;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using Desglose.Ayuda;


namespace Desglose.Calculos.Tipo

{
    public class EstriboVigaElv : ARebarLosa_Estribo, IRebarLosa_Desglose
    {

       public double mayorDistancia { get; set; }
     

        public EstriboVigaElv(UIApplication uiapp, RebarElevDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
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
