using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Desglose.Ayuda;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.FILTER
{
    public class RebarSelectionDetallesCorte : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            if (element == null) return false;
            if (element.Category == null) return false;

            Debug.Print(element.Category.Name);

            if (element.Category.Name == "Lines")
            {
              //  var casd = (DetailLine)element;

                ParameterSet paras = element.Parameters;

                foreach (Parameter param in paras)
                {
                    if (param.Definition.Name == "Line Style")
                    {
                        string name_ = ParameterUtil.FindValueParaByName(paras, "Line Style", element.Document);

                        if(name_== "SRV-3")
                            return true;
                    }
                }
            }

            if (element.Category.Name == "Structural Rebar Tags" || element.Category.Name == "Multi-Rebar Annotations" || element.Name== "2.5mm Arial Narrow")
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }
    }

}
