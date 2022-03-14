﻿using Desglose.Ayuda;
using Desglose.DTO;
using Desglose.Entidades;
using Desglose.Ayuda;
using Desglose.Extension;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Geometria
{
    class Traba3ladosOrientadaOrtogonal_V
    {
        private RebarElevDTO rebarElevDTO;

        public Traba3ladosOrientadaOrtogonal_V(RebarElevDTO rebarElevDTO)
        {
            this.rebarElevDTO = rebarElevDTO;
        }

      //  public XYZ UbicacionDeFylargo { get; set; }

        public XYZ UbicacionDeF { get; set; }
        public XYZ UbicacionDeL { get; private set; }

        public XYZ UbicacionSup { get; set; }
        public XYZ UbicacionIZq { get; set; }
        public string UbicacionSup_ValorLArgo { get;  set; }

        public bool calcularUbiaciontexto()
        {

            try
            {
                var sololist = rebarElevDTO.ListaCurvaBarrasFinal_conCurva.Where(c => c.TipoCurva == TipoCUrva.linea &&
                                                                                 c.FijacionInicial == FijacionRebar.fijo &&
                                                                                 c.FijacionFinal == FijacionRebar.fijo).OrderByDescending(c => c._curve.Length).ToList();
                if (sololist.Count == 0) return false;

                var ladoMAsLArgo = sololist[0];

                UbicacionSup_ValorLArgo = Math.Round(Util.FootToCm(ladoMAsLArgo._curve.Length), 0).ToString();

                if (Math.Abs(Util.GetProductoEscalar(ladoMAsLArgo.direccion, rebarElevDTO._View.RightDirection)) > 0.8) //paralela
                {

                    UbicacionSup = ladoMAsLArgo.PtoMedioTransformada - rebarElevDTO._viewOriginal.ViewDirection* Util.CmToFoot(5);


                   // UbicacionDeFylargo = ladoMAsLArgo.PtoMedioTransformada + rebarElevDTO._viewOriginal.ViewDirection * Util.CmToFoot(10);
                    UbicacionDeF = ladoMAsLArgo.PtoMedioTransformada + rebarElevDTO._viewOriginal.ViewDirection * Util.CmToFoot(10);
                    UbicacionDeL = ladoMAsLArgo.PtoMedioTransformada + rebarElevDTO._viewOriginal.ViewDirection * Util.CmToFoot(15);
                }
                else
                {
                    if (Util.GetProductoEscalar((ladoMAsLArgo.ptoFinal- ladoMAsLArgo.ptoInicial).AsignarZ(0).Normalize(), -rebarElevDTO._View.RightDirection) > 0)
                    { //entrado
                        //UbicacionDeFylargo = ladoMAsLArgo.PtoInicialTransformada + rebarElevDTO._viewOriginal.ViewDirection * Util.CmToFoot(15);

                        UbicacionDeF = ladoMAsLArgo.PtoInicialTransformada + rebarElevDTO._viewOriginal.ViewDirection * Util.CmToFoot(10);
                        UbicacionDeL = ladoMAsLArgo.PtoInicialTransformada + rebarElevDTO._viewOriginal.ViewDirection * Util.CmToFoot(15);
                    }
                    else
                    { // saliendo 

                       // UbicacionDeFylargo = ladoMAsLArgo.PtoFinalTransformada + rebarElevDTO._viewOriginal.ViewDirection * Util.CmToFoot(15);
                        UbicacionDeF = ladoMAsLArgo.PtoFinalTransformada + rebarElevDTO._viewOriginal.ViewDirection * Util.CmToFoot(10);
                        UbicacionDeL = ladoMAsLArgo.PtoFinalTransformada + rebarElevDTO._viewOriginal.ViewDirection * Util.CmToFoot(15);
                    }

                    UbicacionSup = ladoMAsLArgo.PtoMedioTransformada + rebarElevDTO._viewOriginal.RightDirection * Util.CmToFoot(10);

                } //entraanod en vista

            }
            catch (Exception ex)
            {


                UtilDesglose.ErrorMsg($"Error al obtener ubicacion texto  ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
