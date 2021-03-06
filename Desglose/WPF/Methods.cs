
using ArmaduraLosaRevit.Model.UpdateGenerar;
using Autodesk.Revit.UI;

using Desglose.Ayuda;
using Desglose.Borrar;
using Desglose.DTO;
using Desglose.Seleccionar;

namespace Desglose.WPF
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods
    {


        public static void M1_Desglose(UI_desglose _ui, UIApplication _uiapp)
        {



            UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            _updateGeneral.M3_DesCargarBarras();

            string tipoPosiicon = _ui.BotonOprimido;
  
            if (tipoPosiicon == "btnGenerar_Elev")
            {
                char ch = char.Parse(_ui.dtNombre.Text);

                if (!char.IsLetter(ch))
                {
                    Util.ErrorMsg("Lnombre de grupo debe ser un letra");
                    return;
                }


                _ui.Hide();

                Config_EspecialElev _Config_EspecialElv = _ui.ObtenerConfiguraEspecialElev();
                if (_Config_EspecialElv.TipoCasoAnalisis == CasoAnalisas.AnalsisVertical)
                {
                    ManejadorDesgloseV _ManejadorDesglose = new ManejadorDesgloseV(_uiapp, _ui);
                    _ManejadorDesglose.EjecutarDibujarBarrasEnElevacionV(_Config_EspecialElv);
                }
                else
                {
                  //  _Config_EspecialElv.DiamtroLateralMax = 
                    ManejadorDesgloseH _ManejadorDesglose = new ManejadorDesgloseH(_uiapp, _ui);
                    _ManejadorDesglose.M1_EjecutarDibujarBarrasEnElevacionHV2(_Config_EspecialElv);
                }
                _ui.Show();
            }

            else if (tipoPosiicon == "GenCorteV")
            {
                _ui.Hide();


                Config_EspecialCorte _Config_EspecialCorte = _ui.ObtenerConfiguraEspecialCOrte();

                if (_Config_EspecialCorte.TipoCasoAnalisis == CasoAnalisas.AnalsisVertical)
                {
                    
                    ManejadorDesgloseV _ManejadorDesglose = new ManejadorDesgloseV(_uiapp, _ui);
                    _ManejadorDesglose.EjecutarDibujarBarrasEncorteV(_Config_EspecialCorte);
                }
                else
                {
                    ManejadorDesgloseH _ManejadorDesglose = new ManejadorDesgloseH(_uiapp, _ui);
                    _ManejadorDesglose.M2_EjecutarDibujarBarrasEncorteH(_Config_EspecialCorte);
                }
                _ui.Show();
            }

            else if (tipoPosiicon == "Bton_Cargarconfig")
            {
                _ui.Hide();

                ManejadorConfiguracionDesglose.cargar(_uiapp, true);
                _ui.Show();
            }
            else if (tipoPosiicon == "Bton_RecargarFamilias")
            {
                _ui.Hide();
                //recargar familias
                ManejadorConfiguracionDesglose.RecargarFamilias(_uiapp, true);
                _ui.Show();
            }            
            else if (tipoPosiicon == "Bton_BorrarFamilia")
            {
                _ui.Hide();
                //recargar familias
                BorrarFamilia _BorrarFamilia = new BorrarFamilia(_uiapp);
                _BorrarFamilia.BorrarTodasLasFamilias();
                
                _ui.Show();
            }
            else if (tipoPosiicon == "Bton_BorrarConfi")
            {
                _ui.Hide();
                //recargar familias
                DefinicionBorrarManejador _DefinicionBorrarManejador = new DefinicionBorrarManejador(_uiapp);
                _DefinicionBorrarManejador.EjecutarBorrarParametros();
     
                _ui.Show();
            }
            else if (tipoPosiicon == "Bton_BorrarElev")
            {
                _ui.Hide();
                //recargar familias
                SeleccionarDetalles _SeleccionarDetalles = new SeleccionarDetalles(_uiapp);
                _SeleccionarDetalles.SeleccionarElevaciones();

                _ui.Show();
            }
            else if (tipoPosiicon == "Bton_BorrarCorte")
            {
                _ui.Hide();
                //recargar familias

                SeleccionarDetallesCorte _SeleccionarDetalles = new SeleccionarDetallesCorte(_uiapp);
                _SeleccionarDetalles.SeleccionarElevacionCorte();
                _ui.Show();
            }
            //CargarCambiarPathReinfomenConPto_Wpf

            _updateGeneral.M4_CargarGenerar();

        }






    }
}