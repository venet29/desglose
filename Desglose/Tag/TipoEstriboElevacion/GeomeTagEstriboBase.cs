
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Desglose.Ayuda;
using Desglose.BuscarTipos;
using Desglose.DTO;
using Desglose.Extension;
using System;
using System.Collections.Generic;

namespace Desglose.Tag.TipoEstriboElevacion
{
    //public interface IGeometriaTag
    //{
    //    List<TagBarra> listaTag { get; set; }
    //    void CAlcularPtosDeTAg(bool IsGarficarEnForm=false);
    //}
    public class GeomeTagEstriboBase
    {


        protected readonly XYZ CentroBarra;
        private readonly UIApplication uiapp;
        protected readonly RebarElevDTO rebarElevDTO;
        protected XYZ _posiciontag;


        //  protected double _anguloBarraRad;
        protected int _anguloBArraGrado;
        protected string _signoAngulo;
        protected double _largoMedioEnFoot;
        Document _doc;

        protected int escala;
        protected int escala_realview;
        protected string nombreDefamiliaBase;

        protected int desplazaEESTRIBO = 10;
        protected int desplazaLATERAL = 15;
        protected int desplazaTRABA = 20;
        //lista con objetos que representan los tag de la barra
        public List<TagBarra> listaTag { get; set; }

        public TagBarra TagP0_Traba { get; set; }
        public TagBarra TagP0_Lateral { get; set; }
        public TagBarra TagP0_Estribo { get; set; }

        public GeomeTagEstriboBase(UIApplication _uiapp, RebarElevDTO _RebarElevDTO)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
            uiapp = _uiapp;
            rebarElevDTO = _RebarElevDTO;
            this._posiciontag = (_RebarElevDTO.ptoini + _RebarElevDTO.ptofinal) / 2;         
            //this.CentroBarra = (ptoIni + new XYZ(0, 0, Util.CmToFoot(150)));
            this.CentroBarra = _posiciontag;// (ptoFin - new XYZ(0, 0, Util.CmToFoot(50)));
                                            //this._view = _doc.ActiveView;
                                            // this.escala = ConstantesGenerales.CONST_ESCALA_BASE;// _view.Scale;
            this.escala = 50;// _doc.ActiveView.Scale;
            this.escala_realview = _doc.ActiveView.Scale;

            //dos opciones  "M_Path Reinforcement Tag(ID_cuantia_largo)"
            this.nombreDefamiliaBase = "MRA Rebar";
            this.listaTag = new List<TagBarra>();
        }

        public void M1_ObtnerPtosInicialYFinalDeBarra(double anguloRoomRad)
        {


        }

        //obs4
        public void M2_CAlcularPtosDeTAg(bool IsGraficarEnForm = false)
        {

            var confEstr = rebarElevDTO.Config_DatosEstriboElevVigas;
            confEstr.ObtenerDesplazamientos(escala_realview);
      
            XYZ p0_Estribo = CentroBarra + new XYZ(0, 0, Util.CmToFoot(confEstr.desplazaEESTRIBO));
            TagP0_Estribo = M1_1_ObtenerTAgBarra(p0_Estribo, "ESTRIBO", nombreDefamiliaBase + " EV");
            listaTag.Add(TagP0_Estribo);

            XYZ p0_Lateral = CentroBarra + new XYZ(0, 0, Util.CmToFoot(confEstr.desplazaLATERAL));
            TagP0_Lateral = M1_1_ObtenerTAgBarra(p0_Lateral, "LATERAL", nombreDefamiliaBase + " EVL");
            listaTag.Add(TagP0_Lateral);

            XYZ p0_Trabas = CentroBarra + new XYZ(0, 0, Util.CmToFoot(confEstr.desplazaTRABA));
            TagP0_Traba = M1_1_ObtenerTAgBarra(p0_Trabas, "TRABA", nombreDefamiliaBase + " EVT");
            listaTag.Add(TagP0_Traba);


        }

     
        protected TagBarra M1_1_ObtenerTAgBarra(XYZ posicion, string nombreLetra, string NombreFamilia)
        {
            //caso sin giraR
            Element IndependentTagPath = TiposRebarTag.M1_GetRebarTag(NombreFamilia, _doc);

            if (IndependentTagPath == null) { Util.ErrorMsg($"NO se puedo encontrar  familia de letra del tag de barra :{nombreLetra}"); }

            TagBarra newTagBarra = new TagBarra(posicion, nombreLetra, NombreFamilia, IndependentTagPath);
            return newTagBarra;
        }


        protected TagBarra AgregaroEditaPosicionTAgLitsta(string nombre, XYZ p0_XXX_)
        {
            listaTag.RemoveAll(c => c.nombre == nombre);
            TagBarra TagP0_XXX = M1_1_ObtenerTAgBarra(p0_XXX_, nombre, $"{nombreDefamiliaBase}");
            listaTag.Add(TagP0_XXX);
            return TagP0_XXX;
        }
    }

}

