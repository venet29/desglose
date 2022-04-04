using Desglose.DTO;
using Desglose.Entidades;
using Desglose.Tag;
using Desglose.Ayuda;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;

namespace Desglose.Calculos.Tipo.ParaElevVigas

{
    public class BarraPataInicialH : AARebarLosa_desgloseH, IRebarLosa_Desglose
    {
        private RebarElevDTO _RebarInferiorDTO;


        public double mayorDistancia { get; set; }


        public BarraPataInicialH(UIApplication uiapp, RebarElevDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
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

            List<WraperRebarLargo> listaCuvas = _RebarInferiorDTO.listaCUrvas;
            double pataInicial = listaCuvas[0]._curve.Length;
            XYZ direcionPAtaInferior = -listaCuvas[0].direccion;



         //   XYZ PtoIniConDesplazamineto = _RebarInferiorDTO.ptoini + DesplazamietoPOrLInea;
           // XYZ PtoFinConDesplazamineto = _RebarInferiorDTO.ptofinal + DesplazamietoPOrLInea;

            ladoAB_pathSym = Line.CreateBound(PtoIniConDesplazamineto + direcionPAtaInferior, PtoIniConDesplazamineto);
            ladoBC_pathSym = Line.CreateBound(PtoIniConDesplazamineto, PtoFinConDesplazamineto);

             _texToLargoParciales = $"({ Math.Round(Util.FootToCm(ladoAB_pathSym.Length), 0) }+{ Math.Round(Util.FootToCm(ladoBC_pathSym.Length), 0) })";

             _largoTotal = (Math.Round(Util.FootToCm(ladoAB_pathSym.Length), 0) + Math.Round(Util.FootToCm(ladoBC_pathSym.Length), 0)).ToString();

            _ptoTexto = (_RebarInferiorDTO.ptoini + _RebarInferiorDTO.ptofinal) / 2;
            
            CargarPAratrosSHARE();

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
