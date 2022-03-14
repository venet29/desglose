﻿using Desglose.DTO;
using Desglose.Tag;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using Desglose.Ayuda;
using Desglose.Entidades;

namespace Desglose.Calculos.Tipo

{
    public class BarraPataAmbos : ARebarLosa_desglose, IRebarLosa_Desglose
    {
        private RebarElevDTO _RebarInferiorDTO;
        public string _texToLargoParciales { get; private set; }
        public string _largoTotal { get; private set; }

        public double mayorDistancia { get; set; }

        public BarraPataAmbos(UIApplication uiapp, RebarElevDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
        {
            this._RebarInferiorDTO = _rebarInferiorDTO;
            _newGeometriaTag = newGeometriaTag;
            _largoPataInclinada = _rebarInferiorDTO.LargoPata;
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
            List<WraperRebarLargo> listaCuvas = _RebarInferiorDTO.listaCUrvas;
            double pataSuperior = listaCuvas[2]._curve.Length;
            double pataInicial = listaCuvas[0]._curve.Length;

            ladoAB_pathSym = Line.CreateBound(_RebarInferiorDTO.ptoini + _RebarInferiorDTO.DireccionPataEnFierrado * pataInicial, _RebarInferiorDTO.ptoini);
             ladoBC_pathSym = Line.CreateBound(_RebarInferiorDTO.ptoini, _RebarInferiorDTO.ptofinal);
             ladoCD_pathSym = Line.CreateBound(_RebarInferiorDTO.ptofinal, _RebarInferiorDTO.ptofinal + _RebarInferiorDTO.DireccionPataEnFierrado * pataSuperior);

             _texToLargoParciales = $"({ Math.Round(Util.FootToCm(ladoAB_pathSym.Length), 0) }+{ Math.Round(Util.FootToCm(ladoBC_pathSym.Length), 0) }+{ Math.Round(Util.FootToCm(ladoCD_pathSym.Length), 0) })";

             _largoTotal = (Math.Round(Util.FootToCm(ladoAB_pathSym.Length), 0) + Math.Round(Util.FootToCm(ladoBC_pathSym.Length), 0) + Math.Round(Util.FootToCm(ladoCD_pathSym.Length))).ToString();


            _ptoTexto = (_RebarInferiorDTO.ptoini + _RebarInferiorDTO.ptofinal) / 2;
            if (_RebarInferiorDTO.Id == -1)
                _textoBArras = $" {_RebarInferiorDTO.Clasificacion} {_RebarInferiorDTO.cantidadBarras}Ø{_RebarInferiorDTO.diametroMM} L={_largoTotal}\n {_texToLargoParciales} ";
            else
                _textoBArras = $" {_RebarInferiorDTO.Clasificacion} {_RebarInferiorDTO.cantidadBarras}Ø{_RebarInferiorDTO.diametroMM} L={_largoTotal}  id:{_RebarInferiorDTO.Id}\n {_texToLargoParciales} ";
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
