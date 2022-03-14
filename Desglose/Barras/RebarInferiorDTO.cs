using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Desglose.Ayuda;
using Desglose.enumNh;
using System.Collections.Generic;

namespace Desglose.Barras
{
    //ver obs 1
    public class RebarInferiorDTO
    {

        public UIApplication uiapp { get; set; }
        public Element floor { get; set; }

      

        public List<XYZ> listaPtosPerimetroBarras { get; set; }
        public XYZ PtoDirectriz1 { get; set; }
        public XYZ PtoDirectriz2 { get; set; }
        public XYZ barraIni { get; set; }
        public XYZ barraFin { get; set; }

        public XYZ ptoSeleccionMouse { get; set; }


        public double diametroFoot { get; set; }


        private int _diametroMM; // field
        public int diametroMM   // property
        {
            get { return _diametroMM; }   // get method
            set
            {
                _diametroMM = value;
                diametroFoot = Util.MmToFoot(_diametroMM);
            }  // set method
        }

        public int largorecorrido { get; set; }

        public double espaciamientoFoot { get; set; }
        public int cantidadBarras { get; set; }
        public double espesorLosaFoot { get; set; }

        public double espesorBarraEnLOsaFooT { get; set; }

        public UbicacionLosa ubicacionLosa { get; set; }
        public TipoBarra tipoBarra { get; set; }
        public int numeroBarra { get; set; }
        public double largo_recorridoFoot { get; set; }
        public double anguloBarraGrados { get; set; }
        public double anguloBarraRad { get; set; }
        public double anguloTramoRad { get; set; }
        public double LargoPata { get; set; }
        public double largomin_1 { get; internal set; }
        public int AcortamientoEspesorSecundario { get; set; }
        public bool IsOK { get; internal set; }
        public double LargoPataF4 { get; set; }
        public TipoDireccionBarra TipoDireccionBarra_ { get; set; }
        public RebarInferiorDTO(UIApplication uiapp)
        {
            this.uiapp = uiapp;
            listaPtosPerimetroBarras = new List<XYZ>();
            IsOK = false;
        }


     
    }
}
