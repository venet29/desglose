using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Desglose.Ayuda;
using Desglose.DTO;
using Desglose.Extension;
using System;
using System.Collections.Generic;

namespace Desglose.Dibujar2D
{
    internal class SeleccionarElementosV
    {
        protected UIApplication _uiapp;
      
        private  ConfiguracionIniciaWPFlBarraVerticalDTO _confiEnfierradoDTO;
        private  List<Level> _listaLevelTotal;

        protected UIDocument _uidoc;
        protected Document _doc;
        protected View _view;
        private bool ConTransaccionAlCrearSketchPlane;

        protected XYZ _PtoInicioBaseBordeMuro;
        protected XYZ _origenSeccionView; //punto mas al derecha, abajo y mas cerca de pantalla a la  vista
        protected XYZ _RightDirection;//direccion paralalea a la pantalla (izq hacia derecha)
        protected XYZ _ViewNormalDirection6;//direccion perpendicular a la pantalla
        private List<double> ListaLevelIntervalo;
        protected View3D _view3D_paraBuscar;

        public SeleccionarElementosV(UIApplication uiapp, bool ConTransaccionAlCrearSketchPlane = true)
        {
       
            this._uidoc = uiapp.ActiveUIDocument;
            this._doc = _uidoc.Document;
            this._view = _doc.ActiveView;
            this.ConTransaccionAlCrearSketchPlane = ConTransaccionAlCrearSketchPlane;
        }

        public bool M1_1_CrearWorkPLane_EnCentroViewSecction()
        {
            _origenSeccionView = _view.Origin;
            _RightDirection = _view.RightDirection.Redondear(8);
            _ViewNormalDirection6 = _view.ViewDirection.Redondear(8);

            double AnchoView = _view.get_Parameter(BuiltInParameter.VIEWER_BOUND_OFFSET_FAR).AsDouble();
            XYZ NuevoOrigen = _origenSeccionView + -_ViewNormalDirection6 * AnchoView / 2;

            if (!M1_1_1_CrearOAsignarSketchPlane(NuevoOrigen)) return false;


            return true;
        }

        private bool M1_1_1_CrearOAsignarSketchPlane(XYZ NuevoOrigen)
        {
            if (_view == null) return false;

            bool isCrearPlane = false;


            Plane skInicial = null;

            if (_view.SketchPlane == null)
            { isCrearPlane = true; }
            else
            {
                skInicial = _view.SketchPlane.GetPlane();
                if (skInicial == null)
                { isCrearPlane = true; }
                else if ((!skInicial.Origin.IsAlmostEqualTo(NuevoOrigen)) || (!skInicial.Normal.IsAlmostEqualTo(_ViewNormalDirection6)))
                {
                    isCrearPlane = true;
                }
            }

            if (isCrearPlane)
            {
                try
                {
                    if (ConTransaccionAlCrearSketchPlane)
                    {
                        using (Transaction t = new Transaction(_doc))
                        {
                            var result = t.GetStatus();
                            t.Start("CreandoSketchPlane-NH");

                            CrearSketchPlane(NuevoOrigen);
                            t.Commit();
                        }
                    }
                    else
                    {
                        CrearSketchPlane(NuevoOrigen);
                    }
                }
                catch (Exception ex)
                {
                    Util.ErrorMsg($"Error en 'CrearOAsignarSketchPlane'    \n\n  ex: {ex.Message}");
                    return false;
                }

            }
            return true;
        }

        private void CrearSketchPlane(XYZ NuevoOrigen)
        {
            Plane plano = Plane.CreateByNormalAndOrigin(_ViewNormalDirection6, NuevoOrigen);
            SketchPlane sk = SketchPlane.Create(_doc, plano); ;
            _view.SketchPlane = sk;
        }
    }
}