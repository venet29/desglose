using Desglose.Ayuda;
using Desglose.DTO;
using Desglose.Geometria;
using Desglose.Tag;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace Desglose.Tag.TipoEstiboCorte
{
    public class GeomeTagEstriboVigaCorte_H : GeomeTagBaseV, IGeometriaTag
    {
        private Config_EspecialCorte Config_EspecialCorte_;

        public GeomeTagEstriboVigaCorte_H(UIApplication _uiapp, RebarElevDTO _RebarElevDTO) :
            base( _uiapp,  _RebarElevDTO)
        {
            Config_EspecialCorte_ = _RebarElevDTO.Config_EspecialCorte;
        }


        public override void M3_DefinirRebarShape()
        {

            EstribosRectagularesOrtogonales_H _EstribosRectagularesHortogonales = new EstribosRectagularesOrtogonales_H(_rebarElevDTO);
            if (_EstribosRectagularesHortogonales.calcularUbiaciontexto())
            {
                //double Zrefe = CentroBarra.Z;
                CentroBarra = _EstribosRectagularesHortogonales.UbicacionDeF;//.AsignarZ(Zrefe);

                string familiaF = "_F_normal";
                if (Config_EspecialCorte_.TipoCOnfigCuantia == TipoCOnfCuantia.SegunPlano)
                    familiaF = "_F_segun";

                TagP0_F = M1_1_ObtenerTAgBarra(CentroBarra, "FCorte", nombreDefamiliaBase + familiaF, escala);
                listaTag.Add(TagP0_F);


                //largo
                LBarra = _EstribosRectagularesHortogonales.UbicacionDeL;//.AsignarZ(Zrefe);
                string familiaL = "_L_normal";
               if (Config_EspecialCorte_.TipoCOnfigLargo == TipoCOnfLargo.Aprox5)
                    familiaL = "_L_5aprox";
                else if (Config_EspecialCorte_.TipoCOnfigLargo == TipoCOnfLargo.Aprox10)
                    familiaL = "_L_10aprox";
                TagP0_L = M1_1_ObtenerTAgBarra(LBarra, "LCorte", nombreDefamiliaBase + familiaL, escala);
                listaTag.Add(TagP0_L);

                if (true)
                {
                    //1
                    XYZ p0_sup = _EstribosRectagularesHortogonales.UbicacionSup;//.AsignarZ(Zrefe);
                    TagP0_ancho_ = M1_1_ObtenerTAgBarra(p0_sup, "_LargoAncho", nombreDefamiliaBase + " LAncho", escala);//uso '_F_normal_' solo par acargar tl tag
                    //TagP0_ancho_ = M1_1_ObtenerTAgBarra(p0_sup, "Ancho", nombreDefamiliaBase + "_F_normal", escala);
                    TagP0_ancho_.valorTag = _EstribosRectagularesHortogonales.UbicacionSup_ValorLArgo;
                    // ParameterUtil.SetParaStringNH(_rebarElevDTO._rebarDesglose._rebar, "LargoAncho", _EstribosRectagularesHortogonales.UbicacionSup_ValorLArgo.ToString());
                    ParametroShareNhDTO _newParaMe = new ParametroShareNhDTO()
                    {
                        Isok = true,
                        NombrePAra = "LargoAncho",
                        valor = _EstribosRectagularesHortogonales.UbicacionSup_ValorLArgo
                    };
                  // Config_EspecialCorte_.ListaPAraShare.Add(_newParaMe);
                    _rebarElevDTO.listaPArametroSharenh.Add(_newParaMe);
                    listaTag.Add(TagP0_ancho_);

                    //2
                    XYZ p0_izq = _EstribosRectagularesHortogonales.UbicacionIZq;//;.AsignarZ(Zrefe);
                    TagP0_Prof_ = M1_1_ObtenerTAgBarra(p0_izq, "_LargoAlto", nombreDefamiliaBase + " LAlto", escala);//uso '_F_normal_' solo par acargar tl tag
                    //TagP0_Prof_ = M1_1_ObtenerTAgBarra(p0_izq, "Ancho", nombreDefamiliaBase + "_F_normal", escala);//uso '_F_normal_' solo par acargar tl tag
                    TagP0_Prof_.valorTag = _EstribosRectagularesHortogonales.UbicacionIZq_ValorLArgo;
                    //ParameterUtil.SetParaStringNH(_rebarElevDTO._rebarDesglose._rebar, "LargoAlto", _EstribosRectagularesHortogonales.UbicacionIZq_ValorLArgo.ToString());
                    ParametroShareNhDTO _newParaMe2 = new ParametroShareNhDTO()
                    {
                        Isok = true,
                        NombrePAra = "LargoAlto",
                        valor = _EstribosRectagularesHortogonales.UbicacionIZq_ValorLArgo
                    };
                    //Config_EspecialCorte_.ListaPAraShare.Add(_newParaMe2);
                    _rebarElevDTO.listaPArametroSharenh.Add(_newParaMe2);
                    //listaTag.Add(TagP0_ancho_);
                    listaTag.Add(TagP0_Prof_);
                }
            }

            AsignarPArametros(this);
        }

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagEstriboVigaCorte_H> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBaseV _geomeTagBase)
        {

            //_geomeTagBase.TagP0_A.IsOk = false;
            //_geomeTagBase.TagP0_B.IsOk = false;
            //_geomeTagBase.TagP0_D.IsOk = false;
            //_geomeTagBase.TagP0_E.IsOk = false;
            // _geomeTagBase.TagP0_F_SIN.IsOk = false;

            //_geomeTagBase.TagP0_C.IsOk = false;
            //_geomeTagBase.TagP0_L.IsOk = false;
            //_geomeTagBase.TagP0_C.CAmbiar(_geomeTagBase.TagP0_A);
        }
    }
}
