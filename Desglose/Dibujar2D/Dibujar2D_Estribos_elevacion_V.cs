using Desglose.Calculos;
using Desglose.Model;
using Desglose.Dimensiones;
using Desglose.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;

namespace Desglose.Dibujar2D
{
    public class Dibujar2D_Estribos_elevacion_V
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;
        private GruposListasTraslapoIguales_V _GruposListasTraslapoIguales;
        private GruposListasEstribo_V _GruposListasEstribo;
        private List<IRebarLosa> _ListIRebarLosa;

        public Dibujar2D_Estribos_elevacion_V(UIApplication _uiapp, GruposListasEstribo_V gruposListasEstribo)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _uiapp.ActiveUIDocument.Document.ActiveView;
            _GruposListasTraslapoIguales = null;
            _GruposListasEstribo = gruposListasEstribo;
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

                    RebarDesglose_GrupoBarras_V item1 = _GruposListasEstribo.GruposRebarMismaLinea[i];
                    item1.ObtenerTextos();
                    RebarDesglose_Barras_V _primerEstrivo = item1._GrupoRebarDesglose[0];
                    //_primerEstrivo.ObtenerTextos();

                   CreadorDimensiones _CreadorDimensiones = new CreadorDimensiones(_doc, posicionAUX.AsignarZ(item1._ptoInicial.Z), posicionAUX.AsignarZ(item1._ptoFinal.Z), "SRV-Arial Narrow 2mm Flecha CM");
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
