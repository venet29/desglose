using Desglose.Calculos;
using Desglose.DTO;
using Desglose.Tag;
using Desglose.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using Desglose.Ayuda;
using Desglose.Dimensiones;
using Desglose.Tag.TipoBarraV;
using Desglose.Tag.TipoBarraH;

namespace Desglose.Dibujar2D
{
    public class Dibujar2D_Barra_BASE
    {
        protected UIApplication _uiapp;
        protected Document _doc;
        protected View _view;
        protected GruposListasTraslapoIguales_H _GruposListasTraslapoIguales_H;
        
        protected GruposListasTraslapoIguales_V _GruposListasTraslapoIguales_v;
        protected Config_EspecialElev _config_EspecialElv;

        //private GruposListasEstribo _GruposListasEstribo;
        protected List<IRebarLosa_Desglose> _ListIRebarLosa;

        public XYZ posicionInicial { get;  set; }

        public Dibujar2D_Barra_BASE(UIApplication _uiapp, GruposListasTraslapoIguales_V gruposListasTraslapoIguales, Config_EspecialElev _Config_EspecialElv)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _uiapp.ActiveUIDocument.Document.ActiveView;
            _GruposListasTraslapoIguales_v = gruposListasTraslapoIguales;
            _config_EspecialElv = _Config_EspecialElv;
            //  _GruposListasEstribo = gruposListasEstribo;
            _ListIRebarLosa = new List<IRebarLosa_Desglose>();
        }
        public Dibujar2D_Barra_BASE(UIApplication _uiapp, GruposListasTraslapoIguales_H gruposListasTraslapoIguales, Config_EspecialElev _Config_EspecialElv)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _uiapp.ActiveUIDocument.Document.ActiveView;
            _GruposListasTraslapoIguales_H = gruposListasTraslapoIguales;
            _config_EspecialElv = _Config_EspecialElv;
            //  _GruposListasEstribo = gruposListasEstribo;
            _ListIRebarLosa = new List<IRebarLosa_Desglose>();
        }
        public Dibujar2D_Barra_BASE(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _uiapp.ActiveUIDocument.Document.ActiveView;
            //  _GruposListasEstribo = gruposListasEstribo;
            _ListIRebarLosa = new List<IRebarLosa_Desglose>();
        }
        protected void CrearDimensionENtreBArras(RebarElevDTO rebarElevDTO, RebarElevDTO rebarElevDTOANterior)
        {
            XYZ iNICIAL = rebarElevDTOANterior.ptoini.AsignarZ(rebarElevDTO.ptoini.Z)-_view.RightDirection*Util.CmToFoot(5);
            CreadorDimensiones _CreadorDimensiones = new CreadorDimensiones(_doc, rebarElevDTOANterior.ptofinal - _view.RightDirection * Util.CmToFoot(5), iNICIAL, "SRV-Arial Narrow 2mm Flecha CM");
            _CreadorDimensiones.Crear_conTrans();
        }

        protected bool GenerarBarra_2D(RebarElevDTO _RebarElevDTO)
        {
            try
            {
                //3)tag
                
                IGeometriaTag _newIGeometriaTag =  new GeomeTagBarrarElev(_uiapp, _RebarElevDTO);

                //4)barra
                IRebarLosa_Desglose rebarLosa = FactoryIRebarDesglose.CrearIRebarLosa(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                if (!rebarLosa.M1A_IsTodoOK()) return false;

                _ListIRebarLosa.Add(rebarLosa);



            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }
        protected bool GenerarBarra_2DH2(RebarElevDTO _RebarElevDTO)
        {
            try
            {
                //3)tag
                IGeometriaTag _newIGeometriaTag = FactoryGeomTagRebarH.CrearGeometriaTagH(_uiapp, _RebarElevDTO);
                //4)barra
                IRebarLosa_Desglose rebarLosa = FactoryIRebarDesglose.CrearIRebarLosa(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                if (!rebarLosa.M1A_IsTodoOK()) return false;

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
