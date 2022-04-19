﻿using Desglose.Ayuda;
using Desglose.Model;
using Desglose.Entidades;
using Desglose.Extension;
using Desglose.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Linq;
using System.Collections.Generic;
using Desglose.Ayuda;

namespace Desglose.Calculos
{
    internal class GeneradorListaTrasfomardas
    {
        private UIApplication _uiapp;
        private View _view;
        private List<RebarDesglose> lista_RebarDesglose;
 
        public List<RebarDesglose> listaTransformada_RebarDesglose { get; set; }
        public List<RebarDesglose> listaTransformada_RebarDesgloseEstribo { get; private set; }

        private DatosHost _DatosHost;

        public GeneradorListaTrasfomardas(UIApplication uiapp, List<RebarDesglose> lista_RebarDesglose)
        {
            this._uiapp = uiapp;
            this._view=_uiapp.ActiveUIDocument.ActiveView;
            this.lista_RebarDesglose = lista_RebarDesglose;
            this.listaTransformada_RebarDesglose = new List<RebarDesglose>();

            this.listaTransformada_RebarDesgloseEstribo = new List<RebarDesglose>();
        }

        public CrearTrasformadaSobreVectorDesg _Trasform { get;  set; }

        public bool Ejecutar()
        {
            try
            {

                //NOTA CODIGO SE PUEDE MEJAR SI NO ENCUENTRO, BUSCAR ESTRIBO O TRABA--> CREAR CLASE APARTE PARA Y QUE DEVUELVA 'DatosHost'
                var barraAnalizada = lista_RebarDesglose.Where(c => c._tipoBarraEspecifico == TipoRebar.ELEV_BA_H ||
                                                                    c._tipoBarraEspecifico == TipoRebar.ELEV_BA_V)
                                                        .FirstOrDefault();

                if (barraAnalizada == null)
                {
                    //UtilDesglose.ErrorMsg($"Host no contiene barras longitudinales, no es posible encontrar Host");
                    return true;

                }
                //datos iniciales
                _DatosHost = new DatosHost(_uiapp, barraAnalizada);
                if (!_DatosHost.ObtenerPtoMedioYDireccion()) return false;
                if (!_DatosHost.ObtenerCentroPilarOmUro()) return false;

                //revisar si viga tiene angulo posito o negatico
                WraperRebarLargo curvaPrinciplar = lista_RebarDesglose[0].ListaCurvaBarras.Find(c => c.IsBarraPrincipal);
                if (curvaPrinciplar == null)
                {
                    UtilDesglose.ErrorMsg($"No se puedo obtener de coordenadas planos de host");
                    return false;
                }

                double angleRADNormalHostYEJeZ = Math.PI/2 -curvaPrinciplar.direccion.GetAngleEnZ_respectoPlanoXY(false);


                _Trasform = new CrearTrasformadaSobreVectorDesg(_DatosHost.CentroHost,-UtilDesglose.RadianeToGrados( angleRADNormalHostYEJeZ),- _view.ViewDirection);


                for (int i = 0; i < lista_RebarDesglose.Count; i++)
                {
                    RebarDesglose _RebarDesglose = lista_RebarDesglose[i];
                    RebarDesglose _COPYRebarDesgloseTrans = _RebarDesglose.CrearCopiarTrans(_Trasform);
                    listaTransformada_RebarDesglose.Add(_COPYRebarDesgloseTrans);
                }

                if (listaTransformada_RebarDesglose.Count == 0) return false;
            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener reorden de coordenadas: {ex.Message}");
                return false;
            }
            return true;
        }

        internal bool EjecutarEstribo()
        {
            try
            {
                //lista_RebarDesglose = lista_RebarDesglose.Where(c => (c._tipoBarraEspecifico == TipoRebar.ELEV_ES_VT || c._tipoBarraEspecifico == TipoRebar.ELEV_ES_VL ||
                //                                                ))
                //                                                 )
                //                                     .FirstOrDefault();
                //NOTA CODIGO SE PUEDE MEJAR SI NO ENCUENTRO, BUSCAR ESTRIBO O TRABA--> CREAR CLASE APARTE PARA Y QUE DEVUELVA 'DatosHost'
                var barraAnalizada = lista_RebarDesglose.Where(c => (c._tipoBarraEspecifico == TipoRebar.ELEV_ES_VT || c._tipoBarraEspecifico == TipoRebar.ELEV_ES_VL ||
                                                                    c._tipoBarraEspecifico == TipoRebar.ELEV_ES_V )
                                                                    )
                                                        .FirstOrDefault();

                if (barraAnalizada == null)
                {
                    //UtilDesglose.ErrorMsg($"Host no contiene barras longitudinales, no es posible encontrar Host");
                    return true;

                }
                //datos iniciales
                _DatosHost = new DatosHost(_uiapp, barraAnalizada);
                if (!_DatosHost.ObtenerPtoMedioYDireccion()) return false;
                if (!_DatosHost.ObtenerCentroPilarOmUro()) return false;

                //revisar si viga tiene angulo posito o negatico
                WraperRebarLargo curvaPrinciplar = lista_RebarDesglose[0].ListaCurvaBarras.Find(c => c.IsBarraPrincipal);
                XYZ normal_=lista_RebarDesglose[0]._rebar.GetShapeDrivenAccessor().Normal;
                if (curvaPrinciplar == null)
                {
                    UtilDesglose.ErrorMsg($"No se puedo obtener de coordenadas planos de host");
                    return false;
                }

                double angleRADNormalHostYEJeZ = Math.PI / 2 - normal_.GetAngleEnZ_respectoPlanoXY(false);


                _Trasform = new CrearTrasformadaSobreVectorDesg(_DatosHost.CentroHost, -UtilDesglose.RadianeToGrados(angleRADNormalHostYEJeZ), -_view.ViewDirection);


                for (int i = 0; i < lista_RebarDesglose.Count; i++)
                {
                    RebarDesglose _RebarDesglose = lista_RebarDesglose[i];

                    XYZ direcciotion=((Line)_RebarDesglose.CurvaMasLargo_WraperRebarLargo._curve).Direction;
                    if (_RebarDesglose._tipoBarraEspecifico == TipoRebar.ELEV_ES_V)
                        if( !Util.IsParallel(_RebarDesglose._rebar.GetShapeDrivenAccessor().Normal, _view.RightDirection))
                                continue;

                    RebarDesglose _COPYRebarDesgloseTrans = _RebarDesglose.CrearCopiarTrans_Estribo(_Trasform);
                    listaTransformada_RebarDesgloseEstribo.Add(_COPYRebarDesgloseTrans);
                }

                if (listaTransformada_RebarDesgloseEstribo.Count == 0) return false;
            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener reorden de coordenadas: {ex.Message}");
                return false;
            }
            return true;
        }
    }
}