using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Desglose.Ayuda;
using Desglose.Dibujar2D;
using Desglose.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Calculos
{
    class PtoResult
    {
        public XYZ resultInserccion { get; set; }
        public bool Isok { get; set; }
        public PtoResult()
        {
            resultInserccion = XYZ.Zero;
            Isok = false;
        }
    }
    class PtoInferiorLAtDTO
    {
        public XYZ ptomedioENview { get; set; }
        public double distanciREspectoOtro { get; set; }
        public RebarDesglose_Barras_H2 BArrasEnElev_laterales { get; internal set; }
    }

    class CalculoPtoTagBArraHorizontal_Corte
    {
        private static List<PtoInferiorLAtDTO> ListaPtoDTO;

        internal static List<ElementId> lista2BarrasMAsInferior(List<RebarDesglose_Barras_H2> listaBArrasEnElev_laterales, ViewSection section, CrearTrasformadaSobreVectorDesg trasform_, int dire)
        {
            List<ElementId> _lista = new List<ElementId>();
            try
            {
                if (listaBArrasEnElev_laterales.Count == 0) return _lista;
                ObtenerLista(listaBArrasEnElev_laterales, section, trasform_, dire);

                ListaPtoDTO = ListaPtoDTO.OrderBy(c => c.ptomedioENview.Z).ToList();
                _lista.Add(ListaPtoDTO[0].BArrasEnElev_laterales.RebarDesglose_Barras_H_._rebarDesglose._rebar.Id);
                _lista.Add(ListaPtoDTO[1].BArrasEnElev_laterales.RebarDesglose_Barras_H_._rebarDesglose._rebar.Id);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener punto lateral  ex:{ex.Message}");
                _lista = new List<ElementId>();
            }

            return _lista;
        }


        internal static PtoResult PtoInferior(List<RebarDesglose_Barras_H2> listaBArrasEnElev_laterales, ViewSection section, CrearTrasformadaSobreVectorDesg trasform_, int dire)
        {
            PtoResult _PtoResult = new PtoResult();
            try
            {
                if (listaBArrasEnElev_laterales.Count == 0) return _PtoResult;
                ObtenerLista(listaBArrasEnElev_laterales, section, trasform_, dire);

                ListaPtoDTO = ListaPtoDTO.OrderByDescending(c => c.distanciREspectoOtro).ToList();
                double zmin = ListaPtoDTO.Min(c => c.ptomedioENview.Z);
                _PtoResult.Isok = true;
                _PtoResult.resultInserccion = ListaPtoDTO[0].ptomedioENview.AsignarZ(zmin);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener punto lateral  ex:{ex.Message}");
                _PtoResult = new PtoResult();
            }

            return _PtoResult;
        }


        internal static PtoResult PtoSuperior(List<RebarDesglose_Barras_H2> listaBArrasEnElev_laterales, ViewSection section, CrearTrasformadaSobreVectorDesg trasform_, int dire)
        {
            PtoResult _PtoResult = new PtoResult();
            try
            {
                if (listaBArrasEnElev_laterales.Count == 0) return _PtoResult;
                ObtenerLista(listaBArrasEnElev_laterales, section, trasform_, dire);

                ListaPtoDTO = ListaPtoDTO.OrderByDescending(c => c.distanciREspectoOtro).ToList();
                double zMAX = ListaPtoDTO.Max(c => c.ptomedioENview.Z);
                _PtoResult.Isok = true;
                _PtoResult.resultInserccion = ListaPtoDTO[0].ptomedioENview.AsignarZ(zMAX);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener punto lateral  ex:{ex.Message}");
                _PtoResult = new PtoResult();
            }

            return _PtoResult;
        }


        private static void ObtenerLista(List<RebarDesglose_Barras_H2> listaBArrasEnElev_laterales, ViewSection section, CrearTrasformadaSobreVectorDesg trasform_, int dire)
        {
            ListaPtoDTO = new List<PtoInferiorLAtDTO>();
            XYZ ptoMedioInicialENview = trasform_.EjecutarTransformInvertida(listaBArrasEnElev_laterales[0].RebarDesglose_Barras_H_.ptoMedio);


            Rebar _rebarInic = listaBArrasEnElev_laterales[0].RebarDesglose_Barras_H_._rebarDesglose._rebar;
            ptoMedioInicialENview = ptoMedioInicialENview+ ObtenerPosicionUltimaBarraSet(section, _rebarInic);
            ptoMedioInicialENview = section.NH_ObtenerPtoSObreVIew(ptoMedioInicialENview);

            foreach (var item in listaBArrasEnElev_laterales)
            {
                XYZ ptoMedioAux = trasform_.EjecutarTransformInvertida(item.RebarDesglose_Barras_H_.ptoMedio);

                Rebar auxRebarAnaliz = item.RebarDesglose_Barras_H_._rebarDesglose._rebar;
                XYZ ptomedioENview_Aux = ptoMedioAux+ ObtenerPosicionUltimaBarraSet(section, auxRebarAnaliz);

                 ptomedioENview_Aux = section.NH_ObtenerPtoSObreVIew(ptomedioENview_Aux);

                ListaPtoDTO.Add(new PtoInferiorLAtDTO()
                {
                    ptomedioENview = ptomedioENview_Aux,
                    distanciREspectoOtro = ptoMedioInicialENview.AsignarZ(0).DistanceTo(ptomedioENview_Aux.AsignarZ(0)) * ObtenerSigno(ptoMedioInicialENview, ptomedioENview_Aux, section.RightDirection),
                    BArrasEnElev_laterales = item
                });
            }
        }

        private static XYZ ObtenerPosicionUltimaBarraSet(ViewSection section, Rebar _rebarInic)
        {
            XYZ ptoDesplazado = XYZ.Zero;

            try
            { 
                if (_rebarInic.Quantity > 1)
                {
                    int cantidad = _rebarInic.Quantity;
                    double espa = Util.CmToFoot(_rebarInic.ObtenerEspaciento_cm());

                    if (espa == 0)
                        espa = _rebarInic.ObtenerEspaciento_cm();

                    var _ShapeDrivenAccessor = _rebarInic.GetShapeDrivenAccessor();
                    XYZ direcio_ = _ShapeDrivenAccessor.Normal;

                    if (Util.GetProductoEscalar(direcio_, section.RightDirection) > 0)
                    {
                        ptoDesplazado = direcio_ * espa * cantidad;
                    }
                }

                return ptoDesplazado;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'ObtenerPosicionUltimaBarraSet()'  \n ex:{ex.Message}");

                return XYZ.Zero;
            }
        }

        //si el corte esta hacia un lado puede u otro puede ser que 'rightDirection' de la view se  en la direccion contraria ala tradicional
        // obteene un signo para que los largos siempre sen medido de izq a derecha
        private static double ObtenerSigno(XYZ ptoMedioInicialENview, XYZ ptomedioENview_Aux, XYZ rightDirection)
        {
            int resutl = 1;
            try
            {
                XYZ direccionLargo = (ptomedioENview_Aux.AsignarZ(0) - ptoMedioInicialENview.AsignarZ(0));
                double valor = Util.GetProductoEscalar(direccionLargo, rightDirection);
                if (valor > 0)
                    return 1;
                else
                    return -1;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'ObtenerSigno()'  \n ex:{ex.Message}");
                return 1;
            }

        }
    }
}
