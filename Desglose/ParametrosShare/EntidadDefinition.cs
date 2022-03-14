using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.ParametrosShare
{
    public class EntidadDefinition
    {
        public UIApplication _Uiapp { get; }
        public BuiltInCategory[] _builtInCategoryArray { get; set; }
        public BuiltInCategory _builtInCategory { get; set; }
        public string nombreParametro { get; }
        public string NombreGrupo { get; }
        public bool EsModificable { get; }
        public bool EsVisible { get; }
        public ParameterType TipoParametro { get; }
        public bool EsOcultoCuandoNOvalor { get; set; }
        public string Description { get; set; }
        public Guid Guid_ { get; set; }

        public EntidadDefinition(UIApplication uiapp, BuiltInCategory _builtInCategory, ParameterType tipoParametro,
                                string nameParametro, string NombreGrupo, bool EsModificable, bool EsOcultoCuandoNOvalor, bool EsVisible, string Description,
                                 string _guid = "")
        {
            this._Uiapp = uiapp;
            this._builtInCategory = _builtInCategory;
            this._builtInCategoryArray = new BuiltInCategory[] { _builtInCategory };
            this.nombreParametro = nameParametro;
            this.NombreGrupo = NombreGrupo;
            this.EsModificable = EsModificable;
            this.EsVisible = EsVisible;
            this.TipoParametro = tipoParametro;
            this.Description = Description;
            if (_guid != "")
            {
                this.Guid_ = new Guid(_guid);
            }

        }

        public EntidadDefinition(UIApplication uiapp, BuiltInCategory[] _builtInCategoryArray, ParameterType tipoParametro,
                                string nameParametro, string NombreGrupo, bool EsModificable, bool EsOcultoCuandoNOvalor, bool EsVisible, string Description,
                                 string _guid = "")
        {
            this._Uiapp = uiapp;
            this._builtInCategoryArray = _builtInCategoryArray;
            this.nombreParametro = nameParametro;
            this.NombreGrupo = NombreGrupo;
            this.EsModificable = EsModificable;
            this.EsVisible = EsVisible;
            this.TipoParametro = tipoParametro;
            this.Description = Description;
            if (_guid != "")
            {
                this.Guid_ = new Guid(_guid);
            }
        }

        public EntidadDefinition(string nameParametro, string _guid = "")
        {

            this.nombreParametro = nameParametro;

            if (_guid != "")
            {
                this.Guid_ = new Guid(_guid);
            }
        }
    }
}
