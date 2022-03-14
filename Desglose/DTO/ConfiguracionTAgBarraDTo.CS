using Autodesk.Revit.DB;
using Desglose.Ayuda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.DTO
{
    public class ConfiguracionTAgBarraDTo
    {
        public bool IsDIrectriz { get; set; }
        public TagOrientation tagOrientation { get; set; }

        public XYZ LeaderElbow { get; set; }
        public XYZ desplazamientoPathReinSpanSymbol { get; set; }
        //barras elevacione Horizon y Vertical
        public TipoRebar BarraTipo { get; set; }

        //barras SOLO elevacione Horizon 
        public TipoCaraObjeto TipoCaraObjeto_ { get; set; }

    }
}
