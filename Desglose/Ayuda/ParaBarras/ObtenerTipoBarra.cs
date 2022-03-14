using System;
using System.Diagnostics;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Desglose.Ayuda;
using Desglose.BuscarTipos;

namespace Desglose.UTILES.ParaBarras
{
    public class ObtenerTipoBarra
    {
        private readonly Element _rebar;
        public TipoRebar TipoBarra_ { get; set; }
        public TipoBarraGeneral TipoBarraGeneral { get;  set; }
        public static bool IsMjs { get; set; } = true;

        public ObtenerTipoBarra(Element rebar)
        {
            this._rebar = rebar;
        }
        public  bool Ejecutar()
        {
            string _TipoBarra = "";
            if (!(_rebar is Rebar || _rebar is PathReinforcement || _rebar is RebarInSystem)) return false;
            
            try
            {
                _TipoBarra = ParameterUtil.FindParaByName(_rebar, "BarraTipo")?.AsString();
 
                if (_TipoBarra == "" || _TipoBarra == null)
                {
                    //Util.ErrorMsg($"Error  -> No se encontro tipo de barra  id:{_rebar.Id.IntegerValue} ");
                    Debug.WriteLine($"Error 'ObtenerTipoBarra' -> No se encontro tipo de barra  id:{_rebar.Id.IntegerValue} ");
                    TipoBarraGeneral = TipoBarraGeneral.NONE;
                    TipoBarra_ = TipoRebar.NONE;
                    return true;
                }

                TipoBarraGeneral = Tipos_Barras.M2_Buscar_TipoGrupoBarras_pornombre(_TipoBarra);
                TipoBarra_ = EnumeracionBuscador.ObtenerEnumGenerico(TipoRebar.NONE, _TipoBarra);

                if (TipoBarra_ == TipoRebar.NONE && IsMjs)
                {

                    var result =Util.ErrorMsgConDesctivar($"Error 'ObtenerTipoBarra' -> No se encontro tipo de barra. Barra Id:{_rebar.Id.IntegerValue}");
                    if (result == System.Windows.Forms.DialogResult.Cancel)
                    {
                        IsMjs = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error 'ObtenerTipoBarra' -> Error al encontro tipo de barra \n Barra id:{_rebar.Id.IntegerValue} \nex:{ex.Message}");
                return false;
            }

            return true;
        }


    }
}
