using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Desglose.Ayuda;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Desglose.BuscarTipos;
using Desglose.DImensionNh;

namespace Desglose.Anotacion
{
    public class AnotacionMultipleBarra
    {
        private readonly UIApplication _uiapp;
        private readonly View _view;
        private Document _doc;
        private MultiReferenceAnnotationOptions opt;
        private XYZ Origen;
        private XYZ Taghead;

        public AnotacionMultipleBarra(UIApplication uiapp, View _view)
        {
            this._uiapp = uiapp;
            this._view = _view;
            this._doc = _uiapp.ActiveUIDocument.Document;
        }

        public bool CreateAnnotation(List<ElementId> listaBArras, XYZ Origen_, XYZ taghead_)
        {
            try
            {
                Origen = Origen_;
                Taghead = taghead_;
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
                MultiReferenceAnnotationType tupoanotation = TiposMultiReferenceAnnotationType.obtenerDefault(_doc);
                if (tupoanotation == null) return false;

                //2)obtener dimensio
                DimensionType dmNh = SeleccionarDimensiones.ObtenerPrimerDimensioneTypeLinear(_doc);
                if (dmNh == null) return false;
           

                //3) obtener tag 
                Element IndependentTagPath = TiposRebarTag.M1_GetRebarTag("MRA Rebar Section", _doc);
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

                opt.DimensionLineOrigin = Origen;
                opt.TagHasLeader = true;
                opt.TagHeadPosition = Taghead;
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
