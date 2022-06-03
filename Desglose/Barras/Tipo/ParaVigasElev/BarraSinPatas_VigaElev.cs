using Desglose.DTO;
using Desglose.Tag;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using Desglose.Ayuda;


namespace Desglose.Calculos.Tipo.ParaVigasElev

{
    public class BarraSinPatas_VigaElev : AARebarLosa_desglose_VigaElev, IRebarLosa_Desglose
    {
        private RebarElevDTO _RebarInferiorDTO;


        public double mayorDistancia { get; set; }

        public BarraSinPatas_VigaElev(UIApplication uiapp, RebarElevDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
        {
    
            this._RebarInferiorDTO = _rebarInferiorDTO;
            _newGeometriaTag = newGeometriaTag;
            _largoPataInclinada = _rebarInferiorDTO.LargoPata;

            _Prefijo_F = "F=";

        }

        public bool M1A_IsTodoOK()
        {
            //if (!M1_1_DatosBarra3d()) return false;
            DesplazamientoSegunLInea();
            if (!M1_2_DatosBarra2d()) return false;
            if (!M1_3_PAthSymbolTAG()) return false;

            return true;
        }


        #region comprobacion2

        public bool M1_2_DatosBarra2d()
        {
            ladoAB_pathSym = Line.CreateBound(PtoIniConDesplazamineto, PtoFinConDesplazamineto);
            _largoTotal = (Math.Round(Util.FootToCm(ladoAB_pathSym.Length), 0)).ToString();
        
            CargarPAratrosSHARE();

            //ivitar dibujar la primeralinea
            if (!(_RebarInferiorDTO._rebarDesglose.TipobarraH_ == TipobarraH.Linea1INF || _RebarInferiorDTO._rebarDesglose.TipobarraH_ == TipobarraH.Linea1SUP))
                OBtenerListaFalsoPAthSymbol();
            return true;

        }
        #endregion


        public bool M1_3_PAthSymbolTAG()
        {
            ObtenerPAthSymbolTAG();
            return true;
        }



    }
}
