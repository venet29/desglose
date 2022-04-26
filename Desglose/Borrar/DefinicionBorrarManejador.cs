
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Desglose.Ayuda;
using Desglose.ParametrosShare;
using System;
using System.Collections.Generic;



namespace Desglose.Borrar
{

    public class DefinicionBorrarManejador
    {
        private UIApplication _uiapp;


     //   public Dictionary<string, ElementId> listaParameter;
        private List<ElementId> listaIdExistentes;

        public DefinicionBorrarManejador(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            listaIdExistentes = new List<ElementId>();
          //  listaParameter = new Dictionary<string, ElementId>();

        }

        public void EjecutarBorrarParametros()
        {
            List<EntidadDefinition> lista = new List<EntidadDefinition>();

            lista.AddRange(FactoryEntidadDefinition.CrearListaConParametrosDesglose(_uiapp));

            //lista.AddRange(FactoryEntidadDefinition.CrearListaConParametrosFundaciones(_uiapp));
            // lista.AddRange(FactoryEntidadDefinition.CrearListaConParametrosEscalera(_uiapp));

           // UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
          //  _updateGeneral.M3_DesCargarBarras();

            string nameParametro = "";
            try
            {
                Document doc = _uiapp.ActiveUIDocument.Document;
                if (doc == null) return;

                    foreach (var item in lista)
                    {
                        nameParametro = item.nombreParametro;
                        M1_ShareParameterExists(doc, item.nombreParametro);
                    }

                    if (listaIdExistentes.Count == 0 )
                    {
                        Util.InfoMsg("No se encontraron parametros para borrar");
                        return;
                    }
            

                    using (Transaction tran = new Transaction(_uiapp.ActiveUIDocument.Document, "Add shared param"))
                    {
                        tran.Start();
                        doc.Delete(listaIdExistentes);
                        tran.Commit();
                    }
                
                Util.InfoMsg("Datos parameros compartidos borrados");
            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Parametro:  " + ex.Message);
            }

          //  _updateGeneral.M4_CargarGenerar();
        }


        private void M1_ShareParameterExists(Document doc, string paramName)
        {
            BindingMap bindingMap = doc.ParameterBindings;
            DefinitionBindingMapIterator iter = bindingMap.ForwardIterator();
            iter.Reset();

            while (iter.MoveNext())
            {
                Definition tempDefinition = iter.Key;
                // find the definition of which the name is the appointed one
                if (String.Compare(tempDefinition.Name, paramName) != 0)
                {
                    continue;
                }
                InternalDefinition intDef = (InternalDefinition)iter.Key;
                listaIdExistentes.Add(intDef.Id);
                return;
            }
        }




    }
}
