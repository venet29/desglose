using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Seleccionar
{
    public class SeleccionarDimensiones
    {

        public static Dimension ObtenerDimensionePorNombre(Document doc, string nombre)
        {



            List<Dimension> linearDimensions = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Dimensions)
                        .Cast<Dimension>().Where(q => q.DimensionShape == DimensionShape.Linear).ToList();

            List<Dimension> linearDimensions2 = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Dimensions).Cast<Dimension>().ToList();

            //buscar primer nivel
            FilteredElementCollector Colectornivel = new FilteredElementCollector(doc);
            Dimension Lv = Colectornivel
                          .OfCategory(BuiltInCategory.OST_Dimensions)
                          .Cast<Dimension>()
                         .Where(X => X.Name == nombre).FirstOrDefault();

            return Lv;
        }
        public static DimensionType ObtenerDimensionTypePorNombre(Document doc, string nombre)
        {



            List<DimensionType> m_family = new List<DimensionType>();

            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(doc);
            filteredElementCollector.OfClass(typeof(DimensionType));
            m_family = filteredElementCollector.Cast<DimensionType>().ToList();


            DimensionType familiaResult = null;

            foreach (var item in m_family)
            {
                if (item.Name == nombre)
                {
                    return familiaResult = item;
                    // return familiaResult;
                }
            }




            return familiaResult;
        }
    }
}
