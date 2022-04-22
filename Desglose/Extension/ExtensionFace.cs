using Autodesk.Revit.DB;
using Desglose.Ayuda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Desglose.Extension
{
    public static class ExtensionFace
    {
        private static double CurvePoints_Umin;
        private static double CurvePoints_Umax;
        private static double CurvePoints_Vmin;
        private static double CurvePoints_Vmax;

        public static bool IsTopFace(this Face f)
        {
            BoundingBoxUV b = f.GetBoundingBox();
            UV p = b.Min;
            UV q = b.Max;
            UV midpoint = p + 0.5 * (q - p);
            XYZ normal = f.ComputeNormal(midpoint);
            return Util.PointsUpwards(normal);
        }

        public static bool IsDownFace(this Face f)
        {
            BoundingBoxUV b = f.GetBoundingBox();
            UV p = b.Min;
            UV q = b.Max;
            UV midpoint = p + 0.5 * (q - p);
            XYZ normal = f.ComputeNormal(midpoint);
            return Util.Pointsdownwards(normal);
        }

        public static List<Curve> ObtenerListaCurvas(this Face f)
        {
            List<Curve> listaCurve = new List<Curve>();
            try
            {
                if (f == null) return listaCurve;
                //  f.GetEdgesAsCurveLoops().SelectMany(c=>c.SelectMany()).
                List<CurveLoop> list = f.GetEdgesAsCurveLoops().ToList().ToList();

                foreach (CurveLoop cl in list)
                {
                    foreach (Curve _curve in cl)
                    {
                        if (_curve.Length < 0.0001) continue;
                        listaCurve.Add(_curve);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener el planarface ex:{ex.Message}");
                listaCurve.Clear();
            }
            return listaCurve;
        }

        public static List<XYZ> ObtenerListaPuntos(this Face f)
        {
            List<XYZ> listaCurve = new List<XYZ>();
            try
            {
                listaCurve = (List<XYZ>)f.Triangulate().Vertices;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener el ptos planarface ex:{ex.Message}");
                listaCurve.Clear();
            }
            return listaCurve;
        }




        public static XYZ ObtenerPtosInterseccionFace(this PlanarFace _planarFace, XYZ ptoselec, XYZ direccon, bool _ISMensajes = false)
        {
            XYZ ptoInters = XYZ.Zero; ;
            try
            {

                var ListaPlanarFaceSuperior = new List<PlanarFace>() { _planarFace };

                Curve lineVertcal = Line.CreateBound(ptoselec + direccon * 50, ptoselec - direccon * 50);

                ptoInters = XYZ.Zero;

                //****************************
                ptoInters = ExtensionFloorAyuda.ObtenerPto(ListaPlanarFaceSuperior, lineVertcal, _ISMensajes);
                //**

                if (ptoInters.IsAlmostEqualTo(XYZ.Zero))
                {
                    Util.ErrorMsg($"Error al obtener punto intersecion con face superior de losa. Se utliza punto 'origen' de face superior");
                    return ListaPlanarFaceSuperior[0].Origin;
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener espesor  de losa variable. Ex:{ex.Message}");
            }
            return ptoInters;
        }

     

        public static bool IsPtosProyectadoVerticalmenteFace(this PlanarFace _planarFace, XYZ ptoselec, bool _ISMensajes = false)
        {
            XYZ ptoInters = XYZ.Zero; ;
            try
            {


                var ListaPlanarFaceSuperior = new List<PlanarFace>() { _planarFace };

                Curve lineVertcal = Line.CreateBound(ptoselec + new XYZ(0, 0, +50), ptoselec + new XYZ(0, 0, -50));

                ptoInters = XYZ.Zero;

                //****************************
                ptoInters = ExtensionFloorAyuda.ObtenerPto(ListaPlanarFaceSuperior, lineVertcal, _ISMensajes);
                //**

                if (ptoInters.IsAlmostEqualTo(XYZ.Zero))
                    return false;
                else
                    return true;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener espesor  de losa variable. Ex:{ex.Message}");
                return false;
            }

        }

        public static XYZ GetCenterOfFace(this Face MyFace)
        {
            CalcularALargosVU_maximoYmoin(MyFace);

            UV MyCenter = new UV((CurvePoints_Umax + CurvePoints_Umin) / 2, (CurvePoints_Vmax + CurvePoints_Vmin) / 2);
            XYZ ptcentro = MyFace.Evaluate(MyCenter);
            return ptcentro;
        }


        public static double MaximoladoLArgo(this Face MyFace)
        {
            CalcularALargosVU_maximoYmoin(MyFace);

            return   Math.Max(CurvePoints_Umax - CurvePoints_Umin, CurvePoints_Vmax - CurvePoints_Vmin);
        }

        private static void CalcularALargosVU_maximoYmoin(Face MyFace)
        {
            CurvePoints_Umin = double.MaxValue;
            CurvePoints_Umax = double.MinValue;
            CurvePoints_Vmin = double.MaxValue;
            CurvePoints_Vmax = double.MinValue;
            List<List<UV>> EdgePointsUV = new List<List<UV>>();
            foreach (EdgeArray MyEdgeArray in MyFace.EdgeLoops)
            {
                foreach (Edge MyEdge in MyEdgeArray)
                {
                    foreach (UV MyUV in MyEdge.TessellateOnFace(MyFace))
                    {
                        CurvePoints_Umin = Math.Min(CurvePoints_Umin, MyUV.U);
                        CurvePoints_Umax = Math.Max(CurvePoints_Umax, MyUV.U);
                        CurvePoints_Vmin = Math.Min(CurvePoints_Vmin, MyUV.V);
                        CurvePoints_Vmax = Math.Max(CurvePoints_Vmax, MyUV.V);
                    }
                }
            }
        }

        public static XYZ ProjectNH(this PlanarFace pl, XYZ ptoInter)
        {
            var resul = pl.Project(ptoInter);
            if (resul == null) return XYZ.Zero;

            return resul.XYZPoint;
        }

    }
}
