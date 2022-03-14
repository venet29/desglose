using Autodesk.Revit.DB;
using Desglose.Ayuda;
using Desglose.Buscar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Extension
{
   public static class ExtensionElement
    {
        public static List<List<PlanarFace>> ListaFace(this Element elemet, bool IsComputeReferences = false)
        {
            GeometriaViga _geometriaBase = new GeometriaViga(elemet.Document);
            _geometriaBase.M1_AsignarGeometriaObjecto(elemet, IsComputeReferences);
            return _geometriaBase.listaGrupoPlanarFace;

        }
        public static PlanarFace ObtenerCaraSegun_Direccion(this Element _element, XYZ direccion)
        {
            var ListaPlanarFace = _element.ListaFace()[0];
            if (ListaPlanarFace == null) return null;

            PlanarFace planaface = ListaPlanarFace.Where(c => UtilDesglose.IsParallelIgualSentido(c.FaceNormal, direccion))
                                                  .FirstOrDefault();
            return planaface;
        }
    }
}
