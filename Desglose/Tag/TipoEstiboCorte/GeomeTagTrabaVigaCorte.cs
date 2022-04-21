using Desglose.Ayuda;
using Desglose.DTO;
using Desglose.Geometria;
using Desglose.Tag;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace Desglose.Tag.TipoEstiboCorte
{
    public class GeomeTagTrabaVigaCorte : GeomeTagBaseV, IGeometriaTag
    {
        private Config_EspecialCorte Config_EspecialCorte;

        public GeomeTagTrabaVigaCorte(UIApplication _uiapp, RebarElevDTO _RebarElevDTO) :
            base(_uiapp, _RebarElevDTO)
        {

            Config_EspecialCorte = _RebarElevDTO.Config_EspecialCorte;
        }


        public override void M3_DefinirRebarShape()
        {


            Traba3ladosOrientadaOrtogonal_H _EstribosRectagularesHortogonales = new Traba3ladosOrientadaOrtogonal_H(_rebarElevDTO);
            if (_EstribosRectagularesHortogonales.calcularUbiaciontexto())
            {
                //  double Zrefe = CentroBarra.Z;
                CentroBarra = _EstribosRectagularesHortogonales.UbicacionDeF;//.AsignarZ(Zrefe);

                string familiaF = "_F_normal";
                if (Config_EspecialCorte.TipoCOnfigCuantia == TipoCOnfCuantia.SegunPlano)
                    familiaF = "_F_segun";

                TagP0_F = M1_1_ObtenerTAgBarra(CentroBarra, "FCorte", nombreDefamiliaBase + familiaF , escala);
                listaTag.Add(TagP0_F);

                //largo
                LBarra = _EstribosRectagularesHortogonales.UbicacionDeL;//.AsignarZ(Zrefe);
                string familiaL = "_L_normal";
                if (Config_EspecialCorte.TipoCOnfigLargo == TipoCOnfLargo.Aprox5)
                    familiaL = "_L_normal";//_L_5aprox_
                else if (Config_EspecialCorte.TipoCOnfigLargo == TipoCOnfLargo.Aprox10)
                    familiaL = "_L_normal";//_L_10aprox_
                TagP0_L = M1_1_ObtenerTAgBarra(LBarra, "LCorte", nombreDefamiliaBase + familiaL , escala);
                listaTag.Add(TagP0_L);

                //parte
                XYZ textoSup = _EstribosRectagularesHortogonales.UbicacionSup;
                TagP0_ancho_ = M1_1_ObtenerTAgBarra(textoSup, "Ancho", nombreDefamiliaBase + "_F_normal" , escala);
                TagP0_ancho_.valorTag = _EstribosRectagularesHortogonales.UbicacionSup_ValorLArgo;
                listaTag.Add(TagP0_ancho_);
            }
            AsignarPArametros(this);
        }

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagTrabaVigaCorte> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBaseV _geomeTagBase)
        {

        }
    }
}
