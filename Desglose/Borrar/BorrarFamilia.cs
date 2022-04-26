
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Desglose.Ayuda;
using Desglose.Familias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Borrar
{
    public class BorrarFamilia
    {
        private readonly UIApplication _uiapp;
        private readonly Document _doc;
        private FilteredElementCollector _fec;
        private List<ElementId> _listaFamilia;
        private List<string> _listaNoEncotrado;


        public BorrarFamilia(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            _listaFamilia = new List<ElementId>();
            _listaNoEncotrado = new List<string>();
        }
        public bool BorrarTodasLasFamilias()
        {

           // UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
           // _updateGeneral.M3_DesCargarBarras();
            try
            {
                var listaRutasFamilias = FactoryCargarFamilias.CrearDiccionarioRutasFamilias("");
           
                foreach (var item in listaRutasFamilias)
                {
                    Family famToDelete = OBtenerFamiliaPorNombre(item.Item1);
                    if (famToDelete != null)
                        _listaFamilia.Add(famToDelete.Id);
                    else
                        _listaNoEncotrado.Add(item.Item1);
                }

                BorrarFamilias();
                Util.InfoMsg("Familias borradas correctamente");
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al borrar familias, ex:{ex.Message}");
                return false;
            }
         //   _updateGeneral.M4_CargarGenerar();
            return true;
        }

        private void BorrarFamilias()
        {
            if (_listaFamilia.Count > 0)
            {

                try
                {
                    using (Transaction trans = new Transaction(_doc))
                    {
                        trans.Start("Borrar Familias Armadura-NH");
                        _doc.Delete(_listaFamilia);
                        trans.Commit();
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    TaskDialog.Show("Error", message);
                    return;
                }

            }

        }

        private Family OBtenerFamiliaPorNombre(string nombreFamilia)
        {
            foreach (Element e in _fec.ToElements())
            {
                Family f = e as Family;
                if (null != f && f.Name.Equals(nombreFamilia))
                {
                    return f;
                }

            }

            return null;
        }

        private void ObtenerListaFamilias()
        {
            _fec = new FilteredElementCollector(_doc);
            _fec.OfClass(typeof(Family));
        }
    }
}
