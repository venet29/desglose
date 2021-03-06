using Desglose.Ayuda;
using Desglose.DTO;
using Desglose.Extension;

namespace Desglose.Calculos.Tipo.ParaVigasElev
{
    public class ARebarLosa_desgloseEstribo_VigaElev : ARebarLosa_desglose
    {

        protected string CantidadEstriboCONF { get; set; }
        protected string CantidadEstriboLAT { get; set; }
        protected int CantidadEstriboTRABA { get; set; }
        public Config_DatosEstriboElevVigas _config_DatosEstriboElevVigas { get; set; }
        public ARebarLosa_desgloseEstribo_VigaElev(RebarElevDTO _RebarInferiorDTO) : base(_RebarInferiorDTO)
        {


            _config_DatosEstriboElevVigas = _rebarInferiorDTO.Config_DatosEstriboElevVigas;

        }

        protected void CargarPAratrosSHAR_Estribo()
        {
            if (_rebarInferiorDTO.Rebar_.ObtenerEspaciento_cm() < 15)
                CrearParameter("CantidadBarra", cantidadBArras.ToString());
            CrearParameter("CantidadEstriboCONF", _config_DatosEstriboElevVigas.CantidadEstriboCONF);

            CrearParameter("CantidadEstriboLAT", _config_DatosEstriboElevVigas.CantidadEstriboLAT);

            CrearParameter("CantidadEstriboTRABA", _config_DatosEstriboElevVigas.CantidadEstriboTRABA);
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
