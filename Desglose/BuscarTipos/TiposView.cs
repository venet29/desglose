using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.BuscarTipos
{
    internal class TiposView
    {

        public static View BusarView(Document _doc, string nombreVista)
        {
            try
            {

                FilteredElementCollector collector = new FilteredElementCollector(_doc);
                var asdListaVIew2 = collector.OfClass(typeof(View)).Cast<View>()
                                            .Where(c => c.Name == nombreVista).FirstOrDefault();

                return asdListaVIew2;
            }
            catch (Exception )
            {


            }
            return null;
        }
    }
}
