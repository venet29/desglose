using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Dimensiones
{
    public class DimensionesDatosTextoDTO
    {
        public string Above { get; set; } = "textoAbovoe";
        public string Below { get; set; } = "textobelow";
        public string ValueOverride { get; set; } = "replaceWithText";
        public bool IsSobreEscribir { get; set; } = false;
    }

}
