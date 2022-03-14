using Autodesk.Revit.DB;
using Desglose.Ayuda;

namespace Desglose.RebarLosa.DTO
{
    public class VectorDireccionLosaExternaInclinadaDTO
    {
        public XYZ direccionLosa { get; set; }
        public Floor Losa { get; set; }
        public PosicionDeBusqueda PosicionDeBusquedaEnu { get; set; }
        public bool IsLosaEncontrada { get; internal set; }
    }
}
