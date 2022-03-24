using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.ParametrosShare
{
    public class ConfiguracionInicialParametros
    {
        private UIApplication _uiapp;
        private Document _doc;

        public ConfiguracionInicialParametros(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
        }

  
        public bool AgregarParametrosShareDesglose()
        {
            ManejadorCrearParametrosShare _definicionManejador = new ManejadorCrearParametrosShare(_uiapp, RutaArchivoCompartido: "ParametrosNH",true);
            if (!_definicionManejador.EjecutarDesglose()) return false;
            return true;
        }

    }


}
