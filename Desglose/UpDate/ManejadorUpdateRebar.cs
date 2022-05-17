using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Desglose.Ayuda;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.UpDate
{
    public class ManejadorUpdateRebar
    {
        private UIApplication _uiapp;
        private Document _doc;

        public ManejadorUpdateRebar(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
        }

        public void CargarUpdateREbar()
        {
            UpdaterBarrasRebar updateopen = new UpdaterBarrasRebar(_doc,_uiapp.ActiveAddInId);
            try
            {
                if (!UpdaterRegistry.IsUpdaterRegistered(updateopen.GetUpdaterId()))
                {
                    UpdaterRegistry.RegisterUpdater(updateopen, _doc, true);//PREGUNTAR AL USUARIO: TRUE ES OPCIONAL. FALSE ES OBLIGATORIO.

                    #region ELEMENTOS DISPARADORES
                    //BuiltInCategory.OST_IOSDetailGroups
                    ElementCategoryFilter filtromuros = new ElementCategoryFilter(BuiltInCategory.OST_Rebar);
                    UpdaterRegistry.AddTrigger(updateopen.GetUpdaterId(), filtromuros, Element.GetChangeTypeGeometry());
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error Descargar Reactor 'UpdateRebar' ex:" + ex.Message);
            }
        }


        public void DesCargarUpdateREbar()
        {
            UpdaterBarrasRebar updateopen = new UpdaterBarrasRebar(_doc, _uiapp.ActiveAddInId);

            try
            {
                if (UpdaterRegistry.IsUpdaterRegistered(updateopen.GetUpdaterId()))
                {
                    UpdaterRegistry.UnregisterUpdater(updateopen.GetUpdaterId());
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error Descargar Reactor 'UpdateRebar' ex:" + ex.Message );
            }
        }
    }
}
