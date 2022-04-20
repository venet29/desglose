using Desglose.Ayuda;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Desglose.Entidades;
using ADesglose.Ayuda;
using Desglose.Extension;
using Desglose.DTO;

namespace Desglose.Model
{
    public class RebarDesglose
    {
        static int nextId = 0;

        public int contRebar { get; set; }
        public UIApplication _uiapp { get; }

        private Document _doc;
        private View _view;

        public Rebar _rebar { get; set; }
        public double LargoTotalSumaParcialesFoot { get; set; }
        public TipoRebar _tipoBarraEspecifico { get; set; }
        public List<parametrosRebar> ListaParametrosRebar { get; set; }
        public List<WraperRebarLargo> ListaCurvaBarras { get; set; }
        public List<WraperRebarLargo> ListaCurvaBarrasFinal_Estribo { get; set; }
        public List<WraperRebarLargo> ListaCurvaBarrasFinal_conCurva_Estribo { get; set; }
        public TipobarraH TipobarraH_ { get; set; }
        public XYZ _normal { get; set; }
        public int Diametro_MM { get; private set; }
        public List<Curve> listaptoInicialConCurva { get; private set; }
        public WraperRebarLargo CurvaMasLargo_WraperRebarLargo { get; private set; }

        public WraperRebarLargo CurvaMasLargo_WraperEstribo { get; private set; }
        public OrientacionBArra OrientacionBArra_ { get; set; }
        public RebarHookType HookInicial { get; private set; }
        public RebarHookType HookFinal { get; private set; }
        public CrearTrasformadaSobreVectorDesg trasform { get;  set; }
        public RebarDesglose RebarDesgloseSinTraslapo { get; set; }
        internal TraslapoBarrasH TraslapoCOnbarras { get; set; }


        //******* SOLO AUXILIARES PARA TRASFORMADA
        List<WraperRebarLargo> AuxTRans_ListaCurvaBarras;
        List<WraperRebarLargo> AuxTRans_ListaCurvaBarrasFinal_Estribo;
        List<WraperRebarLargo> AuxTRans_ListaCurvaBarrasFinal_conCurva_Estribo;
        public RebarDesglose(UIApplication uiapp, Rebar _rebar)
        {
            nextId += 1;
            this.contRebar = nextId;
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _uiapp.ActiveUIDocument.ActiveView;
            this._rebar = _rebar;
            this.ListaParametrosRebar = new List<parametrosRebar>();
            this.ListaCurvaBarras = new List<WraperRebarLargo>();
            this.ListaCurvaBarrasFinal_Estribo = new List<WraperRebarLargo>();
            this.ListaCurvaBarrasFinal_conCurva_Estribo = new List<WraperRebarLargo>();
            this._tipoBarraEspecifico = TipoRebar.NONE;
            this.CurvaMasLargo_WraperEstribo = new WraperRebarLargo();
        }

        public RebarDesglose CrearCopiarTrans(CrearTrasformadaSobreVectorDesg trasform)
        {
            AuxTRans_ListaCurvaBarras = new List<WraperRebarLargo>();
            AuxTRans_ListaCurvaBarrasFinal_Estribo = new List<WraperRebarLargo>();
            AuxTRans_ListaCurvaBarrasFinal_conCurva_Estribo = new List<WraperRebarLargo>();

            //a)
            for (int i = 0; i < ListaCurvaBarras.Count; i++)
            {
                var trans = ListaCurvaBarras[i].GenerarTrasformada(trasform);
                if(trans!=null)
                    AuxTRans_ListaCurvaBarras.Add(trans);
            }

            //b
            for (int i = 0; i < ListaCurvaBarrasFinal_Estribo.Count; i++)
            {
                var trans = ListaCurvaBarrasFinal_Estribo[i].GenerarTrasformada(trasform);
                if (trans != null)
                    AuxTRans_ListaCurvaBarrasFinal_Estribo.Add(trans);
            }

            //c
            for (int i = 0; i < ListaCurvaBarrasFinal_conCurva_Estribo.Count; i++)
            {
                var trans = ListaCurvaBarrasFinal_conCurva_Estribo[i].GenerarTrasformada(trasform);
                if (trans != null)
                    AuxTRans_ListaCurvaBarrasFinal_conCurva_Estribo.Add(trans);
            }



            return new RebarDesglose(_uiapp, _rebar)
            {
                contRebar = contRebar,
                LargoTotalSumaParcialesFoot = LargoTotalSumaParcialesFoot,
                _tipoBarraEspecifico = _tipoBarraEspecifico,

                ListaParametrosRebar = ListaParametrosRebar,
                ListaCurvaBarras = AuxTRans_ListaCurvaBarras,
                ListaCurvaBarrasFinal_Estribo = AuxTRans_ListaCurvaBarrasFinal_Estribo,
                ListaCurvaBarrasFinal_conCurva_Estribo = AuxTRans_ListaCurvaBarrasFinal_conCurva_Estribo,
                Diametro_MM =Diametro_MM,
                _normal = _normal,
                TipobarraH_= TipobarraH_,
                listaptoInicialConCurva = listaptoInicialConCurva,
                CurvaMasLargo_WraperRebarLargo = CurvaMasLargo_WraperRebarLargo.GenerarTrasformada(trasform),
                OrientacionBArra_ = OrientacionBArra_,
                HookInicial = HookInicial,
                HookFinal = HookFinal,
                trasform= trasform,
                RebarDesgloseSinTraslapo=this
            };

        }

        public RebarDesglose CrearCopiarTrans_Estribo(CrearTrasformadaSobreVectorDesg trasform)
        {
            AuxTRans_ListaCurvaBarras = new List<WraperRebarLargo>();
            AuxTRans_ListaCurvaBarrasFinal_Estribo = new List<WraperRebarLargo>();
            AuxTRans_ListaCurvaBarrasFinal_conCurva_Estribo = new List<WraperRebarLargo>();

            //a)
           
            for (int i = 0; i < ListaCurvaBarras.Count; i++)
            {
                var trans = ListaCurvaBarras[i].GenerarTrasformada(trasform);
                if (trans != null)
                    AuxTRans_ListaCurvaBarras.Add(trans);
            }

            //b
            for (int i = 0; i < ListaCurvaBarrasFinal_Estribo.Count; i++)
            {
                var trans = ListaCurvaBarrasFinal_Estribo[i].GenerarTrasformada(trasform);
                if (trans != null)
                    AuxTRans_ListaCurvaBarrasFinal_Estribo.Add(trans);
            }

            //c
            for (int i = 0; i < ListaCurvaBarrasFinal_conCurva_Estribo.Count; i++)
            {
                var trans = ListaCurvaBarrasFinal_conCurva_Estribo[i].GenerarTrasformada(trasform);
                if (trans != null)
                    AuxTRans_ListaCurvaBarrasFinal_conCurva_Estribo.Add(trans);
            }

            CurvaMasLargo_WraperEstribo.PtoInicialTransformada = trasform.EjecutarTransform( CurvaMasLargo_WraperRebarLargo.ptoInicial);
            CurvaMasLargo_WraperEstribo.PtoFinalTransformada = trasform.EjecutarTransform( CurvaMasLargo_WraperRebarLargo.ptoFinal + _normal * _rebar.Quantity * _rebar.MaxSpacing);

            return new RebarDesglose(_uiapp, _rebar)
            {
                contRebar = contRebar,
                LargoTotalSumaParcialesFoot = LargoTotalSumaParcialesFoot,
                _tipoBarraEspecifico = _tipoBarraEspecifico,

                ListaParametrosRebar = ListaParametrosRebar,
                ListaCurvaBarras = AuxTRans_ListaCurvaBarras,
                ListaCurvaBarrasFinal_Estribo = AuxTRans_ListaCurvaBarrasFinal_Estribo,
                ListaCurvaBarrasFinal_conCurva_Estribo = AuxTRans_ListaCurvaBarrasFinal_conCurva_Estribo,
                Diametro_MM = Diametro_MM,
                _normal = _normal,
                listaptoInicialConCurva = listaptoInicialConCurva,
                CurvaMasLargo_WraperEstribo = CurvaMasLargo_WraperEstribo,
                CurvaMasLargo_WraperRebarLargo = CurvaMasLargo_WraperRebarLargo.GenerarTrasformada(trasform),
                OrientacionBArra_ = OrientacionBArra_,
                HookInicial = HookInicial,
                HookFinal = HookFinal,
                trasform = trasform,
                RebarDesgloseSinTraslapo = this
            };

        }

        public bool Ejecutar()
        {
            try
            {
                if (!A_ObtenerListaParametros()) return false;

                if (!B_ObtenerListaCurvas_InicialSetBArras()) return false;

                if (!C_ObtenerCUrvaPrinciapl()) return false;

                if (!D_ObtenerTipobarra()) return false;

                if (_tipoBarraEspecifico != TipoRebar.ELEV_BA_V && _tipoBarraEspecifico != TipoRebar.ELEV_BA_H)
                {
                    if (!E_ObtenerListaCurvas_Estribos()) return false;
                }
            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error -> Obtener desglose de barras   ex:{ ex.Message} ");
                return false;
            }
            return true;
        }

        private bool C_ObtenerCUrvaPrinciapl()
        {
            try
            {
                if (ListaCurvaBarras[0].IsBarraPrincipal)
                {
                    CurvaMasLargo_WraperRebarLargo = ListaCurvaBarras[0];
                }
                else if (ListaCurvaBarras.Last().IsBarraPrincipal)
                {
                    CurvaMasLargo_WraperRebarLargo = ListaCurvaBarras.Last();
                }
                else
                {
                    CurvaMasLargo_WraperRebarLargo = ListaCurvaBarras.OrderByDescending(c => c._curve.Length).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error UpdateRebar -> 'CreadorListaWraperRebarLargo'   ex:{ ex.Message} ");
                return false;
            }
            return true;
        }

        public bool A_ObtenerListaParametros()
        {
            try
            {
                string[] listaLetra = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "K" };
                ParameterSet pars = _rebar.Parameters;
                foreach (var letra in listaLetra)
                {
                    var parabet = ParameterUtil.FindParaByName(pars, letra);
                    if (parabet == null) continue;
                    double largo = parabet.AsDouble();
                    if (largo != 0)
                    {
                        parametrosRebar _newparametrosRebar = (_tipoBarraEspecifico == TipoRebar.NONE
                                                                ? new parametrosRebar(letra, largo)
                                                                : new parametrosRebar(letra, largo, _tipoBarraEspecifico));
                        if (_newparametrosRebar.ObtenerLetraNH())
                            ListaParametrosRebar.Add(_newparametrosRebar);
                    };
                }
            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener lista Parametros a): {ex.Message}");
                return false;
            }

            return true;
        }

        public bool B_ObtenerListaCurvas_InicialSetBArras()
        {
            try
            {
                var _driverAccesor = _rebar.GetShapeDrivenAccessor();
                _normal = _driverAccesor.Normal;

                if (!AyudaCurveRebar.GetPrimeraRebarCurves(_rebar)) return false;

                listaptoInicialConCurva = AyudaCurveRebar.ListacurvesSoloLineas[0];

                int cont = 0;
                foreach (Curve item in listaptoInicialConCurva)
                {
                    // bool IsBarraPrincipal = curvaMAyorlargo.IsEqual((Line)item);
                    WraperRebarLargo newCurbaBarras = new WraperRebarLargo(item, ObtenerPAtametro(cont), false); ;
                    newCurbaBarras.DatosIniciales();
                    ListaCurvaBarras.Add(newCurbaBarras);
                    cont += 1;
                }

                if (ListaCurvaBarras.Count > 2)
                {
                    for (int i = 1; i < ListaCurvaBarras.Count - 1; i++)
                    {
                        ListaCurvaBarras[i].IsBarraPrincipal = true;
                    }
                    ListaCurvaBarras[0].FijacionInicial = FijacionRebar.movil;
                    ListaCurvaBarras.Last().FijacionFinal = FijacionRebar.movil;
                }
                else if (ListaCurvaBarras.Count == 2)
                {
                    if (ListaCurvaBarras[1].ParametrosRebar.largo > ListaCurvaBarras[0].ParametrosRebar.largo)
                        ListaCurvaBarras[1].IsBarraPrincipal = true;
                    else
                        ListaCurvaBarras[0].IsBarraPrincipal = true;

                    ListaCurvaBarras[0].FijacionInicial = FijacionRebar.movil;
                    ListaCurvaBarras.Last().FijacionFinal = FijacionRebar.movil;
                }
                else if (ListaCurvaBarras.Count == 1)
                {
                    ListaCurvaBarras[0].IsBarraPrincipal = true;
                    ListaCurvaBarras[0].FijacionInicial = FijacionRebar.movil;
                    ListaCurvaBarras[0].FijacionFinal = FijacionRebar.movil;
                }
                else
                    return false;


                HookInicial = _doc.GetElement(_rebar.GetHookTypeId(0)) as RebarHookType;
                HookFinal = _doc.GetElement(_rebar.GetHookTypeId(1)) as RebarHookType;
            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener lista curca b): {ex.Message}");
                return false;
            }
            return true;
        }
        private bool D_ObtenerTipobarra()
        {
            try
            {
                var _driverAccesor = _rebar.GetShapeDrivenAccessor();
                _normal = _driverAccesor.Normal;
                Diametro_MM= _rebar.ObtenerDiametroInt();

                if (ListaParametrosRebar.Count == 6) //estribo
                {
                    if (UtilDesglose.IsSimilarValor(_normal.Z, 0, 0.15708)) //+-10°
                    {
                        _tipoBarraEspecifico = TipoRebar.ELEV_ES_V; // estribo viga horizontal    
                        OrientacionBArra_ = OrientacionBArra.Horizontal;
                    }
                    else
                    {
                        _tipoBarraEspecifico = TipoRebar.ELEV_ES;  // estribo pilar vertical -> 
                        OrientacionBArra_ = OrientacionBArra.Vertical;
                    }

                }

                else if (ListaParametrosRebar.Count == 3 &&
                    UtilDesglose.IsSimilarValor(Math.Abs(UtilDesglose.GetProductoEscalar(((Line)CurvaMasLargo_WraperRebarLargo._curve).Direction, _view.ViewDirection)), 1, 0.015)) //traba
                {
                    if (UtilDesglose.IsSimilarValor(_normal.Z, 0, 0.15708)) //+-10°
                    {
                        _tipoBarraEspecifico = TipoRebar.ELEV_ES_VT; // estribo viga horizontal  
                        OrientacionBArra_ = OrientacionBArra.Horizontal;
                    }
                    else
                    {
                        _tipoBarraEspecifico = TipoRebar.ELEV_ES_T;  // estribo pilar vertical -> 
                        OrientacionBArra_ = OrientacionBArra.Vertical;
                    }
                }

                else if (ListaParametrosRebar.Count == 3 && IsEstriboPorHook()) //traba
                {
                    if (UtilDesglose.IsSimilarValor(_normal.Z, 0, 0.15708)) //+-10°
                    {
                        _tipoBarraEspecifico = TipoRebar.ELEV_ES_VT; // estribo viga horizontal  
                        OrientacionBArra_ = OrientacionBArra.Horizontal;
                    }
                    else
                    {
                        _tipoBarraEspecifico = TipoRebar.ELEV_ES_T;  // estribo pilar vertical -> 
                        OrientacionBArra_ = OrientacionBArra.Vertical;
                    }
                }

                else //barra que pueden ser vertical u horizontal
                {



                    if (UtilDesglose.IsmasHorizontal(((Line)CurvaMasLargo_WraperRebarLargo._curve).Direction)) //+-10°
                    {
                        _tipoBarraEspecifico = TipoRebar.ELEV_BA_H;// estribo viga horizontal    
                        OrientacionBArra_ = OrientacionBArra.Horizontal;
                    }
                    else
                    {
                        _tipoBarraEspecifico = TipoRebar.ELEV_BA_V;  // estribo pilar vertical -> 
                        OrientacionBArra_ = OrientacionBArra.Vertical;
                    }
                    _normal = CurvaMasLargo_WraperRebarLargo.direccion;
                }
            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener tipo barras c): {ex.Message}");
                return false;
            }
            return true;
        }
        private bool IsEstriboPorHook()
        {
            if (HookInicial == null) return false;
            if (HookFinal == null) return false;

            if (!HookInicial.Name.Contains("135")) return false;
            if (!HookFinal.Name.Contains("135")) return false;
            return true;
        }
        private bool E_ObtenerListaCurvas_Estribos()
        {
            try
            {
              //  CurvaMasLargo_WraperEstribo. 
                if (!B_ObtenerListaCurvas_InicialSetBArras()) return false;

                CurvaMasLargo_WraperEstribo.ptoInicial = CurvaMasLargo_WraperRebarLargo.ptoInicial;
                CurvaMasLargo_WraperEstribo.ptoFinal = CurvaMasLargo_WraperRebarLargo.ptoFinal + _normal * (_rebar.Quantity-1) * _rebar.MaxSpacing;

                if (!AyudaCurveRebar.GetUtimaRebarCurves(_rebar)) return false;

                listaptoInicialConCurva = AyudaCurveRebar.ListacurvesSoloLineas[0];
                //    List<Curve> listaCurvaInicialRef = AyudaCurveRebar.ListacurvesSoloLineas[0];
                int cont = 0;
                foreach (Curve item in listaptoInicialConCurva)
                {

                    // bool IsBarraPrincipal = curvaMAyorlargo.IsEqual((Line)item);
                    WraperRebarLargo newCurbaBarras = new WraperRebarLargo(item, ObtenerPAtametro(cont), false); ;
                    newCurbaBarras.DatosIniciales();
                    ListaCurvaBarrasFinal_Estribo.Add(newCurbaBarras);

                    cont += 1;
                }

                LargoTotalSumaParcialesFoot = ListaCurvaBarrasFinal_Estribo.Sum(c => c._curve.Length);

                //con curva
                List<Curve> listaptoFinalconcurva = AyudaCurveRebar.ListacurvesOriginal[0];

                cont = 0;

                Debug.WriteLine("--------- nueva barra");
                foreach (Curve item in listaptoFinalconcurva)
                {
                    // bool IsBarraPrincipal = curvaMAyorlargo.IsEqual((Line)item);
                    WraperRebarLargo newCurbaBarras = new WraperRebarLargo(item, ObtenerPAtametro(cont), false); ;
                    newCurbaBarras.DatosIniciales();
                    ListaCurvaBarrasFinal_conCurva_Estribo.Add(newCurbaBarras);
                    if (item is Line) cont += 1;

                }

            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener lista curca b): {ex.Message}");
                return false;
            }
            return true;
        }

        private parametrosRebar ObtenerPAtametro(int cont)
        {
            parametrosRebar _parametrosRebar = null;
            try
            {
                if (ListaParametrosRebar.Count - 1 >= cont)
                    _parametrosRebar = ListaParametrosRebar[cont];
                else
                    _parametrosRebar = new parametrosRebar("Sin", 0);
            }
            catch (Exception ex)
            {
                _parametrosRebar = new parametrosRebar("Sin", 0);
            }
            return _parametrosRebar;
        }

        //publicas

        public TipoRebarElev C_obtenerTipobarra_H2()
        {
            if (ListaCurvaBarras.Count == 3)
                return TipoRebarElev.AmbasPataH;
            else if (ListaCurvaBarras.Count == 1)
                return TipoRebarElev.SinpataH;
            else if (ListaCurvaBarras.Count == 2)
            {
                if (ListaCurvaBarras[0].IsBarraPrincipal)
                {
                    if (Util.IsSimilarValor(ListaCurvaBarras[0].ptoFinal.Z, ListaCurvaBarras[1].ptoFinal.Z, 0.1))
                        return TipoRebarElev.PataSuperiorH;
                    else
                        return TipoRebarElev.PataInferiorH;
                }
                else
                {
                    if (Util.IsSimilarValor(ListaCurvaBarras[1].ptoFinal.Z, ListaCurvaBarras[0].ptoFinal.Z, 0.1))
                        return TipoRebarElev.PataSuperiorH;
                    else
                        return TipoRebarElev.PataInferiorH;
                }
            }
            else
            {
                UtilDesglose.ErrorMsg($"Barras con {ListaCurvaBarras.Count}  segmentos. Rutina solo implementada par 1,2 y 3 segmentos. Se considera barra de 1 segmentos");
                return TipoRebarElev.Sinpata;
            }
        }
        public TipoRebarElev C_obtenerTipobarra()
        {
            if (ListaCurvaBarras.Count == 3)
                return TipoRebarElev.AmbasPata;
            else if (ListaCurvaBarras.Count == 1)
                return TipoRebarElev.Sinpata;
            else if (ListaCurvaBarras.Count == 2)
            {
                if (ListaCurvaBarras[0].IsBarraPrincipal)
                {
                    if (Util.IsSimilarValor(ListaCurvaBarras[0].ptoFinal.Z, ListaCurvaBarras[1].ptoFinal.Z, 0.1))
                        return TipoRebarElev.PataSuperior;
                    else
                        return TipoRebarElev.PataInferior;
                }
                else
                {
                    if (Util.IsSimilarValor(ListaCurvaBarras[1].ptoFinal.Z, ListaCurvaBarras[0].ptoFinal.Z, 0.1))
                        return TipoRebarElev.PataSuperior;
                    else
                        return TipoRebarElev.PataInferior;
                }
            }
            else
            {
                UtilDesglose.ErrorMsg($"Barras con {ListaCurvaBarras.Count}  segmentos. Rutina solo implementada par 1,2 y 3 segmentos. Se considera barra de 1 segmentos");
                return TipoRebarElev.Sinpata;
            }
        }

        public TipoRebarElev C_obtenerTipoEstribo_H()
        {
            if (ListaCurvaBarrasFinal_Estribo.Count == 3)
                return TipoRebarElev.EstriboVigaTraba;
            else
                return TipoRebarElev.EstriboViga;
        }

        public TipoRebarElev C_obtenerTipoEstribo_V()
        {
            if (ListaCurvaBarrasFinal_Estribo.Count == 3)
                return TipoRebarElev.EstriboTraba;
            else
                return TipoRebarElev.Estribo;
        }
    }
}
