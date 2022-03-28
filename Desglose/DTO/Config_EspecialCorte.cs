using Desglose.Ayuda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.DTO
{
    public class Config_EspecialCorte
    {
        
        public int dire { get; set; } // direccion corte
        public double tolerancia { get; set; }
        public TipoCOnfLargo TipoCOnfigLargo { get; internal set; }
        public TipoCOnfCuantia TipoCOnfigCuantia { get; internal set; }
        public List<ParametroShareNhDTO> ListaPAraShare { get; internal set; }
        public CasoAnalisas TipoCasoAnalisis  { get; internal set; }
        public CrearTrasformadaSobreVectorDesg Trasform_ { get; internal set; }
        public int DiamtroLateralMax { get; internal set; }
        public TipoTagBArraHorizontalENcorte ParaBarraHorizontalEnCorteViga { get;  set; }
   
    }
}
