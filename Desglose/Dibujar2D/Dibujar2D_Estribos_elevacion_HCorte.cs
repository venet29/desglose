using Desglose.Calculos;
using Desglose.DTO;
using Desglose.Model;
using Desglose.Dimensiones;
using Desglose.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using Desglose.Ayuda;

namespace Desglose.Dibujar2D
{
    public class Dibujar2D_Estribos_elevacion_HCorte: Dibujar2D_Barra_BASE
    {

        private GruposListasEstribo_HCorte _GruposListasEstribo;


        public Dibujar2D_Estribos_elevacion_HCorte(UIApplication _uiapp, GruposListasEstribo_HCorte gruposListasEstribo, Config_EspecialElev _Config_EspecialElv) : base(_uiapp)
        {


            _GruposListasEstribo = gruposListasEstribo;
            _config_EspecialElv = _Config_EspecialElv;


        }

        public bool PreDibujar_HV2(bool isId)
        {

            try
            {

                SeleccionarElementosV _SeleccionarElementosV = new SeleccionarElementosV(_uiapp, true);
                _SeleccionarElementosV.M1_1_CrearWorkPLane_EnCentroViewSecction();

         
                foreach (RebarDesglose_GrupoBarras_H itemGRUOP in _GruposListasEstribo.GruposRebarMismaLinea)
                {
                    for (int i = 0; i < itemGRUOP._GrupoRebarDesglose.Count; i++)
                    {

                        RebarDesglose_Barras_H item1 = itemGRUOP._GrupoRebarDesglose[i];
                        item1.contBarra = itemGRUOP._ListaRebarDesglose_GrupoBarrasRepetidas.Count + 1;
                       
                        RebarElevDTO _RebarElevDTO = item1.ObtenerRebarElevDTO_HV2Estribo(_uiapp, isId, _config_EspecialElv);
                        _RebarElevDTO.tipoBarra = TipoRebarElev.EstriboVigaElv;

                        Config_DatosEstriboElevVigas _Config_DatosEstriboElevVigas = new Config_DatosEstriboElevVigas()
                        {
                            CantidadEstriboCONF = "2ED."
                        };
                        _RebarElevDTO.Config_DatosEstriboElevVigas= _Config_DatosEstriboElevVigas;


                        GenerarBarra_2DH2(_RebarElevDTO);
                    }
                }

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }


        public bool Dibujar(XYZ posicionAUX)
        {

            try
            {
                SeleccionarElementosV _SeleccionarElementosV = new SeleccionarElementosV(_uiapp, true);
                _SeleccionarElementosV.M1_1_CrearWorkPLane_EnCentroViewSecction();



                for (int i = 0; i < _GruposListasEstribo.GruposRebarMismaLinea.Count; i++)
                {

                    RebarDesglose_GrupoBarras_H item1 = _GruposListasEstribo.GruposRebarMismaLinea[i];
                    item1.ObtenerTextos();
                    RebarDesglose_Barras_H _primerEstrivo = item1._GrupoRebarDesglose[0];
                    //_primerEstrivo.ObtenerTextos();

                    XYZ AUX_ptoini = _config_EspecialElv.Trasform_.EjecutarTransformInvertida(posicionAUX.AsignarZ(item1._ptoInicial.Z));
                    XYZ AUX_ptofinal = _config_EspecialElv.Trasform_.EjecutarTransformInvertida(posicionAUX.AsignarZ(item1._ptoFinal.Z));

                   CreadorDimensiones _CreadorDimensiones = new CreadorDimensiones(_doc, AUX_ptoini, AUX_ptofinal, "SRV-Arial Narrow 2mm Flecha CM");
                    _CreadorDimensiones.CrearConref_conTrans("", item1.replaceWithText, item1.textobelow, _primerEstrivo.refenciaInicial, _primerEstrivo.refenciaFinal);

                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al dibujar texto barras \n ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool DibujarH2_crearDimension()
        {
            try
            {
                if (!VAlidarDatos()) return false;
               
                SeleccionarElementosV _SeleccionarElementosV = new SeleccionarElementosV(_uiapp, true);
                _SeleccionarElementosV.M1_1_CrearWorkPLane_EnCentroViewSecction();

                for (int i = 0; i < _GruposListasEstribo.GruposRebarMismaLinea.Count; i++)
                {
                    RebarDesglose_GrupoBarras_H item1 = _GruposListasEstribo.GruposRebarMismaLinea[i];
                    if (!item1.ObtenerTextos()) continue;

                    RebarDesglose_Barras_H _primerEstrivo = item1._GrupoRebarDesglose[0];
                    //_primerEstrivo.ObtenerTextos();
                    
                    XYZ AUX_ptoini = _config_EspecialElv.Trasform_.EjecutarTransformInvertida(item1._ptoInicial);
                    XYZ AUX_ptofinal = _config_EspecialElv.Trasform_.EjecutarTransformInvertida(item1._ptoFinal);
                   
                    double zmedio = (AUX_ptoini.Z + AUX_ptofinal.Z) / 2;
                    AUX_ptoini = AUX_ptoini.AsignarZ(zmedio);
                    AUX_ptofinal = AUX_ptofinal.AsignarZ(zmedio);

                    CreadorDimensiones _CreadorDimensiones = new CreadorDimensiones(_doc, AUX_ptoini, AUX_ptofinal, "SRV-Arial Narrow 2mm Flecha CM");
                    _CreadorDimensiones.CrearConref_conTrans("", item1.replaceWithText, item1.textobelow, _primerEstrivo.refenciaInicial, _primerEstrivo.refenciaFinal);

                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al dibujar texto estribo   \n ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool VAlidarDatos()
        {
            if (_GruposListasEstribo == null) return false;
            if (_GruposListasEstribo.GruposRebarMismaLinea == null) return false;

            return true;
        }
    }
}
