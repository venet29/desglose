using Desglose.Ayuda;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using Desglose.ParametrosShare;
using Desglose.Familias;
using Desglose.BuscarTipos;
using Desglose.DImensionNh;
using Desglose.UTILES;

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

                    //confign texto 
                    CrearTexNote _CrearTexNote = new CrearTexNote(_uiapp, "2.5mm Arial Narrow", TipoCOloresTexto.negro);
                    _CrearTexNote.M2_CrearTipoText_ConTrasn();

                    //    
                    CrearTipoDimension _CrearTipoDimension = new CrearTipoDimension(_uiapp, "DimensionBarra");
                    _CrearTipoDimension.CrearTipoDimensionConTrasn_concirculo();
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


        //2)
        public static bool RecargarFamilias(UIApplication _uiapp, bool IsMje = true)
        {
            if (_uiapp == null) return false;
            Document _doc = _uiapp.ActiveUIDocument.Document;

            

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

                    // familias
                    ManejadorCargarFAmilias _ManejadorCargarFAmilias = new ManejadorCargarFAmilias(_uiapp,true);
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

        //3)
        public static bool BorrarFamilias(UIApplication _uiapp, bool IsMje = true)
        {
            if (_uiapp == null) return false;
            Document _doc = _uiapp.ActiveUIDocument.Document;





            try
            {
                using (TransactionGroup transGroup = new TransactionGroup(_doc))
                {
                    transGroup.Start("Inicio ConfiguracionInicialGeneral-NH");

                    // familias
                    ManejadorCargarFAmilias _ManejadorCargarFAmilias = new ManejadorCargarFAmilias(_uiapp, true);
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


        //4)
        public static bool BorrarConfiguracion(UIApplication _uiapp, bool IsMje = true)
        {
            if (_uiapp == null) return false;
            Document _doc = _uiapp.ActiveUIDocument.Document;





            try
            {
                using (TransactionGroup transGroup = new TransactionGroup(_doc))
                {
                    transGroup.Start("Inicio ConfiguracionInicialGeneral-NH");

                    // familias
                    ManejadorCargarFAmilias _ManejadorCargarFAmilias = new ManejadorCargarFAmilias(_uiapp, true);
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
