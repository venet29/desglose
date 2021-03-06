using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.BuscarTipos
{
    internal class TiposMultiReferenceAnnotationType
    {

        public static Dictionary<string, MultiReferenceAnnotationType> ListaFamilias { get; set; }

        public static MultiReferenceAnnotationType elemetEncontrado;

        public static MultiReferenceAnnotationType M1_GetMultiReferenceAnnotationType(string name, Document rvtDoc)
        {

            if (BuscarDiccionario(name)) return elemetEncontrado;

            //Debug.WriteLine($" ---->   name:{name}");
            MultiReferenceAnnotationType elemento = M1_2_BuscarEnColecctor(name, BuiltInCategory.OST_MultiReferenceAnnotations, rvtDoc);

            AgregarDiccionario(name, elemento);

            return elemento;
        }
        private static bool BuscarDiccionario(string nombre)
        {
            elemetEncontrado = null;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, MultiReferenceAnnotationType>();
                return false;
            }
            else if (ListaFamilias.Count == 0)
            {
                return false;
            }


            if (ListaFamilias.ContainsKey(nombre))
                elemetEncontrado = ListaFamilias[nombre];

            if (elemetEncontrado != null && elemetEncontrado.IsValidObject == false)
            {
                ListaFamilias.Remove(nombre);
                elemetEncontrado = null;
            }

            return (elemetEncontrado == null ? false : true);
        }

        private static MultiReferenceAnnotationType M1_2_BuscarEnColecctor(string name, BuiltInCategory builtInCategory, Document rvtDoc)
        {
            MultiReferenceAnnotationType elemento = null;
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);
            filteredElementCollector.OfCategory(builtInCategory);
            var m_roomTagTypes = filteredElementCollector.ToList();
            foreach (var item in m_roomTagTypes)
            {
                if (item.Name == name)
                {
                    elemento = (MultiReferenceAnnotationType)item;
                    return elemento;
                }
            }

            return elemento;
        }

        public static MultiReferenceAnnotationType obtenerDefault(Document doc)
        {
            try
            {
                elemetEncontrado= MultiReferenceAnnotationType.CreateDefault(doc);
            }
            catch (Exception)
            {

                return null;
            }
            return elemetEncontrado;
        }
        public static MultiReferenceAnnotationType obtenerPrimeroDebaseDatos(Document doc)
        {
            try
            {
                elemetEncontrado = new FilteredElementCollector(doc)
                   .OfClass(typeof(MultiReferenceAnnotationType))
                    .Cast<MultiReferenceAnnotationType>()
                   .FirstOrDefault();
    
            }
            catch (Exception)
            {

                return null;
            }
            return elemetEncontrado;
        }
        private static void AgregarDiccionario(string nombre, MultiReferenceAnnotationType element)
        {
            if (element == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, MultiReferenceAnnotationType>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }


        public static bool CrearMultiReferenceAnnotationTypeIniciales(Document _doc)
        {


            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("MultiReferenceAnnotationTypeIniciales-NH");

                    ElementType _typeDefault = obtenerDefault( _doc);

                    if (_typeDefault != null)

                    {
                        ElementType _tipoLat = M1_GetMultiReferenceAnnotationType("MultiReferenceAnnotationType_LAT", _doc);
                        if (_tipoLat == null)
                        {
                            var _ResultLat = _typeDefault.Duplicate("MultiReferenceAnnotationType_LAT");
                          //  if (ParameterUtil.FindParaByName(newArrow50, "Tick Size") != null) ParameterUtil.SetParaInt(newArrow50, "Tick Size", Util.CmToFoot(1 / 10f));
                        }

                        ElementType _tipoDiamtros = M1_GetMultiReferenceAnnotationType("MultiReferenceAnnotationType_DIAM", _doc);
                        if (_tipoDiamtros == null)
                        {
                            var _ResultDiam = _typeDefault.Duplicate("MultiReferenceAnnotationType_DIAM");
                           // if (ParameterUtil.FindParaByName(newArrow75, "Tick Size") != null) ParameterUtil.SetParaInt(newArrow75, "Tick Size", Util.CmToFoot(0.66665 / 10f));
                        }
                        ElementType _tipoSegunElev = M1_GetMultiReferenceAnnotationType("MultiReferenceAnnotationType_SegunELEV", _doc);
                        if (_tipoSegunElev == null)
                        {
                            var _ResultSegunElev = _typeDefault.Duplicate("MultiReferenceAnnotationType_SegunELEV");
                            // if (ParameterUtil.FindParaByName(newArrow75, "Tick Size") != null) ParameterUtil.SetParaInt(newArrow75, "Tick Size", Util.CmToFoot(0.66665 / 10f));
                        }
                    }




                    t.Commit();
                }

            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                return false;
            }
            return true;
        }


    }
}
