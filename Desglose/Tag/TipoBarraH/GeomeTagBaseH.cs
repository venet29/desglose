using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Desglose.Ayuda;
using Desglose.BuscarTipos;
using Desglose.Creador;
using Desglose.DTO;
using System;
using System.Collections.Generic;

namespace Desglose.Tag.TipoBarraH
{
    //public interface IGeometriaTag
    //{
    //    List<TagBarra> listaTag { get; set; }
    //    void CAlcularPtosDeTAg(bool IsGarficarEnForm=false);
    //}
    public class GeomeTagBaseH
    {

        // private readonly List<XYZ> _listaPtosPerimetroBarras;

        protected readonly XYZ CentroBarra;
        private readonly UIApplication _uiapp;
        protected RebarElevDTO _rebarElevDTO;
        private View _view;

        //pto inical y final de barra( linea inferior)
        protected XYZ _p1;
        protected XYZ _p2;
        protected XYZ _posiciontag;
        protected XYZ _direccionBarra;

        protected XYZ _DesfaseLInea;
        protected XYZ _DesfaseLInea_F;

        protected double _anguloBarraRad;
        protected int _anguloBArraGrado;
        protected string _signoAngulo;
        protected double _largoMedioEnFoot;
        Document _doc;
        //  private RebarInferiorDTO _rebarInferiorDTO1;
        protected int escala;
        protected string nombreDefamiliaBase;

        protected bool IsTagCompletoENLinea;
        //lista con objetos que representan los tag de la barra
        public List<TagBarra> listaTag { get; set; }
        //public TagBarra TagP0_A { get; set; }
        //public TagBarra TagP0_B { get; set; }
        public TagBarra TagP0_C { get; set; }
        public TagBarra TagP0_F { get; set; }
        public TagBarra TagP0_L { get; set; }

        public GeomeTagBaseH() { }
        public GeomeTagBaseH(UIApplication _uiapp, RebarElevDTO _RebarElevDTO)
        {
            this._uiapp = _uiapp;
            _rebarElevDTO = _RebarElevDTO;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._p1 = _RebarElevDTO.ptoini;// ptoIni;
            this._p2 = _RebarElevDTO.ptofinal;

            _posiciontag = (_p1 + _p2) / 2;




            this.CentroBarra = _posiciontag;// (ptoFin - new XYZ(0, 0, Util.CmToFoot(50)));
            this.CentroBarra = (_p1 + _p2) / 2;
            this._view = _doc.ActiveView;
            this.escala = ConstNH.CONST_ESCALA_BASE;// _view.Scale;
            this.escala = 50;// _view.Scale;
            //dos opciones  "M_Path Reinforcement Tag(ID_cuantia_largo)"
            this.nombreDefamiliaBase = "MRA Rebar";

        }

        public virtual void M1_ObtnerPtosInicialYFinalDeBarra(double anguloRoomRad)
        {
            _direccionBarra = (_p2 - _p1).Normalize();
            if (_p1.DistanceTo(_p2) < 10)
            {
                XYZ ptomedio = (_p1 + _p2) / 2;
                _p1 = ptomedio - _direccionBarra * 5;
                _p2 = ptomedio + _direccionBarra * 5;
            }


            _anguloBarraRad = Util.angulo_entre_ptRadXY0(_p1, _p2);
            _largoMedioEnFoot = _p1.DistanceTo(_p2);

            listaTag = new List<TagBarra>();
            _DesfaseLInea = new XYZ(0, 0, 0);
            if (_rebarElevDTO._rebarDesglose.TipobarraH_.ToString().Contains("INF"))
            {
                _DesfaseLInea = new XYZ(0, 0, -0.8);
            }
            else
                _DesfaseLInea = new XYZ(0, 0, 0.5);

            _DesfaseLInea_F = -_direccionBarra * 1; //new XYZ(0,.0,0);

            switch (_rebarElevDTO._rebarDesglose.TipobarraH_)
            {
                case Ayuda.TipobarraH.Lateral:
                    break;
                case Ayuda.TipobarraH.Linea1SUP:
                    _DesfaseLInea_F = new XYZ(0, .0, 0);
                    break;
                case Ayuda.TipobarraH.Linea2SUP:
                    _DesfaseLInea = _DesfaseLInea + ConstNH.CONST_DesfaseLine;
                    break;
                case Ayuda.TipobarraH.Linea3SUP:
                    _DesfaseLInea = _DesfaseLInea + ConstNH.CONST_DesfaseLine * 2;
                    break;
                case Ayuda.TipobarraH.Linea4SUP:
                    _DesfaseLInea = _DesfaseLInea + ConstNH.CONST_DesfaseLine * 3;
                    break;
                case Ayuda.TipobarraH.Linea5SUP:
                    break;
                case Ayuda.TipobarraH.Linea1INF:
                    _DesfaseLInea_F = new XYZ(0, .0, 0);
                    break;
                case Ayuda.TipobarraH.Linea2INF:
                    _DesfaseLInea = _DesfaseLInea + -ConstNH.CONST_DesfaseLine;
                    break;
                case Ayuda.TipobarraH.Linea3INF:
                    _DesfaseLInea = _DesfaseLInea + -ConstNH.CONST_DesfaseLine * 2;
                    break;
                case Ayuda.TipobarraH.Linea4INF:
                    _DesfaseLInea = _DesfaseLInea + -ConstNH.CONST_DesfaseLine * 3;
                    break;
                case Ayuda.TipobarraH.Linea5INF:
                    break;
                case Ayuda.TipobarraH.NONE:
                    break;
                case Ayuda.TipobarraH.LineaNOLateral:
                    break;
                default:
                    break;
            }




        }

        #region met2
        //obs4
        public void M2_CAlcularPtosDeTAg(bool IsGraficarEnForm = false)
        {

            if (IsTagCompletoENLinea)
            {
                XYZ p0_FTC = _p1 + _DesfaseLInea + _direccionBarra * _largoMedioEnFoot * 0.25;
                p0_FTC = CentroBarra + _DesfaseLInea + _DesfaseLInea_F - _direccionBarra * Util.CmToFoot(80);
                TagP0_F = M2_1_ObtenerTAgBarra(p0_FTC, "FBCompleto", nombreDefamiliaBase + " FBarraCompleto", escala);// 2@12
                listaTag.Add(TagP0_F);
            }
            else
            {

                XYZ p0_F = _p1 + _DesfaseLInea + _direccionBarra * _largoMedioEnFoot * 0.25;
                p0_F = CentroBarra + _DesfaseLInea + _DesfaseLInea_F - _direccionBarra * Util.CmToFoot(80);
                TagP0_F = M2_1_ObtenerTAgBarra(p0_F, "FB", nombreDefamiliaBase + " FBarra", escala);// 2@12
                listaTag.Add(TagP0_F);

                XYZ p0_L = CentroBarra + _DesfaseLInea;

                TagP0_L = M2_1_ObtenerTAgBarra(p0_L, "LT", nombreDefamiliaBase + " LT", escala);// L=340
                listaTag.Add(TagP0_L);

                XYZ p0_C = _p2 + _DesfaseLInea - _direccionBarra * _largoMedioEnFoot * 0.25;
                p0_C = CentroBarra + _DesfaseLInea + _direccionBarra * Util.CmToFoot(70);
                TagP0_C = M2_1_ObtenerTAgBarra(p0_C, "LP", nombreDefamiliaBase + " LP", escala); // (20+300+20)
                listaTag.Add(TagP0_C);

            }


        }

        protected TagBarra M2_1_ObtenerTAgBarra(XYZ posicion, string nombreLetra, string NombreFamilia, int escala)
        {
            //caso sin giraR
            Element IndependentTagPath = TiposRebarTag.M1_GetRebarTag(NombreFamilia, _doc);

            //si no la cuentr lCRE
            if (IndependentTagPath == null)
            {
                IndependentTagPath = M2_1_1_ObtenertTagGirado(nombreLetra, NombreFamilia, escala);
            }

            if (IndependentTagPath == null) { Util.ErrorMsg($"NO se puedo encontrar  familia de letra del tag de barra :{nombreLetra}"); }

            TagBarra newTagBarra = new TagBarra(posicion, nombreLetra, NombreFamilia, IndependentTagPath);
            return newTagBarra;

        }

        private Element M2_1_1_ObtenertTagGirado(string nombreLetra, string NombreFamilia, int escala)
        {
            Element IndependentTagPath;
            string NombreFAmiliaGenerico = nombreDefamiliaBase + "_" + nombreLetra + nombreLetra + "_";
            NombreFAmiliaGenerico = $"{nombreDefamiliaBase}_{nombreLetra}{nombreLetra}_";
            CrearFamilySymbolTagRein tiposTagLetrasBarra = new CrearFamilySymbolTagRein(_doc);
            IndependentTagPath = tiposTagLetrasBarra.ObtenerLetraGirada(NombreFamilia, NombreFAmiliaGenerico, _anguloBArraGrado, escala);
            return IndependentTagPath;
        }

        #endregion

        protected TagBarra AgregarTAgLitsta(string nombre, int coorX, int coorY, XYZ ptoReferencia)
        {
            listaTag.RemoveAll(c => c.nombre == nombre);
            XYZ p0_XXX_ = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoReferencia, _anguloBarraRad, Util.CmToFoot(coorX), Util.CmToFoot(coorY));
            TagBarra TagP0_XXX = M2_1_ObtenerTAgBarra(p0_XXX_, nombre, $"{nombreDefamiliaBase}_{nombre}_{escala}_{ _signoAngulo + Math.Abs(_anguloBArraGrado).ToString()}", escala);
            listaTag.Add(TagP0_XXX);
            return TagP0_XXX;

        }


    }

}

