using Desglose.DTO;
using Desglose.Tag;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using Desglose.Ayuda;
using Desglose.Entidades;

namespace Desglose.Calculos.Tipo.ParaVigasElev

{
    public class BarraPataAmbos_VigaElev : AARebarLosa_desglose_VigaElev, IRebarLosa_Desglose
    {
        private RebarElevDTO _RebarInferiorDTO;
 

        public double mayorDistancia { get; set; }

        public BarraPataAmbos_VigaElev(UIApplication uiapp, RebarElevDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
        {
            this._RebarInferiorDTO = _rebarInferiorDTO;
            _newGeometriaTag = newGeometriaTag;
            _largoPataInclinada = _rebarInferiorDTO.LargoPata;
            _Prefijo_F = "F=";

            //if (_rebarInferiorDTO.Config_EspecialElv.ListaPAraShare != null)
            //    listaPArametroSharenh = _rebarInferiorDTO.Config_EspecialElv.ListaPAraShare;
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
            double pataSuperior = listaCuvas[2]._curve.Length;
            XYZ direcionPAtaSuperiopr = listaCuvas[2].direccion;
            double pataInicial = listaCuvas[0]._curve.Length;
            XYZ direcionPAtaInferior = -listaCuvas[0].direccion;


          //  XYZ PtoIniConDesplazamineto = _RebarInferiorDTO.ptoini + DesplazamietoPOrLInea;
            //XYZ PtoFinConDesplazamineto = _RebarInferiorDTO.ptofinal + DesplazamietoPOrLInea;

            ladoAB_pathSym = Line.CreateBound(PtoIniConDesplazamineto + direcionPAtaInferior, PtoIniConDesplazamineto);
            ladoBC_pathSym = Line.CreateBound(PtoIniConDesplazamineto, PtoFinConDesplazamineto);
            ladoCD_pathSym = Line.CreateBound(PtoFinConDesplazamineto, PtoFinConDesplazamineto + direcionPAtaSuperiopr * pataSuperior);

            _texToLargoParciales = $"({ Math.Round(Util.FootToCm(ladoAB_pathSym.Length), 0) }+{ Math.Round(Util.FootToCm(ladoBC_pathSym.Length), 0) }+{ Math.Round(Util.FootToCm(ladoCD_pathSym.Length), 0) })";

            _largoTotal = (Math.Round(Util.FootToCm(ladoAB_pathSym.Length), 0) + Math.Round(Util.FootToCm(ladoBC_pathSym.Length), 0) + Math.Round(Util.FootToCm(ladoCD_pathSym.Length))).ToString();

            CargarPAratrosSHARE();

            //ivitar dibujar la primeralinea
            if(!(_RebarInferiorDTO._rebarDesglose.TipobarraH_==TipobarraH.Linea1INF || _RebarInferiorDTO._rebarDesglose.TipobarraH_ == TipobarraH.Linea1SUP))
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
