using Autodesk.Revit.DB;
using Desglose.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Ayuda
{
    public class ProyectadoEnPlano
    {
        public static XYZ ObtenerPtoProyectadoEnPlano(XYZ normalPlano, XYZ ptoReferencia, XYZ ptoBuscar)
        {
            XYZ ptoProyectado = XYZ.Zero;
            try
            {
                Plane plano = Plane.CreateByNormalAndOrigin(normalPlano.Redondear(8), ptoReferencia);
                ptoProyectado = plano.ProjectOnto(ptoBuscar);
            }
            catch (Exception)
            {
                return XYZ.Zero;
            }
            return ptoProyectado;

        }
    }
}
