using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.BuscarTipos
{
    public class TiposFamilyRebar
    {




        /// <summary>
        /// obtienen la familia del documento con el nombre  'name' 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rvtDoc"></param>
        /// <returns></returns>
        public static Family getFamilyRebarShape(string name, Document rvtDoc)
        {

            //ElementClassFilter sad  = new ElementClassFilter()




            List<Family> m_family = new List<Family>();

            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);
            filteredElementCollector.OfClass(typeof(Family));
            m_family = filteredElementCollector.Cast<Family>().ToList();


            Family familiaResult = null;

            foreach (var item in m_family)
            {
                if (item.Name == name)
                {
                    familiaResult = item;
                    return familiaResult;
                }
            }

            return familiaResult;

        }

    }
}
