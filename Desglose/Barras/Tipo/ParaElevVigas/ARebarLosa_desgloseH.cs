using Autodesk.Revit.DB;
using Desglose.Ayuda;
using Desglose.DTO;

namespace Desglose.Calculos.Tipo.ParaElevVigas
{
    public class AARebarLosa_desgloseH : ARebarLosa_desglose
    {
        protected XYZ PtoIniConDesplazamineto;
        protected XYZ PtoFinConDesplazamineto;

        protected string _texToLargoParciales { get; set; }
        protected string _largoTotal { get; set; }
        protected int cantidadBArras { get; set; }

        protected XYZ DesplazamietoPOrLInea { get; set; }

        public AARebarLosa_desgloseH(RebarElevDTO _RebarInferiorDTO) : base(_RebarInferiorDTO)
        {
            cantidadBArras = _RebarInferiorDTO.cantidadBarras;
            _largoTotal = "";
            _texToLargoParciales = "";
        }

        protected void CargarPAratrosSHARE()
        {
            CrearParameter("CantidadBarra", cantidadBArras.ToString());
            CrearParameter("LargoTotal", _largoTotal.ToString());
            CrearParameter("LargoParciales", _texToLargoParciales.ToString());
        }

        internal bool DesplazamientoSegunLInea()        
        {
            DesplazamietoPOrLInea = XYZ.Zero;
            try
            {    
                switch (_rebarInferiorDTO._rebarDesglose.TipobarraH_)
                {
                    case Ayuda.TipobarraH.Lateral:
                        break;
                    case Ayuda.TipobarraH.Linea1SUP:
                        CrearParameter(ConstNH.CONST_FPrefijo, "F'=");
                        break;
                    case Ayuda.TipobarraH.Linea2SUP:
                        {
                            CrearParameter(ConstNH.CONST_FPrefijo, "+F'=");
                            CrearParameter(ConstNH.CONST_TempSUFIJO, "(2°C)");
                            DesplazamietoPOrLInea = ConstNH.CONST_DesfaseLine;
                            break;
                        }
                    case Ayuda.TipobarraH.Linea3SUP:
                        {
                            CrearParameter(ConstNH.CONST_FPrefijo, "+F'=");
                            CrearParameter(ConstNH.CONST_TempSUFIJO, "(2°C)");
                            DesplazamietoPOrLInea = ConstNH.CONST_DesfaseLine * 2;
                            break;
                        }
                    case Ayuda.TipobarraH.Linea4SUP:
                        {
                            CrearParameter(ConstNH.CONST_FPrefijo, "+F'=");
                            CrearParameter(ConstNH.CONST_TempSUFIJO, "(2°C)");
                            DesplazamietoPOrLInea = ConstNH.CONST_DesfaseLine * 3;
                            break;
                        }
                    case Ayuda.TipobarraH.Linea5SUP:
                        break;
                    case Ayuda.TipobarraH.Linea1INF:
                        CrearParameter(ConstNH.CONST_FPrefijo, "F=");
                        break;
                    case Ayuda.TipobarraH.Linea2INF:
                        {
                            CrearParameter(ConstNH.CONST_FPrefijo, "+F=");
                            CrearParameter(ConstNH.CONST_TempSUFIJO, "(2°C)");
                            DesplazamietoPOrLInea = -ConstNH.CONST_DesfaseLine;
                            break;
                        }
                    case Ayuda.TipobarraH.Linea3INF:
                        {
                            CrearParameter(ConstNH.CONST_FPrefijo, "+F=");
                            CrearParameter(ConstNH.CONST_TempSUFIJO, "(2°C)");
                            DesplazamietoPOrLInea = -ConstNH.CONST_DesfaseLine * 2;
                            break;
                        }
                    case Ayuda.TipobarraH.Linea4INF:
                        {
                            CrearParameter(ConstNH.CONST_FPrefijo, "+F=");
                            CrearParameter(ConstNH.CONST_TempSUFIJO  , "(2°C)");
                            DesplazamietoPOrLInea = -ConstNH.CONST_DesfaseLine * 3;
                            break;
                        }
                    case Ayuda.TipobarraH.Linea5INF:
                        break;
                    case Ayuda.TipobarraH.NONE:
                        break;
                    case Ayuda.TipobarraH.LineaNOLateral:
                        break;
                    default:
                        break;
                }

                 PtoIniConDesplazamineto = _rebarInferiorDTO.ptoini + DesplazamietoPOrLInea;
                 PtoFinConDesplazamineto = _rebarInferiorDTO.ptofinal + DesplazamietoPOrLInea;
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }
    
        private void CrearParameter(string _NombrePAra, string _valor)
        {
            if (_valor == "") return;
            if (_valor == null) return;

            ParametroShareNhDTO _newParaMe_Cantiadabarras = new ParametroShareNhDTO()
            {
                Isok = true,
                NombrePAra = _NombrePAra,
                valor = _valor
            };
            listaPArametroSharenh.Add(_newParaMe_Cantiadabarras);
        }
    }
}
