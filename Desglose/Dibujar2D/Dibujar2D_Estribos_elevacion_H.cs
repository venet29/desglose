using Desglose.Calculos;
using Desglose.DTO;
using Desglose.Model;
using Desglose.Dimensiones;
using Desglose.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;

namespace Desglose.Dibujar2D
{
    public class Dibujar2D_Estribos_elevacion_H
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;
        private GruposListasTraslapoIguales_H _GruposListasTraslapoIguales;
        private GruposListasEstribo_H _GruposListasEstribo;
        private  Config_EspecialElev _config_EspecialElv;
        private List<IRebarLosa> _ListIRebarLosa;

        public Dibujar2D_Estribos_elevacion_H(UIApplication _uiapp, GruposListasEstribo_H gruposListasEstribo, Config_EspecialElev _Config_EspecialElv)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _uiapp.ActiveUIDocument.Document.ActiveView;
            _GruposListasTraslapoIguales = null;
            _GruposListasEstribo = gruposListasEstribo;
            _config_EspecialElv = _Config_EspecialElv;
            _ListIRebarLosa = new List<IRebarLosa>();
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

                return false;
            }
            return true;
        }


    }
}
