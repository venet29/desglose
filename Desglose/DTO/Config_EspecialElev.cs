using Desglose.Ayuda;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.DTO
{
    public class Config_EspecialElev
    {

        public ParametroShareNhDTO tipoBarraElev_parameterShare { get;  set; }
        public List<ParametroShareNhDTO> ListaPAraShare { get;  set; }
        public bool IsAgregarId { get; set; }
        public CasoAnalisas TipoCasoAnalisis { get;  set; }
        public CrearTrasformadaSobreVectorDesg Trasform_ { get;  set; }
        public XYZ direccionMuevenBarrasFAlsa { get;  set; }
        public double DiamtroLateralMax { get;  set; }

    }
}
