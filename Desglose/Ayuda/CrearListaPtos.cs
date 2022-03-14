using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Ayuda
{
    public class ptoGenerarIntervaloDTO
    {

        public XYZ pto { get; set; }
        public double distaciato_L1_L2 { get; set; }
    }

    public class CrearListaPtos
    {


        public static List<XYZ> M2_ListaPtoSimple(UIApplication uiapp, int contador = 100000)
        {

            List<XYZ> _listaptoTramo = new List<XYZ>();
            try
            {

                XYZ _ptoIntervalo = XYZ.Zero;
                bool continuar = true;
                int cont = 0;
                while (continuar && cont < contador)
                {
                    try
                    {
                        continuar = false;
                        ObjectSnapTypes snapTypes = ObjectSnapTypes.Nearest;
                        //Nos permite seleccionar un punto en una posición cualquiera y nos da el dato XYZ
                        _ptoIntervalo = uiapp.ActiveUIDocument.Selection.PickPoint(snapTypes, "Seleccionar Punto ");

                        _listaptoTramo.Add(_ptoIntervalo);
                        cont += 1;
                        continuar = true;
                    }
                    catch (Exception ex)
                    {
                        Util.DebugDescripcion(ex);
                        continuar = false;
                    }
                }

            }
            catch (Exception)
            {
                _listaptoTramo.Clear();
            }
            return _listaptoTramo;
        }


 

   
        public static XYZ ObtenerCentroPoligono(List<XYZ> ListaPoligono4ptos)
        {
            return new XYZ(ListaPoligono4ptos.Average(c => c.X), ListaPoligono4ptos.Average(c => c.Y), ListaPoligono4ptos.Average(c => c.Z));
        }

    }
}
