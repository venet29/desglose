﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Desglose.Ayuda;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using System.Reflection;

namespace Desglose.ParametrosShare
{
    public class ManejadorCrearParametrosShare
    {
        private UIApplication _uiapp;
        private Document _doc;
        private string _rutaArchivoCompartido;
        private bool IsCrearArchivoParametrosCOmpartido;
        private object _versionREviT;

        private List<ElementId> listaIdExistentes;

        public ManejadorCrearParametrosShare(UIApplication uiapp, string RutaArchivoCompartido , bool _IsCrearArchivoParametrosCOmpartido)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._rutaArchivoCompartido = RutaArchivoCompartido;
            IsCrearArchivoParametrosCOmpartido = _IsCrearArchivoParametrosCOmpartido;
            listaIdExistentes = new List<ElementId>();
            _versionREviT = _uiapp.Application.VersionNumber;
        }

        public bool EjecutarDesglose() => Ejecutar(FactoryEntidadDefinition.CrearListaConParametrosDesglose(_uiapp));



        private bool Ejecutar(List<EntidadDefinition> lista)
        {
            string name = "";
            try
            {
                Document doc = _uiapp.ActiveUIDocument.Document;
                if (doc == null) return false;



                using (Transaction tran = new Transaction(_uiapp.ActiveUIDocument.Document, "Add shared param"))
                {
                    tran.Start();
                    foreach (EntidadDefinition item in lista)
                    {

                        if (!AddSharedTestParameter(item))
                        {
                            tran.RollBack();
                            return false;
                        }

                    }
                    tran.Commit();

                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" {name} - {ex.Message} ");
                return false;
            }


            return true;
        }

        public bool AddSharedTestParameter(EntidadDefinition entidadDefinition)
        {

            try
            {
                // si archivo de parametro comppartido no existe salir
                if (!File.Exists(_uiapp.Application.SharedParametersFilename))
                {
                    if (IsCrearArchivoParametrosCOmpartido)
                        M2_CrearArchivoDePArametrosCompartidos();
                    else
                    {
                        Util.ErrorMsg("Archivo de parametros compartidos no existe. Crear antes de asignar parametros compartidos");
                        return false;
                    }
                }

                if (entidadDefinition.nombreParametro == "B_")
                { }
                // check whether shared parameter exists
                if (M1_ShareParameterExists(_uiapp.ActiveUIDocument.Document, entidadDefinition.nombreParametro))
                {
                    return true;
                }
                Application revitApp = _uiapp.Application;
                //if (IsCrearArchivoParametrosCOmpartido)
                //    revitApp = M2_CrearArchivoDePArametrosCompartidos();
                //else
                //    revitApp = _uiapp.Application;

                //creadefinicio de parametroCon 'EntidadDefinition'
                Definition rebarSharedParamDef = M3_CrearDefinicionDeParametro(entidadDefinition);

                if (rebarSharedParamDef == null) return false;
                // get rebar category
                InstanceBinding binding = M4_ObtieneIsntaciaDecategoriaParaAsociar(entidadDefinition, revitApp);
                _uiapp.ActiveUIDocument.Document.ParameterBindings.Insert(rebarSharedParamDef, binding);
                return true;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"No se pudo crear el parámetro compartido: {entidadDefinition.nombreParametro} -" + ex.Message);
                return false;

            }
        }

        private bool M1_ShareParameterExists(Document doc, string paramName)
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
                if (intDef != null) listaIdExistentes.Add(intDef.Id);
                // get the category which is bound
                ElementBinding binding = bindingMap.get_Item(tempDefinition) as ElementBinding;

                CategorySet bindCategories = binding.Categories;
                foreach (Category category in bindCategories)
                {
                    if (category.Name == doc.Settings.Categories.get_Item(BuiltInCategory.OST_Rebar).Name)
                    {

                        return true;
                    }
                }
            }
            return false;
        }

        public bool M1_ShareCrearLista() => AyudaBuscaParametrerShared.CrearListaLog(_doc);


        private Application M2_CrearArchivoDePArametrosCompartidos()
        {
            // create shared parameter file
            String modulePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // String paramFile = _rutaArchivoCompartido; // modulePath + "\\RebarTestParameters.txt";
            String paramFile = modulePath + $"\\{_rutaArchivoCompartido}.txt";
            if (File.Exists(paramFile))
            {
                File.Delete(paramFile);
            }
            FileStream fs = File.Create(paramFile);
            fs.Close();

            // cache application handle
            Application revitApp = _uiapp.Application;

            // prepare shared parameter file
            _uiapp.Application.SharedParametersFilename = paramFile;
            return revitApp;
        }



        private Definition M3_CrearDefinicionDeParametro(EntidadDefinition entidadDefinition)
        {
            if (_versionREviT.ToString().Contains("2019") || _versionREviT.ToString().Contains("2018"))
            {
                return M3_CrearDefinicionDeParametro_down20(entidadDefinition);
            }
            else
            {
                return M3_CrearDefinicionDeParametro_Up20(entidadDefinition);
            }

        }

        private Definition M3_CrearDefinicionDeParametro_Up20(EntidadDefinition entidadDefinition)
        {
            // open shared parameter file
            DefinitionFile parafile = _uiapp.Application.OpenSharedParameterFile();

            DefinitionGroup apiGroup = parafile.Groups.get_Item(entidadDefinition.NombreGrupo);
            if (apiGroup == null)
                apiGroup = parafile.Groups.Create(entidadDefinition.NombreGrupo);  // create a group

            //busca si ya esta el parametro creado en el archivo compartido
            Definition rebarSharedParamDef = apiGroup.Definitions.get_Item(entidadDefinition.nombreParametro);
            if (rebarSharedParamDef == null) //si no lo encuentra lo crea
            {
                // create a visible param 
                ExternalDefinitionCreationOptions ExtDefinitionCreationOptions = new ExternalDefinitionCreationOptions(entidadDefinition.nombreParametro, entidadDefinition.TipoParametro);

                if (_versionREviT.ToString().Contains("2020") ||
                    _versionREviT.ToString().Contains("2021") ||
                    _versionREviT.ToString().Contains("2022"))
                {
                    if (BuiltInCategory.OST_Rebar == entidadDefinition._builtInCategory)
                        ExtDefinitionCreationOptions.HideWhenNoValue = entidadDefinition.EsOcultoCuandoNOvalor;//used this to show the parameter only in some rebar instances that will use it
                }

                if (entidadDefinition.Guid_ != null) ExtDefinitionCreationOptions.GUID = entidadDefinition.Guid_;

                ExtDefinitionCreationOptions.UserModifiable = entidadDefinition.EsModificable;//  set if users need to modify this
                ExtDefinitionCreationOptions.Visible = entidadDefinition.EsVisible;//  set if users need to modify this
                ExtDefinitionCreationOptions.Description = entidadDefinition.Description;
                rebarSharedParamDef = apiGroup.Definitions.Create(ExtDefinitionCreationOptions);
            }
            return rebarSharedParamDef;
        }
        private Definition M3_CrearDefinicionDeParametro_down20(EntidadDefinition entidadDefinition)
        {
            // open shared parameter file
            DefinitionFile parafile = _uiapp.Application.OpenSharedParameterFile();

            DefinitionGroup apiGroup = parafile.Groups.get_Item(entidadDefinition.NombreGrupo);
            if (apiGroup == null)
                apiGroup = parafile.Groups.Create(entidadDefinition.NombreGrupo);  // create a group

            //busca si ya esta el parametro creado en el archivo compartido
            Definition rebarSharedParamDef = apiGroup.Definitions.get_Item(entidadDefinition.nombreParametro);
            if (rebarSharedParamDef == null) //si no lo encuentra lo crea
            {
                // create a visible param 
                ExternalDefinitionCreationOptions ExtDefinitionCreationOptions = new ExternalDefinitionCreationOptions(entidadDefinition.nombreParametro, entidadDefinition.TipoParametro);

                //ExtDefinitionCreationOptions.GUID = new Guid(); //
                ExtDefinitionCreationOptions.UserModifiable = entidadDefinition.EsModificable;//  set if users need to modify this
                ExtDefinitionCreationOptions.Visible = entidadDefinition.EsVisible;//  set if users need to modify this
                ExtDefinitionCreationOptions.Description = entidadDefinition.Description;
                rebarSharedParamDef = apiGroup.Definitions.Create(ExtDefinitionCreationOptions);
            }
            return rebarSharedParamDef;
        }

        private InstanceBinding M4_ObtieneIsntaciaDecategoriaParaAsociar(EntidadDefinition entidadDefinition, Application revitApp)
        {

            CategorySet categories = revitApp.Create.NewCategorySet();

            for (int i = 0; i < entidadDefinition._builtInCategoryArray.Length; i++)
            {
                Category rebarCat = _uiapp.ActiveUIDocument.Document.Settings.Categories.get_Item(entidadDefinition._builtInCategoryArray[i]);
                categories.Insert(rebarCat);
            }

            // insert the new parameter
            InstanceBinding binding = revitApp.Create.NewInstanceBinding(categories);
            return binding;
        }

        /// <summary>
        /// Checks if a parameter exists based of a name
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>

    }
}
