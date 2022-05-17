
using Autodesk.Revit.UI;
using Desglose.UpDate;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UpdateGenerar
{
    public class UpdateGeneral
    {
        private readonly UIApplication _Uiapp;
        private ManejadorUpdateRebar _manejadorUpdateRebar;


        public UpdateGeneral(UIApplication _uiapp)
        {
            _Uiapp = _uiapp;
            _manejadorUpdateRebar = new ManejadorUpdateRebar(_uiapp);

        }



        public bool M1_ConfiguracionAlCArgarREvit()
        {
            try
            {
                _manejadorUpdateRebar.CargarUpdateREbar();

                Debug.WriteLine("-->ConfiguracionAlCArgarREvit finalizado");
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool M2_CargarBArras()
        {
            try
            {
                _manejadorUpdateRebar.CargarUpdateREbar();               
                
                Debug.WriteLine("-->CargarBArras finalizado");
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool M3_DesCargarBarras()
        {
            try
            {
                _manejadorUpdateRebar.DesCargarUpdateREbar();        

                Debug.WriteLine("-->DesCargarBarras finalizado");
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }


        //generales
        public bool M4_CargarGenerar()
        {
            try
            {
                _manejadorUpdateRebar.CargarUpdateREbar();
                ///  _manejadorNombreVistaEnBarras.CargarUpdateView();
                Debug.WriteLine("-->CargarGenerar finalizado");
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        public bool M5_DesCargarGenerar()
        {

            try
            {
                _manejadorUpdateRebar.DesCargarUpdateREbar();           

                Debug.WriteLine("-->DesCargarGenerar finalizado");
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

    }
}
