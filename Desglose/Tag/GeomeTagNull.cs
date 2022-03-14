using Desglose.Ayuda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Tag
{
    public class GeomeTagNull : IGeometriaTag
    {
        public List<TagBarra> listaTag { get; set; }
        public GeomeTagNull()
        {

        }

        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                double AnguloRadian = args.angulorad;
                M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian);
                M2_CAlcularPtosDeTAg();
                M3_DefinirRebarShape();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar Tag  ex:${ex.Message}");
                return false;
            }
            return true;

        }

        public void M2_CAlcularPtosDeTAg(bool IsGarficarEnForm = false)
        {
            listaTag = new List<TagBarra>();
        }

        public void M1_ObtnerPtosInicialYFinalDeBarra(double anguloRoomRad)
        {

        }

        public void M3_DefinirRebarShape()
        {

        }

        public bool M4_IsFAmiliaValida() => false;
    }
}
