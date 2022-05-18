using Desglose.Ayuda;
using Desglose.DTO;
using Desglose.Ayuda;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Desglose.Geometria
{
    public class EstribosRectagularesOrtogonales_H
    {
        private RebarElevDTO rebarElevDTO;

        public EstribosRectagularesOrtogonales_H(RebarElevDTO rebarElevDTO)
        {
            this.rebarElevDTO = rebarElevDTO;
        }

        public XYZ UbicacionDeF { get;  set; }
        public XYZ UbicacionDeL { get; private set; }
        public XYZ UbicacionSup { get; set; }
        public string UbicacionSup_ValorLArgo { get; private set; }
        public XYZ UbicacionIZq { get; set; }
        public string UbicacionIZq_ValorLArgo { get; private set; }

        public bool calcularUbiaciontexto()
        {

            try
            {
                var sololist = rebarElevDTO.ListaCurvaBarrasFinal_conCurva.Where(c => c.TipoCurva == TipoCUrva.linea &&
                                                                                 c.FijacionInicial == FijacionRebar.fijo &&
                                                                                 c.FijacionFinal == FijacionRebar.fijo).ToList();

                var sololist_sincurva = rebarElevDTO.listaCUrvas.Where(c => c.TipoCurva == TipoCUrva.linea &&
                                                                         c.FijacionInicial == FijacionRebar.fijo &&
                                                                         c.FijacionFinal == FijacionRebar.fijo).ToList();

                double Delta = 0;
                var cur1 = rebarElevDTO.ListaCurvaBarrasFinal_conCurva.Where(c => c.TipoCurva == TipoCUrva.arco).FirstOrDefault();

                if (cur1 != null)
                {
                    var radio = ((Arc)cur1._curve).Radius;
                    Delta = radio * 2 + rebarElevDTO.diametroFoot;
                }

                List<PtosCurvaAuxDTO> ListaptosTrans =
                    AyudaObtenerPtosTransformada.ObtenerPtosSINTransformados(sololist, rebarElevDTO._View);
                List<PtosCurvaAuxDTO> ListaptosTrans_sincurva =
                  AyudaObtenerPtosTransformada.ObtenerPtosSINTransformados(sololist_sincurva, rebarElevDTO._View);

                XYZ unitariZ = new XYZ(0, 0, -1);
                //1) inf
                var Lisinferior = ListaptosTrans.Where(c => c.PtoInicialTransformada.Z < 0 && c.PtoFinalTransformada.Z < 0).OrderByDescending(c => c.largoCurve).ToList();

                if (Lisinferior.Count == 0)
                    return false;

                XYZ ptomedioMAsBajo = Lisinferior.MinBy(c => c.PtoMedio.Z).PtoMedio;

                UbicacionDeF = ptomedioMAsBajo + Util.CmToFoot(7) * unitariZ;// rebarElevDTO._View.RightDirection*Util.CmToFoot(10); // cambiar '_View.ViewDirection' si se selecciona a la izquierda o derecha
                UbicacionDeL = ptomedioMAsBajo + Util.CmToFoot(15) * unitariZ;// rebarElevDTO._View.RightDirection * Util.CmToFoot(15);


                //2)sup
                var superior = ListaptosTrans.Where(c => c.PtoInicialTransformada.Z > 0 && c.PtoFinalTransformada.Z > 0).OrderByDescending(c => c.largoCurve).ToList();

                if (superior.Count == 0)
                    return false;
                XYZ ptoSupMAsAlto = superior.MinBy(c => -c.PtoMedio.Z).PtoMedio;

                ////2.1
                //var superior_sincurva = ListaptosTrans_sincurva.Where(c => c.PtoInicialTransformada.Z > 0 && c.PtoFinalTransformada.Z > 0).OrderByDescending(c => c.largoCurve).ToList();
                //if (superior_sincurva.Count == 0)
                //    return false;


                UbicacionSup = ptoSupMAsAlto - unitariZ * Util.CmToFoot(5);
                UbicacionSup_ValorLArgo = Math.Round(Util.FootToCm(superior[0].ParametrosRebar.largo), 0).ToString(); //largo no real

                //3 izq
                var izq = ListaptosTrans.Where(c => c.PtoInicialTransformada.X < 0 && c.PtoFinalTransformada.X < 0).OrderByDescending(c => c.largoCurve).ToList();

                if (izq.Count == 0)
                    return false;
                UbicacionIZq = izq[0].PtoMedio + rebarElevDTO._View.RightDirection * Util.CmToFoot(5);

                //var izq_sincurva = ListaptosTrans_sincurva.Where(c => c.PtoInicialTransformada.X < 0 && c.PtoFinalTransformada.X < 0).OrderByDescending(c => c.largoCurve).ToList();
                //if (superior_sincurva.Count == 0)
                //    return false;
                //3.1
                //UbicacionIZq_ValorLArgo = Math.Round(Util.FootToCm(izq[0].largoCurve+ Delta), 0).ToString(); //largo no real
                UbicacionIZq_ValorLArgo = Math.Round(Util.FootToCm(izq[0].ParametrosRebar.largo), 0).ToString(); //largo no real
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
