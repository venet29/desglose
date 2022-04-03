using Desglose.Calculos;
using Desglose.DTO;
using Desglose.Model;
using Desglose.Tag;
using Desglose.Ayuda;
using Desglose.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Desglose.Dibujar2D
{
    public class Dibujar2D_Estribos_Corte_H
    {
        private UIApplication _uiapp;
        private  Config_EspecialCorte _config_EspecialCorte;
        private Document _doc;
        private View _view;
        private GruposListasTraslapoIguales_H _GruposListasTraslapoIguales;
        private GruposListasEstribo_H _GruposListasEstribo;
        private List<IRebarLosa_Desglose> _ListIRebarLosa;
        private RebarDesglose_GrupoBarras_H _rebarDesglose_GrupoBarras;
        private XYZ _puntoCentrealHost;
        double mayorDistancia;

        public XYZ ptoCentroPilarAlturaCOrte { get; private set; }

        public Dibujar2D_Estribos_Corte_H(UIApplication uiapp, GruposListasEstribo_H rebarDesglose_GrupoBarras, Config_EspecialCorte _Config_EspecialCorte)
        {
            _uiapp = uiapp;
            this._config_EspecialCorte = _Config_EspecialCorte;
            this._rebarDesglose_GrupoBarras = rebarDesglose_GrupoBarras.GruposRebarMismaLinea[0];
            this._puntoCentrealHost = rebarDesglose_GrupoBarras._DatosHost.CentroHost;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _uiapp.ActiveUIDocument.Document.ActiveView;
            _GruposListasTraslapoIguales = null;

            _ListIRebarLosa = new List<IRebarLosa_Desglose>();
        }


        public bool PreDibujar(View _view, View _viewOriginal, XYZ posicionInicial)
        {

            try
            {
           

        
                XYZ posicionAUX = XYZ.Zero;

                var listaEstribo= _rebarDesglose_GrupoBarras._GrupoRebarDesglose.Where(c => c._tipoBarraEspecifico == TipoRebar.ELEV_ES_V).ToList();
                var listaTrabas = _rebarDesglose_GrupoBarras._GrupoRebarDesglose.Where(c => c._tipoBarraEspecifico == TipoRebar.ELEV_ES_VT).ToList();


                double ZSleccion = posicionInicial.Z;

                //ptoCentroPilarAlturaCOrte= _puntoCentrealHost.ProjectExtendidaXY0(_view.RightDirection, posicionInicial).AsignarZ(ZSleccion);
                posicionInicial = _puntoCentrealHost.ProjectExtendidaXY0(_view.RightDirection, posicionInicial).AsignarZ(_puntoCentrealHost.Z);

                mayorDistancia = 0;
                //estribos
                for (int i = 0; i < listaEstribo.Count; i++)
                {
                    posicionAUX = posicionInicial;
                    RebarDesglose_Barras_H item1 = listaEstribo[i];

                    RebarElevDTO _RebarElevDTO = item1.ObtenerRebarCorteDTO(posicionAUX, _puntoCentrealHost, _uiapp, _view, _viewOriginal, _config_EspecialCorte);
                    GenerarBarra_2D(_RebarElevDTO);
                    posicionInicial = posicionInicial + _view.RightDirection *(mayorDistancia+ Util.CmToFoot(20));
                }
                //trabas
          
                for (int i = 0; i < listaTrabas.Count; i++)
                {
                    posicionAUX = posicionInicial;
                    RebarDesglose_Barras_H item1 = listaTrabas[i];

                    RebarElevDTO _RebarElevDTO = item1.ObtenerRebarCorteDTO(posicionAUX, _puntoCentrealHost, _uiapp, _view, _viewOriginal, _config_EspecialCorte);
                    GenerarBarra_2D(_RebarElevDTO);

                    posicionInicial = posicionInicial + _view.RightDirection * (mayorDistancia + Util.CmToFoot(20));
                }



            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }

        private bool GenerarBarra_2D(RebarElevDTO _RebarElevDTO)
        {
            try
            {
                //3)tag
                IGeometriaTag _newIGeometriaTag = FactoryGeomTagEstriboCorte.CrearIGeomTagRebarEstriboCorte(_uiapp, _RebarElevDTO);

                //4)barra
                IRebarLosa_Desglose rebarLosa = FactoryIRebarDesglose.CrearIRebarLosa(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                if (!rebarLosa.M1A_IsTodoOK()) return false;

                mayorDistancia = rebarLosa.mayorDistancia;
                _ListIRebarLosa.Add(rebarLosa);
                
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }


        public bool Dibujar()
        {

            try
            {
                if (_ListIRebarLosa.Count == 0) return false;
                using (TransactionGroup t = new TransactionGroup(_uiapp.ActiveUIDocument.Document))
                {
                    t.Start("CrearBarraInclinada-NH");
                    foreach (var rebarLosa in _ListIRebarLosa)
                    {
                        rebarLosa.M2A_GenerarBarra();

                    }
                    t.Assimilate();
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
    }
}
