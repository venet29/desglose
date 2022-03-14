using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Ayuda
{
   public class AyudaAumentarLetra
    {
        public static char result { get; private set; }

        public static  bool Ejecutar(char car )
        {

            try
            {
              //  char car = 'A';
                car++;
                result = car; // B 
            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg("Error al incrementar letra");
                return false;
            }

            return true;
        }
    }
}
