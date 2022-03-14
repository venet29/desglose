﻿using System;
using System.Collections.Generic;
using Desglose.Ayuda;
using Desglose.DTO;
using Desglose.Model;
using Desglose.Tag;
using Desglose.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

namespace Desglose.Dibujar2D
{
    internal class Dibujar2D_Barra_Corte_TAg_H
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;
        private List<RebarDesglose_Barras_H> listaBArrasEnElev;
        private List<Curve> ListaCurvasZcero;
        private readonly PlanarFace caraInferior;

        private Config_EspecialCorte _Config_EspecialCorte;

        public Dibujar2D_Barra_Corte_TAg_H(UIApplication uiapp, List<RebarDesglose_Barras_H> listaBArrasEnElev, PlanarFace caraInferior, Config_EspecialCorte _Config_EspecialCorte)
        {
            _uiapp = uiapp;
            _doc = _uiapp.ActiveUIDocument.Document;
            _view = _doc.ActiveView;
            this.listaBArrasEnElev = listaBArrasEnElev;
            this.caraInferior = caraInferior;
            this._Config_EspecialCorte = _Config_EspecialCorte;
        }

        internal void CrearTAg(ViewSection section)
        {
            try
            {

                //1)conf
                double valorZ = section.Origin.Z;

                ConfiguracionTAgBarraDTo confBarraTag = new ConfiguracionTAgBarraDTo()
                {
                    desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0),
                    tagOrientation = TagOrientation.Horizontal

                };

                ObtenerPerimetoHostConCero();

                using (Transaction t = new Transaction(_uiapp.ActiveUIDocument.Document))
                {
                    t.Start("CrearTAgBarrasElev");
                    //2)
                    foreach (RebarDesglose_Barras_H item in listaBArrasEnElev)
                    {                        
                        XYZ Direccion = AyudaObtenerDireccionTAgCorte.Obtener(ListaCurvasZcero, item.ptoMedio.AsignarZ(0));

                        XYZ LeaderEnd = item.ptoMedio;
                        XYZ LeaderElbow = LeaderEnd + Direccion * Util.CmToFoot(10); ;
                        XYZ ptoTag = LeaderEnd + Direccion * Util.CmToFoot(15);

                        Rebar _rebar = item._rebarDesglose._rebar;

                        Config_EspecialElev _auxConfig_EspecialElev = new Config_EspecialElev()
                        { direccionMuevenBarrasFAlsa = _view.RightDirection };

                        GeomeTagBarrarElev _GeomeTagBarrarElev = new GeomeTagBarrarElev(_uiapp, item.ObtenerRebarElevDTO(LeaderEnd, _uiapp, false, _auxConfig_EspecialElev));
                        _GeomeTagBarrarElev.M3_DefinirRebarShape();

                        TagBarra _TagBarra = _GeomeTagBarrarElev.TagP0_Tipo;

                        _TagBarra.posicion = ptoTag.AsignarZ(valorZ);
                        _TagBarra.LeaderElbow = LeaderElbow.AsignarZ(valorZ);
                        _TagBarra.LeaderEnd = LeaderEnd.AsignarZ(valorZ);
                        _TagBarra.IsDIrectriz = true;
                        _TagBarra.IsLibre = true;

                        _TagBarra.DibujarTagRebar_ConLibre(_rebar, _doc, section, confBarraTag);

                    }
                    t.Commit();
                }



            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al generar tag de barras verticales ex:{ex.Message}");
                throw;
            }        
        }

        private void ObtenerPerimetoHostConCero()
        {
            var perimetro = caraInferior.GetEdgesAsCurveLoops();
            ListaCurvasZcero = new List<Curve>();
            foreach (CurveLoop item in perimetro)
            {
                foreach (Curve _curve in item)
                {
                    ListaCurvasZcero.Add(Line.CreateBound(_curve.GetEndPoint(0).AsignarZ(0), _curve.GetEndPoint(1).AsignarZ(0)));
                }
            }
        }
    }
}