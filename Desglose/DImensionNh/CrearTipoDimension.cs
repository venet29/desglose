using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Desglose.Ayuda;
using System;

namespace Desglose.DImensionNh
{
    public class CrearTipoDimension
    {
        // ExternalCommandData commandData;
        UIDocument uidoc;
        Document _doc;
        TextNoteType txtNoteType;
        TextNote txNote;
        private UIApplication _uipp;
        private string nombreFamilyaRef;
        private string nameTipoTexto;

        private int color;


        public CrearTipoDimension(UIApplication uipp, string nameTipoTexto)
        {
            uidoc = uipp.ActiveUIDocument;
            _doc = uipp.ActiveUIDocument.Document;

            //obtienen el tipo texto
            this.nameTipoTexto = nameTipoTexto;//"DimensionRebar"
            this._uipp = uipp;
            this.nombreFamilyaRef = "Arrow Filled - 2.5mm Arial";
        }



        /// <summary>
        /// obtiene el tipo de texto segun el nombre
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        /// 


        //obs1)
        public bool CrearTipoDimensionConTrasn()
        {
            try
            {
                DimensionType _dimensionTypedefault  = SeleccionarDimensiones.ObtenerDimensionTypePorNombre(_doc, nombreFamilyaRef);

                if (_dimensionTypedefault == null)
                {
                    Util.ErrorMsg($"Error al crear Dimension");
                    return false;
                }

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Crear TipoDimension-NH");


                    Element newElem = _dimensionTypedefault.Duplicate(nameTipoTexto);

                    DimensionType newDimensionType = newElem as DimensionType;

                    if (null != newDimensionType)
                    {

                        newDimensionType.get_Parameter(BuiltInParameter.TEXT_SIZE).Set(0);
                    
                    }

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }




        public bool CrearTipoDimensionConTrasn_concirculo()
        {
            try
            {
                DimensionType _dimensionTypedefault = SeleccionarDimensiones.ObtenerPrimerDimensioneTypeLinear_Arial(_doc);

                if (_dimensionTypedefault == null)
                {
                    Util.ErrorMsg($"Error al crear Dimension. No se encontro familia '{nombreFamilyaRef}' de referiencia");
                    return false;
                }

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Crear TipoTextNote-NH");


                    Element newElem = _dimensionTypedefault.Duplicate(nameTipoTexto);

                    DimensionType newDimensionType = newElem as DimensionType;

                    if (null != newDimensionType)
                    {

                        newDimensionType.get_Parameter(BuiltInParameter.WITNS_LINE_EXTENSION).Set(0);
                        newDimensionType.get_Parameter(BuiltInParameter.DIM_LEADER_ARROWHEAD).Set(new ElementId(-1));
                        newDimensionType.get_Parameter(BuiltInParameter.WITNS_LINE_TICK_MARK).Set(new ElementId(-1));

                    }

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }


    }
}
