using Desglose.Ayuda;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using Desglose.ParametrosShare;
using Desglose.Familias;
using Desglose.BuscarTipos;

namespace Desglose
{
    public class ManejadorConfiguracionDesglose
    {

        public static bool cargar(UIApplication _uiapp, bool IsMje = true)
        {
            if (_uiapp == null) return false;
            Document _doc = _uiapp.ActiveUIDocument.Document;
            View _view = _doc.ActiveView;

            ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
            bool resultadoConexion = _ManejadorUsuarios.PostBitacora("CARGAR ManejadorConfiguracionDesglose");

            if (!resultadoConexion)
            {
                Util.ErrorMsg("Error al validad credencial (No conexion)");
                return false;
            }
            else if (!_ManejadorUsuarios.resultnh.Isok)
            {
                Util.ErrorMsg("Error al validar Usuario");
                return false;
            }


            try
            {
                using (TransactionGroup transGroup = new TransactionGroup(_doc))
                {
                    transGroup.Start("Inicio ConfiguracionInicialGeneral-NH");

                   // DefinirArchivoShare.Ejecutar(_uiapp);

                   //1-parametros compartidos
                    ConfiguracionInicialParametros configuracionInicial = new ConfiguracionInicialParametros(_uiapp);
                    configuracionInicial.AgregarParametrosShareDesglose();

                    //    // crear arrow
                    Tipos_Arrow.CrearArropwIniciales(_uiapp.ActiveUIDocument.Document);


                    TiposMultiReferenceAnnotationType.CrearMultiReferenceAnnotationTypeIniciales(_doc);

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
