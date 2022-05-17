using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Desglose.Ayuda;
using Desglose.EditarRebar;
using Desglose.Entidades;
using Desglose.Model;
using System;
using System.Collections.Generic;

namespace Desglose.UpDate.Casos
{
    internal class UpdateRebarElevaciones
    {
        private Document _doc;
        private Rebar _rebar;
        private TipoRebar tipoBarra_;
        private CreadorListaWraperRebarLargo _CreadorListaWraperRebarLargo;

        public UpdateRebarElevaciones(Document doc, Rebar rebar, TipoRebar tipoBarra_)
        {
            _doc = doc;
            _rebar = rebar;
            this.tipoBarra_ = tipoBarra_;
        }

        internal bool Ejecutar()
        {
            try
            {
                if (_rebar == null) return false;
                if (!_rebar.IsValidObject) return false;
                _CreadorListaWraperRebarLargo = new CreadorListaWraperRebarLargo(_rebar, XYZ.Zero);
                if (!_CreadorListaWraperRebarLargo.Ejecutar()) return false;
                
                M1_ActializarLargoParcialYTotal();
                //  RebarShapeDrivenAccessor rebarShapeDrivenAccessor = _rebar.GetShapeDrivenAccessor();
                //   if (rebarShapeDrivenAccessor == null) return;

                // RebarDesglose _RebarDesglose = new RebarDesglose(uiapp, _rebar);

                //_RebarDesglose.Ejecutar();
            }
            catch (Exception)
            {

                return false;
            }
            return true; ;
        }

        private bool M1_ActializarLargoParcialYTotal()
        {
            try
            {
                List<WraperRebarLargo> ListaCurvaBarras = _CreadorListaWraperRebarLargo.ListaCurvaBarras;

                if (ListaCurvaBarras == null) return false; ;

                string largoparciales = "";
                double largoTotal = 0;

                // double diamCm2 = Convert.ToInt32(_rebar.LookupParameter("Bar Diameter").AsValueString().Replace("mm", "")) / 10.0f;
                // double diamCm = Util.FootToCm(_rebar.LookupParameter("Bar Diameter").AsDouble());

                if (ListaCurvaBarras.Count > 1)
                {
                    foreach (WraperRebarLargo _wraperRebarLargo in ListaCurvaBarras)
                    {
                        if (largoparciales == "")
                        {
                            /* largoparciales = largoparciales + "" + Math.Round(Util.FootToCm(_wraperRebarLargo._curve.Length)
                                                                  + (_wraperRebarLargo.FijacionInicial == FijacionRebar.fijo ? diamCm / 2.0f : 0)
                                                                  + (_wraperRebarLargo.FijacionFinal == FijacionRebar.fijo ? diamCm / 2.0f : 0), 0);*/
                            largoparciales = largoparciales + "" + Math.Round(Util.FootToCm(_wraperRebarLargo._curve.Length));
                        }
                        else
                        {
                            /*largoparciales = largoparciales + "+" + Math.Round(Util.FootToCm(_wraperRebarLargo._curve.Length)
                                                                 + (_wraperRebarLargo.FijacionInicial == FijacionRebar.fijo ? diamCm / 2.0f : 0)
                                                                 + (_wraperRebarLargo.FijacionFinal == FijacionRebar.fijo ? diamCm / 2.0f : 0), 0);*/
                            largoparciales = largoparciales + "+" + Math.Round(Util.FootToCm(_wraperRebarLargo._curve.Length));
                        }

                        /* largoTotal += +Math.Round(Util.FootToCm(_wraperRebarLargo._curve.Length)
                                         + (_wraperRebarLargo.FijacionInicial == FijacionRebar.fijo ? diamCm / 2.0f : 0)
                                      + (_wraperRebarLargo.FijacionFinal == FijacionRebar.fijo ? diamCm / 2.0f : 0), 0);*/
                        largoTotal += +Math.Round(Util.FootToCm(_wraperRebarLargo._curve.Length));
                    }

                    if (largoparciales != "") ParameterUtil.SetParaInt(_rebar, "LargoParciales", $"({largoparciales})");//(30+100+30)
                }
                else if (ListaCurvaBarras.Count == 1)
                {
                    largoTotal += Util.FootToCm(ListaCurvaBarras[0]._curve.Length);
                }
                int valorReglaLAyout=_rebar.get_Parameter(BuiltInParameter.REBAR_ELEM_LAYOUT_RULE).AsInteger();
                if(valorReglaLAyout!=0)
                    ParameterUtil.SetParaInt(_rebar, "CantidadBarra", _rebar.Quantity.ToString());//(30+100+30)

                ParameterUtil.SetParaInt(_rebar, "LargoTotal", $"{ Math.Round(largoTotal, 0)}");//(30+100+30)
                //ParameterUtil.SetParaInt(_rebar, "BBOpt", "532");
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error UpdateRebar -> 'M1_ActializarLargoParcialYTotal'  ex:{ex.Message}");
                return false;
            }
            return true;
        }
      
    }
}