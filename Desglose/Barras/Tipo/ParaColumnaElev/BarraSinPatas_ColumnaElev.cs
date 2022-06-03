﻿using Desglose.DTO;
using Desglose.Tag;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using Desglose.Ayuda;


namespace Desglose.Calculos.Tipo.ParaColumnaElev

{
    public class BarraSinPatas_ColumnaElev : ARebarLosa_desglose, IRebarLosa_Desglose
    {
        private RebarElevDTO _RebarInferiorDTO;
        //private string _largoTotal;
        private XYZ direccionMuevenBarrasFAlsa;

        public double mayorDistancia { get; set; }

        public BarraSinPatas_ColumnaElev(UIApplication uiapp, RebarElevDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
        {
    
            this._RebarInferiorDTO = _rebarInferiorDTO;
            _newGeometriaTag = newGeometriaTag;
            _largoPataInclinada = _rebarInferiorDTO.LargoPata;
            direccionMuevenBarrasFAlsa=_rebarInferiorDTO.Config_EspecialElv.direccionMuevenBarrasFAlsa;
            _Prefijo_F = "F=";
            
            if (_rebarInferiorDTO.Config_EspecialElv.ListaPAraShare != null)
                listaPArametroSharenh = _rebarInferiorDTO.Config_EspecialElv.ListaPAraShare;
        }

        public bool M1A_IsTodoOK()
        {
            //if (!M1_1_DatosBarra3d()) return false;
            if (!M1_2_DatosBarra2d()) return false;
            if (!M1_3_PAthSymbolTAG()) return false;

            return true;
        }


        #region comprobacion2

        public bool M1_2_DatosBarra2d()
        {
            ladoAB_pathSym = Line.CreateBound(_RebarInferiorDTO.ptoini, _RebarInferiorDTO.ptofinal);
            _largoTotal = (Math.Round(Util.FootToCm(ladoAB_pathSym.Length), 0)).ToString();

            _ptoTexto = (_RebarInferiorDTO.ptoini + _RebarInferiorDTO.ptofinal) / 2 + -direccionMuevenBarrasFAlsa * Util.CmToFoot(8);

            //para crear texto
            //if(_RebarInferiorDTO.Id==-1)
            //    _textoBArras = $" {_RebarInferiorDTO.Clasificacion} {_RebarInferiorDTO.cantidadBarras}Ø{_RebarInferiorDTO.diametroMM} L={_largoTotal}";
            //else
            //    _textoBArras = $" {_RebarInferiorDTO.Clasificacion} {_RebarInferiorDTO.cantidadBarras}Ø{_RebarInferiorDTO.diametroMM} L={_largoTotal}  id:{_RebarInferiorDTO.Id}";

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
