using Desglose.Ayuda;
using Desglose.Entidades;
using Desglose.UTILES;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Model
{
    public class DatosTransformada
    {
        public XYZ ptoInicial { get; set; }
        public XYZ ptoMedio { get; set; }
        public XYZ ptoFinal { get; set; }
        public Line curvePrincipal { get; set; }
        public bool Isok { get; set; }
        public object listPtosCurvaAuxDTO { get; private set; }

        public DatosTransformada(XYZ _ptoInicial, XYZ _ptoFinal)
        {
            ptoInicial = _ptoInicial;
            ptoFinal = _ptoFinal;
    
        


        }

        public bool Ejecutar()
        {
            try
            {
                /**
                CrearTrasformadaSobreVector _Trasform = new CrearTrasformadaSobreVector(centroDeestribo, -90, _view.RightDirection);

                for (int i = 0; i < listaCuvas.Count; i++)
                {
                    WraperRebarLargo item = listaCuvas[i];

                    PtosCurvaAuxDTO _NewPtosCurvaAuxDTO = new PtosCurvaAuxDTO()
                    {
                        PtoInicial = item.PtoInicialTransformada.AsignarZ(zincial),
                        PtoMedio = item.PtoMedioTransformada.AsignarZ(zincial),
                        PtoFinal = item.PtoFinalTransformada.AsignarZ(zincial),
                        PtoInicialTransformada = _Trasform.EjecutarTransform(item.PtoInicialTransformada.AsignarZ(zincial)),
                        PtoMedioTransformada = _Trasform.EjecutarTransform(item.PtoMedioTransformada.AsignarZ(zincial)),
                        PtoFinalTransformada = _Trasform.EjecutarTransform(item.PtoFinalTransformada.AsignarZ(zincial)),
                        largoCurve = item._curve.Length
                    };
                    listPtosCurvaAuxDTO.Add(_NewPtosCurvaAuxDTO);
                }

                if (ptoInicial.DistanceTo(ptoFinal) > UtilDesglose.CmToFoot(5))
                {
                    curvePrincipal = Line.CreateBound(ptoInicial, ptoFinal);
                    Isok = true;
                }
                else
                {
                    Isok = false;
                }
                */
            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener  datos trasladados");
                return false;
            }
            return true;
        }
    }
}
