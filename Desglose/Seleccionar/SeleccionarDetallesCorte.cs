using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Desglose.Ayuda;
using Desglose.FILTER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Seleccionar
{
    public class SeleccionarDetallesCorte
    {
        private readonly UIApplication uiapp;
        private Document _doc;
        private UIDocument _uidoc;

        public SeleccionarDetallesCorte(UIApplication _uiapp)
        {
            uiapp = _uiapp;
            _doc = _uiapp.ActiveUIDocument.Document;
            _uidoc = _uiapp.ActiveUIDocument;
        }

        public bool SeleccionarElevacionCorte()
        {
            try
            {
                ISelectionFilter f = new RebarSelectionDetallesCorte();
                //selecciona un objeto floor
                var _ListaRebarSeleccionado = _uidoc.Selection.PickElementsByRectangle(f, "Seleccionar ").Select(c=>c.Id).ToList();

                if (_ListaRebarSeleccionado.Count == 0) return false;

                try
                {
                    using (Transaction t = new Transaction(_doc, "Borrar-RT7"))
                    {
                        t.Start();
                        _doc.Delete(_ListaRebarSeleccionado);
                        t.Commit();
                    }

                }
                catch (Exception ex)
                {
                    Util.ErrorMsg($"Error al crear anotacion  EX:{ex.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
         
                return false;
            }

           // Util.InfoMsg("Proceso Terminado");
            return true;

        }
    }
}
