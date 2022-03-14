using Autodesk.Revit.DB;
using Desglose.Ayuda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.ParametrosShare
{
    public class AyudaBuscaParametrerShared
    {
        private static Dictionary<string, ElementId> listaParameter;

        public static Dictionary<string, ElementId> ObtenerListaPArameterShare(Document doc)
        {
            listaParameter = new Dictionary<string, ElementId>();
            M0_ObtenerListaShareParameter(doc);
            return listaParameter;
        }

        public static ElementId ObtenerParameterShare(Document doc, string name)
        {
            listaParameter = new Dictionary<string, ElementId>();
            M0_ObtenerListaShareParameter(doc);

            return listaParameter[name];
        }
        private static void M0_ObtenerListaShareParameter(Document doc)
        {
            BindingMap bindingMap = doc.ParameterBindings;
            DefinitionBindingMapIterator iter = bindingMap.ForwardIterator();
            iter.Reset();

            while (iter.MoveNext())
            {
                Definition tempDefinition = iter.Key;
                // find the definition of which the name is the appointed one

                InternalDefinition intDef = (InternalDefinition)iter.Key;
                if (intDef == null) continue;
                if (!listaParameter.ContainsKey(tempDefinition.Name))
                    listaParameter.Add(tempDefinition.Name, intDef.Id);


            }
        }

        public static bool CrearListaLog(Document _doc)
        {
            try
            {

                listaParameter = new Dictionary<string, ElementId>();
                M0_ObtenerListaShareParameter(_doc);

                ConstNH.sbLog.Clear();
                foreach (var item in listaParameter)
                {
                    string para = item.Key + "  id:" + item.Value.IntegerValue;
                    ConstNH.sbLog.AppendLine(para);
                }

                string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
               // LogNH.guardar_registro_StringBuilder(ConstNH.sbLog, path, "ListaPArametros" + DateTime.Now.ToString("MM_dd_yyyy Hmmss").ToString());
            }
            catch (Exception ex)
            {

                return true;
            }
            return false; ;
        }
    }
}
