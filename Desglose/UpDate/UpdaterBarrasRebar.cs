

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Desglose.Ayuda;
using Desglose.UpDate.Casos;
using Desglose.UTILES.ParaBarras;
using System;
using System.Diagnostics;

namespace Desglose.UpDate
{
    public class UpdaterBarrasRebar : IUpdater
    {
        static AddInId _appId;
        static UpdaterId _updaterId;
        private Document _doc;
        private UpdaterBarrasRebar _updateopen;
        public static bool IsMjs { get; set; } = true;
        public UpdaterBarrasRebar(Document doc, AddInId id)//codigo interno del Updater 145689
        {
            _appId = id;
            _doc = doc;
            _updaterId = new UpdaterId(_appId, new Guid("afe1cab7-f37d-4acc-931b-653a3bdbddeb"));//CAMBIAR CODIGO EN CADA UPDATER NUEVO

        }
        public void Execute(UpdaterData data)
        {
            _doc = data.GetDocument();
            Stopwatch timeMeasure1 = Stopwatch.StartNew();
            // _updateopen = new UpdaterBarrasRebar(_doc.Application.ActiveAddInId);

            //Descargar();
            // RemoveAllTriggers  : no se puede remover disparadores durante le jecucion de un update
            //UnregisterUpdater : no se puede desactiuvar IUpdater dentro del mismo 
            foreach (ElementId id in data.GetAddedElementIds())
            {
                //	Wall muro = doc.GetElement(id) as Wall;

            }
            foreach (ElementId id in data.GetModifiedElementIds())
            {
                Rebar _rebar = _doc.GetElement(id) as Rebar;
                if (_rebar == null) continue;

                ObtenerTipoBarra _newObtenerTipoBarra = new ObtenerTipoBarra(_rebar);
                if (!_newObtenerTipoBarra.EjecutarFALSO()) return;

                if (_newObtenerTipoBarra.TipoBarraGeneral == TipoBarraGeneral.Elevacion || _newObtenerTipoBarra.TipoBarraGeneral == TipoBarraGeneral.Losa)
                {
                    UpdateRebarElevaciones _newUpdateRebarElevaciones = new UpdateRebarElevaciones(_doc, _rebar, _newObtenerTipoBarra.TipoBarra_);
                    _newUpdateRebarElevaciones.Ejecutar();
                }
  
                else
                {

                    if (IsMjs)
                    {

                        var result = Util.ErrorMsgConDesctivar($"Barras  id{_rebar.Id.IntegerValue} sin 'TipoBarraGeneral'. No se puede actualizar ");
                        if (result == System.Windows.Forms.DialogResult.Cancel)
                        {
                            IsMjs = false;
                        }
                    }

                }

            }
            Debug.WriteLine($"   VerificarDatos : {timeMeasure1.ElapsedMilliseconds } ms");
        }

       

        //#endregion


        public string GetUpdaterName()
        {
            return "UPDATER REBAR";
        }

        public string GetAdditionalInformation()
        {
            return "Modificar texto rebar";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.Rebar;
        }

        public UpdaterId GetUpdaterId()
        {
            return _updaterId;
        }
    }
}
