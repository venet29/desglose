using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Calculos
{
    public interface IRebarLosa_Desglose
    {
        double mayorDistancia { get; set; }
        void M2A_GenerarBarra();
        bool M1A_IsTodoOK();

 
    }

    public interface IRebarLosa
    {
        void M2A_GenerarBarra();
        bool M1A_IsTodoOK();

    }
}
