#region Namespaces

using System;
using Autodesk.Revit.UI;
using Desglose;
using Desglose.Ayuda;

#endregion


namespace Desglose.WPF
{

    public class ManejadorWPFDesglose 
    {


        // ModelessForm instance
        private UI_desglose _mMyForm;
        private UIApplication _UIapp;
        private string _caso;

        public ManejadorWPFDesglose(UIApplication _uiapp, string caso)
        {
            _UIapp = _uiapp;
            _caso = caso;

        }

        public  Result Execute()
        {
            try
            {
                ShowForm(_UIapp);
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return Result.Failed;
            }
        }

        public void ShowForm(UIApplication uiapp)
        {
            // If we do not have a dialog yet, create and show it
            if (_mMyForm != null && _mMyForm == null) return;
            //EXTERNAL EVENTS WITH ARGUMENTS
            EventHandlerWithStringArg evStr = new EventHandlerWithStringArg();
            EventHandlerWithWpfArg evWpf = new EventHandlerWithWpfArg();

            // The dialog becomes the owner responsible for disposing the objects given to it.
            _mMyForm = new UI_desglose(_UIapp, evStr, evWpf, _caso);
           // _mMyForm.dtTipo.SelectedItem = TipoBarraTraslapoDereArriba.f1;
            _mMyForm.Show();
        }

    }
}