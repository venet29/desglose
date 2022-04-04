using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.DTO
{
    public class Config_DatosEstriboElevVigas
    {
        public string CantidadEstriboCONF { get; internal set; }
        public string CantidadEstriboLAT { get; internal set; }
        public string CantidadEstriboTRABA { get; internal set; }
        public int desplazaEESTRIBO { get; private set; }
        public int desplazaLATERAL { get; private set; }
        public int desplazaTRABA { get; private set; }

        internal void ObtenerDesplazamientos(int escala_realview)
        {
            if (escala_realview == 50)
                CAso50();
            else if (escala_realview == 75)
                CAso75();
            else if (escala_realview == 100)
                CAso100();
        }

        private void CAso50()
        {
            if (CantidadEstriboCONF != "" && CantidadEstriboLAT != "" && CantidadEstriboTRABA != "")
            {
                desplazaEESTRIBO = 10;
                desplazaLATERAL = 15;
                desplazaTRABA = 20;
            }
            else if (CantidadEstriboCONF != "" && CantidadEstriboLAT != "" || CantidadEstriboTRABA != "")
            {
                desplazaEESTRIBO = 8;
                desplazaLATERAL = -5;
                desplazaTRABA = -5;
            }

            else // este caso solo muestra uno centrado
            {
                desplazaEESTRIBO = 5;
                desplazaLATERAL = 5;
                desplazaTRABA = 5;
            }
        }
        private void CAso75()
        {
            if (CantidadEstriboCONF != "" && CantidadEstriboLAT != "" && CantidadEstriboTRABA != "")
            {
                desplazaEESTRIBO = 12;
                desplazaLATERAL = 6;
                desplazaTRABA = 22;
            }
            else if (CantidadEstriboCONF != "" && CantidadEstriboLAT != "" || CantidadEstriboTRABA != "")
            {
                desplazaEESTRIBO = 8;
                desplazaLATERAL = -5;
                desplazaTRABA = -5;
            }

            else // este caso solo muestra uno centrado
            {
                desplazaEESTRIBO = 5;
                desplazaLATERAL = 5;
                desplazaTRABA = 5;
            }
        }

        private void CAso100()
        {
            if (CantidadEstriboCONF != "" && CantidadEstriboLAT != "" && CantidadEstriboTRABA != "")
            {
                desplazaEESTRIBO = 15;
                desplazaLATERAL = -5;
                desplazaTRABA = -25;
            }
            else if (CantidadEstriboCONF != "" &&( CantidadEstriboLAT != "" ||  CantidadEstriboTRABA != ""))
            {
                desplazaEESTRIBO = 10;
                desplazaLATERAL = -10;
                desplazaTRABA = -10;
            }
            else // este caso solo muestra uno centrado
            {
                desplazaEESTRIBO = 5;
                desplazaLATERAL = 5;
                desplazaTRABA = 5;
            }
        }
    }
}
