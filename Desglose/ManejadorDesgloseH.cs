using Desglose.Ayuda;
using Desglose.Calculos;
using Desglose.Dibujar2D;
using Desglose.DTO;
using Desglose.Model;


using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Desglose.WPF;

namespace Desglose
{
    public class ManejadorDesgloseH
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
        private readonly UI_desglose _ui;
        private bool IScrearCorte;
        private CasoAnalisas CasoAnalisas;
        private CrearViewNH _CrearView;

        private View _ViewOriginal;


        public ManejadorDesgloseH(UIApplication uiapp, UI_desglose _ui)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._ui = _ui;
            this.IScrearCorte = true;
            this.CasoAnalisas = CasoAnalisas.AnalisisHorizontal;
        }

        public bool EjecutarDibujarBarrasEnElevacionH(Config_EspecialElev _Config_EspecialElv)
        {
            try
            {

                ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
                bool resultadoConexion = _ManejadorUsuarios.PostBitacora("EjecutarDibujarBarrasEnElevacionH");

                if (!resultadoConexion)
                {
                    Util.ErrorMsg("Error al validad credencial (No conexion)");
                    return false;
                }
                else if (!_ManejadorUsuarios.resultnh.Isok)
                {
                    Util.ErrorMsg("Error al validar Usuario");
                    return false;
                }

                // ThisApplication _ThisApplication = new ThisApplication(_uiapp);
                //  _ThisApplication.C4_M2_InfoGeometriaAnidada();
                bool isId = (bool)_ui.chb_id.IsChecked;
                SeleccionarRebarRectangulo administrador_ReferenciaRoom = new SeleccionarRebarRectangulo(_uiapp);
                if (!administrador_ReferenciaRoom.GetUnicamenteRebarSeleccionadosConRectaguloYFiltros()) return false;

                if (!AyudaObtenerListaDesglosada.ObtenerLista(administrador_ReferenciaRoom._ListaRebarSeleccionado, _uiapp)) return false;

                //hacer trasformada

                List<RebarDesglose> Lista_RebarDesglose = AyudaObtenerListaDesglosada.Lista_RebarDesglose;

                //*******************  importante
                GeneradorListaTrasfomardas _GeneradorListaTrasfomardas = new GeneradorListaTrasfomardas(_uiapp, Lista_RebarDesglose);
                if (!_GeneradorListaTrasfomardas.Ejecutar()) return false;

                _Config_EspecialElv.Trasform_ = _GeneradorListaTrasfomardas._Trasform;

                Lista_RebarDesglose = _GeneradorListaTrasfomardas.listaTransformada_RebarDesglose;

                //*********************


                //a)BARRAS
                GruposListasTraslapo_H _GruposListasTraslapo = new GruposListasTraslapo_H(_uiapp, Lista_RebarDesglose);
                if (!_GruposListasTraslapo.ObtenerGruposTraslapos()) return false;

                GruposListasTraslapoIguales_H _GruposListasTraslapoIguales = new GruposListasTraslapoIguales_H(_uiapp, _GruposListasTraslapo.GruposRebarMismaLinea);
                if (!_GruposListasTraslapoIguales.ObtenerGruposTraslaposIguales(_Config_EspecialElv)) return false;

                //b)ESTRIBO
                GruposListasEstribo_H _GruposListasEstribo = new GruposListasEstribo_H(_uiapp, Lista_RebarDesglose);
                if (!_GruposListasEstribo.ObtenerGruposEstribo()) return false;


                try
                {
                    using (TransactionGroup t = new TransactionGroup(_doc))
                    {
                        t.Start("Crear Elevacion");
                        //b)dibujar  barra
                        Dibujar2D_Barra_elevacion_H _Dibujar2D_elevcion = new Dibujar2D_Barra_elevacion_H(_uiapp, _GruposListasTraslapoIguales, _Config_EspecialElv);
                        if (_Dibujar2D_elevcion.PreDibujar(isId))
                            _Dibujar2D_elevcion.Dibujar();

                        //b)dibujar estribo
                        Dibujar2D_Estribos_elevacion_H _Dibujar2D_Estribo_Elev = new Dibujar2D_Estribos_elevacion_H(_uiapp, _GruposListasEstribo, _Config_EspecialElv);
                        _Dibujar2D_Estribo_Elev.Dibujar(_Dibujar2D_elevcion.posicionInicial);
                        t.Assimilate();
                    }
                }
                catch (Exception ex)
                {
                    string msj = ex.Message;
                }


            }
            catch (Exception ex)
            {
                UtilDesglose.DebugDescripcion(ex);
                return false;
            }
            return true;
        }


        public bool EjecutarDibujarBarrasEncorteH(Config_EspecialCorte _Config_EspecialCorte)
        {
            try
            {
                ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
                bool resultadoConexion = _ManejadorUsuarios.PostBitacora("EjecutarDibujarBarrasEncorteH");

                if (!resultadoConexion)
                {
                    Util.ErrorMsg("Error al validad credencial (No conexion)");
                    return false;
                }
                else if (!_ManejadorUsuarios.resultnh.Isok)
                {
                    Util.ErrorMsg("Error al validar Usuario");
                    return false;
                }


                _CrearView = null;
                // ThisApplication _ThisApplication = new ThisApplication(_uiapp);
                //  _ThisApplication.C4_M2_InfoGeometriaAnidada();

                _ViewOriginal = _uiapp.ActiveUIDocument.ActiveView;

                SeleccionarRebarRectangulo administrador_ReferenciaRoom = new SeleccionarRebarRectangulo(_uiapp);
                if (!administrador_ReferenciaRoom.GetUnicamenteRebarSeleccionadosConRectaguloYFiltros()) return false;

                if (!AyudaObtenerListaDesglosada.ObtenerLista(administrador_ReferenciaRoom._ListaRebarSeleccionado, _uiapp)) return false;

                //hacer trasformada

                List<RebarDesglose> Lista_RebarDesglose = AyudaObtenerListaDesglosada.Lista_RebarDesglose;

                //*******************  importante
                GeneradorListaTrasfomardas _GeneradorListaTrasfomardas = new GeneradorListaTrasfomardas(_uiapp, Lista_RebarDesglose);
                if (!_GeneradorListaTrasfomardas.Ejecutar()) return false;

                _Config_EspecialCorte.Trasform_ = _GeneradorListaTrasfomardas._Trasform;

                Lista_RebarDesglose = _GeneradorListaTrasfomardas.listaTransformada_RebarDesglose;

                //*********************

                //ESTRIBO
                GruposListasEstribo_H _GruposListasEstribo = new GruposListasEstribo_H(_uiapp, Lista_RebarDesglose);
                if (!_GruposListasEstribo.ObtenerGruposEstribo()) return false;

                if (!_GruposListasEstribo.ObteneGruposDeBarraEnELev()) return false;


                if (!GenerarCorte_H(_GruposListasEstribo)) return false;

                if (_GruposListasEstribo.GruposRebarMismaLinea.Count != 1)
                {
                    UtilDesglose.ErrorMsg($"Error al seleccionar estribos. Se han seleccionado {_GruposListasEstribo.GruposRebarMismaLinea.Count} grupos de estribos, se debe seleccionar solo 1 grupo de Estribo");
                }

                //seleccionar pto
                SeleccionarElementosV _SeleccionarElementosV = new SeleccionarElementosV(_uiapp, true);
                _SeleccionarElementosV.M1_1_CrearWorkPLane_EnCentroViewSecction();
                var lista = CrearListaPtos.M2_ListaPtoSimple(_uiapp, 1);
                if (lista.Count == 0) return false;

                XYZ posicionInicial = lista[0];

                try
                {
                    using (TransactionGroup t = new TransactionGroup(_doc))
                    {
                        t.Start("Crear cortes");

                        Dibujar2D_Estribos_Corte_H _Dibujar2D_Estribos_Corte = new Dibujar2D_Estribos_Corte_H(_uiapp, _GruposListasEstribo, _Config_EspecialCorte);
                        if (_Dibujar2D_Estribos_Corte.PreDibujar(_CrearView.section, _ViewOriginal, lista[0]))
                        {
                            _Dibujar2D_Estribos_Corte.Dibujar();
                        }
                        Dibujar2D_Barra_Corte_TAg_H _dibujar2D_Barra_Corte_TAg = new Dibujar2D_Barra_Corte_TAg_H(_uiapp,
                                                            _GruposListasEstribo.listaBArrasEnElev, _GruposListasEstribo._DatosHost.CaraCentral, _Config_EspecialCorte);

                        //   dibujar detalle de barras transversales
                        Dibujar2D_Barras_Corte_H _Dibujar2D_Barras_Corte_H = new Dibujar2D_Barras_Corte_H(_uiapp, _GruposListasEstribo, _Config_EspecialCorte);
                        if (_Dibujar2D_Barras_Corte_H.PreDibujar(_CrearView.section, _ViewOriginal))
                        {
                            _Dibujar2D_Barras_Corte_H.Dibujar();
                        }

                        t.Assimilate();
                    }
                }
                catch (Exception ex)
                {
                    string msj = ex.Message;
                }



            }
            catch (Exception ex)
            {
                UtilDesglose.DebugDescripcion(ex);
                return false;
            }
            return true;
        }



        private bool GenerarCorte_H(GruposListasEstribo_H _GruposListasEstribo)
        {
            if (IScrearCorte)
            {
                List<XYZ> listaPto = _GruposListasEstribo.ListaPtoSeccion;// ()  CrearListaPtos.M2_ListaPtoSimple(_uiapp, 2);
                _CrearView = new CrearViewNH(_doc, _ui.dtnameCorte.Text);

                XYZ origen = (listaPto[0] + listaPto[1]) * 0.5;
                double ancho_entrandoView_foot = _GruposListasEstribo._DatosHost.LargoMAximoHost_foot;
                double ancho_paralelaVIew_foot = listaPto[0].DistanceTo(listaPto[1]) / 2 + UtilDesglose.CmToFoot(20);
                XYZ direccionZ = _GruposListasEstribo._DatosHost.CaraCentral.FaceNormal.Normalize();
                XYZ direccionY_paralelaVIew = _GruposListasEstribo._DatosHost.Direccion_ParalelaView.Normalize();

                GenerarBoundingBoxXYZDTO _generarBoundingBoxXYZ = new GenerarBoundingBoxXYZDTO()
                {
                    origien = origen,
                    direccionZ = direccionZ,
                    direccionY_paralelaVIew = direccionY_paralelaVIew,
                    xmin = -ancho_entrandoView_foot * 4,
                    xmax = ancho_entrandoView_foot,
                    ymin = -ancho_paralelaVIew_foot,
                    ymax = ancho_paralelaVIew_foot,
                    zmin = -ConstNH.CONST_ANCHO_CORTE_DESGLOSE,
                    zmax = ConstNH.CONST_ANCHO_CORTE_DESGLOSE
                };

                var _AyudaGenerarBoundingBoxXYZ = AyudaGenerarBoundingBoxXYZ.GetSectionconDIreccion(_ViewOriginal, _generarBoundingBoxXYZ);


                if (!_CrearView.M3_CrearDetailViewConTrasn2(_AyudaGenerarBoundingBoxXYZ))
                {
                    UtilDesglose.ErrorMsg($"Error al crear corte  View=null");
                    return false;
                }
                _uiapp.ActiveUIDocument.ActiveView = _CrearView.section;
            }


            if (_CrearView == null)
            {
                UtilDesglose.ErrorMsg($"Error al crear corte  View=null");
                return false;
            }
            if (_CrearView.section == null)
            {
                UtilDesglose.ErrorMsg($"Error al crear corte  View=null");
                return false;
            }
            return true;
        }

    }




}
