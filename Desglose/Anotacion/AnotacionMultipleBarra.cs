using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Desglose.Ayuda;
using Desglose.Familias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Desglose.BuscarTipos;
using Desglose.DImensionNh;

namespace Desglose.Anotacion
{
    public class AnotacionMultipleBarraDTO
    {
        public string nombrefamilia { get; set; }
        public XYZ Origen_ { get; set; }
        public XYZ taghead_ { get; set; }
    }
        public class AnotacionMultipleBarra
    {
        private readonly UIApplication _uiapp;
        private readonly int _dire;
        private readonly View _view;
        private Document _doc;
        private MultiReferenceAnnotationOptions opt;
        private XYZ _Origen;
        private XYZ _Taghead;
        private string _nombrefamilia;

        public AnotacionMultipleBarra(UIApplication uiapp, View _section, int  _dire)
        {
            this._uiapp = uiapp;
            this._dire = _dire;
            this._view = _section;
            this._doc = _uiapp.ActiveUIDocument.Document;
        }

        public bool CreateAnnotation(List<ElementId> listaBArras, AnotacionMultipleBarraDTO _AnotacionMultipleBarraDTO )
        {
            try
            {
                _Origen = _AnotacionMultipleBarraDTO.Origen_;
                _Taghead = _AnotacionMultipleBarraDTO.taghead_;
                _nombrefamilia=_AnotacionMultipleBarraDTO.nombrefamilia;
                if (!ObtenerMultiRef(listaBArras)) return false;

                DibujarAnnotation();

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear anotacion  EX:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool ObtenerMultiRef(List<ElementId> listaBArras)
        {

            try
            {
                //1) definir MultiReferenceAnnotationType


                MultiReferenceAnnotationType tupoanotation = null; // TiposMultiReferenceAnnotationType.obtenerDefault(_doc);

                if (_nombrefamilia == CONSTFami.NOmbre_FAMILIA_LAT)
                    tupoanotation = TiposMultiReferenceAnnotationType.M1_GetMultiReferenceAnnotationType("MultiReferenceAnnotationType_LAT", _doc);
                else if (_nombrefamilia == CONSTFami.NOmbre_Section_Diam)
                    tupoanotation = TiposMultiReferenceAnnotationType.M1_GetMultiReferenceAnnotationType("MultiReferenceAnnotationType_DIAM", _doc);
                else if (_nombrefamilia == CONSTFami.NOmbre_Section_SegunElev)
                    tupoanotation = TiposMultiReferenceAnnotationType.M1_GetMultiReferenceAnnotationType("MultiReferenceAnnotationType_SegunELEV", _doc);
                else
                    tupoanotation = TiposMultiReferenceAnnotationType.obtenerDefault(_doc);


                if (tupoanotation == null) return false;

                //2)obtener dimensio
                //DimensionType dmNh = SeleccionarDimensiones.ObtenerPrimerDimensioneTypeLinear(_doc);
                DimensionType dmNh = SeleccionarDimensiones.ObtenerDimensionTypePorNombre(_doc, "DimensionBarra"); 
                if (dmNh == null) return false;
           

                //3) obtener tag 
                Element IndependentTagPath = TiposRebarTag.M1_GetRebarTag(_nombrefamilia, _doc);
                if (IndependentTagPath == null) return false;

                try
                {
                    using (Transaction t = new Transaction(_doc, "MultiReferenceAnnotation_config"))
                    {
                        t.Start();
                        tupoanotation.DimensionStyleId = dmNh.Id; // definir un dimensiones sin flecha ni nada simple
                        tupoanotation.TagTypeId = IndependentTagPath.Id; // generar tag por ejemplo 'MRA Rebar Section'

                        tupoanotation.GroupTagHeads = true;
                        tupoanotation.ShowDimensionText = false;

                        t.Commit();
                    }

                }
                catch (Exception ex)
                {
                    Util.ErrorMsg($"Error al crear anotacion  EX:{ex.Message}");
                    return false;
                }
           
       

                //2) crear ale option
                opt = new MultiReferenceAnnotationOptions(tupoanotation);
                opt.DimensionLineDirection = _view.RightDirection;
                opt.DimensionPlaneNormal = _view.ViewDirection;

                opt.DimensionLineOrigin = _Origen;
                opt.TagHasLeader = true;
                opt.TagHeadPosition = _Taghead;
                opt.DimensionStyleType = DimensionStyleType.Linear;

                opt.SetElementsToDimension(listaBArras);

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear anotacion  EX:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool DibujarAnnotation()
        {
            try
            {
                using (Transaction t = new Transaction(_doc, "MultiReferenceAnnotation"))
                {
                    t.Start();
                    MultiReferenceAnnotation mra = MultiReferenceAnnotation.Create(_doc, _view.Id, opt);

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear anotacion  EX:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
