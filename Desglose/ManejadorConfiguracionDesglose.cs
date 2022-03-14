using Desglose.Ayuda;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using Desglose.ParametrosShare;
using Desglose.Familias;

namespace Desglose
{
    public class ManejadorConfiguracionDesglose
    {

        public static bool cargar(UIApplication _uiapp, bool IsMje = true)
        {
            if (_uiapp == null) return false;
            Document _doc = _uiapp.ActiveUIDocument.Document;
            View _view = _doc.ActiveView;

         //  ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
          //  _ManejadorUsuarios.PostInscripcion("NHdelporte");


            try
            {
                using (TransactionGroup transGroup = new TransactionGroup(_doc))
                {
                    transGroup.Start("Inicio ConfiguracionInicialGeneral-NH");

                   // DefinirArchivoShare.Ejecutar(_uiapp);

                   //1-parametros compartidos
                    ConfiguracionInicialParametros configuracionInicial = new ConfiguracionInicialParametros(_uiapp);
                    configuracionInicial.AgregarParametrosShareDesglose();


                    // familias
                    ManejadorCargarFAmilias _ManejadorCargarFAmilias = new ManejadorCargarFAmilias(_uiapp);
                    _ManejadorCargarFAmilias.cargarFamilias_run();
                    transGroup.Assimilate();
                }
                if (IsMje) Util.InfoMsg("Datos cargados correctamente");

            }
            catch (Exception ex)
            {
                 
                Util.InfoMsg($"Error al cargar parametros ex:{ex.Message}");
                return false;
            }

            return true;
        }
    }
}
