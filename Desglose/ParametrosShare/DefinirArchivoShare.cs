using Autodesk.Revit.UI;
using Desglose.Ayuda;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.ParametrosShare
{
    public class DefinirArchivoShare
    {
        public static void Ejecutar(UIApplication uiapp)
        {


            string rutaDearchivo = obtenerRuta();

            if (File.Exists(rutaDearchivo))
            {
                uiapp.Application.SharedParametersFilename = rutaDearchivo;
            }
            else
            { Util.ErrorMsg("No se pudo cargar Archivo compartido"); }
        }


        public static string obtenerRuta()
        {
            string ruta = "";
            InfoSystema infoSystema = new InfoSystema();

            if (infoSystema.disco.Trim() == "Disco Duro: 1838_9180_0122_0001_001B_448B_44B6_73D3.")
            { ruta = @"J:\_revit\PROYECTOS_REVIT\REVITArmaduraLosaRevit\_ParametrosCompartido\Parametros Compartidos.txt"; }
            else
            { ruta = @"\\Server-cdv\usuarios2\jose.huerta\A_ PROGRAMAS DE ARMADURAS REVIT\ParametroCompartidos\Parametros Compartidos.txt"; }

            return ruta;
        }
    }
}
