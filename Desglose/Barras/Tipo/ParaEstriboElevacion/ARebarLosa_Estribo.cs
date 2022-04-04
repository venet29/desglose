using Desglose.DTO;

namespace Desglose.Calculos
{
    public  class ARebarLosa_Estribo: ARebarLosa_desglose
    {

        protected string CantidadEstriboCONF { get;  set; }
        protected string CantidadEstriboLAT { get;  set; }
        protected int CantidadEstriboTRABA { get; set; }
        public Config_DatosEstriboElevVigas _config_DatosEstriboElevVigas { get; set; }
        public ARebarLosa_Estribo(RebarElevDTO _RebarInferiorDTO):base(_RebarInferiorDTO)
        {

                  
        _config_DatosEstriboElevVigas = _rebarInferiorDTO.Config_DatosEstriboElevVigas;

        }

        protected void CargarPAratrosSHAR_Estribo()
        {

                CrearParameter("CantidadEstriboCONF", _config_DatosEstriboElevVigas.CantidadEstriboCONF);

                CrearParameter("CantidadEstriboLAT", _config_DatosEstriboElevVigas.CantidadEstriboLAT);

                CrearParameter("CantidadEstriboTRABA", _config_DatosEstriboElevVigas.CantidadEstriboTRABA);
        }

        private void CrearParameter(string _NombrePAra ,string _valor)
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
