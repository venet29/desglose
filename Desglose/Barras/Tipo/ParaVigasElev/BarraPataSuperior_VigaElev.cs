using Desglose.DTO;
using Desglose.Entidades;
using Desglose.Tag;
using Desglose.Ayuda;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;


namespace Desglose.Calculos.Tipo.ParaVigasElev

{
    public class BarraPataSuperior_VigaElev : AARebarLosa_desglose_VigaElev, IRebarLosa_Desglose
    {
        private RebarElevDTO _RebarInferiorDTO;

        public double mayorDistancia { get; set; }

        public BarraPataSuperior_VigaElev(UIApplication uiapp, RebarElevDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
        {
 
            this._RebarInferiorDTO = _rebarInferiorDTO;
            _newGeometriaTag = newGeometriaTag;


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
            double pataSuperior = listaCuvas.Find(c=> !c.IsBarraPrincipal)._curve.Length;
            XYZ direcionPAtaSuperiopr = listaCuvas[1].direccion;


            ladoAB_pathSym = Line.CreateBound(PtoIniConDesplazamineto, PtoFinConDesplazamineto);
            ladoBC_pathSym = Line.CreateBound(PtoFinConDesplazamineto, PtoFinConDesplazamineto + direcionPAtaSuperiopr * pataSuperior);

            ///
  

             _texToLargoParciales = $"({ Math.Round(Util.FootToCm(ladoAB_pathSym.Length), 0) }+{ Math.Round(Util.FootToCm(ladoBC_pathSym.Length), 0) })";
             _largoTotal = (Math.Round(Util.FootToCm(ladoAB_pathSym.Length), 0) + Math.Round(Util.FootToCm(ladoBC_pathSym.Length), 0)).ToString();

            _ptoTexto = (_RebarInferiorDTO.ptoini + _RebarInferiorDTO.ptofinal) / 2;

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
