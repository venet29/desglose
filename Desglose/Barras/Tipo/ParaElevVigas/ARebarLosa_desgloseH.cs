using Desglose.DTO;

namespace Desglose.Calculos
{
    public  class AARebarLosa_desgloseH: ARebarLosa_desglose
    {

        protected string _texToLargoParciales { get;  set; }
        protected string _largoTotal { get;  set; }
        protected int  cantidadBArras { get; set; }

        public AARebarLosa_desgloseH(RebarElevDTO _RebarInferiorDTO):base(_RebarInferiorDTO)
        {
            cantidadBArras = _RebarInferiorDTO.cantidadBarras;
            _largoTotal = "";
            _texToLargoParciales = "";
        }

        protected void CargarPAratrosSHARE()
        {
            ParametroShareNhDTO _newParaMe_Cantiadabarras = new ParametroShareNhDTO()
            {
                Isok = true,
                NombrePAra = "CantidadBarra",
                valor = cantidadBArras.ToString()
            };
            listaPArametroSharenh.Add(_newParaMe_Cantiadabarras);

            ParametroShareNhDTO _newParaMe_largoTotal = new ParametroShareNhDTO()
            {
                Isok = true,
                NombrePAra = "LargoTotal",
                valor = _largoTotal
            };
            listaPArametroSharenh.Add(_newParaMe_largoTotal);

            ParametroShareNhDTO _newParaMe__texToLargoParciales = new ParametroShareNhDTO()
            {
                Isok = true,
                NombrePAra = "LargoParciales",
                valor = _texToLargoParciales
            };
            listaPArametroSharenh.Add(_newParaMe__texToLargoParciales);
        }

    }
}
