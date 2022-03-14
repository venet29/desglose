using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Buscar
{
    public abstract class GeometriaBase
    {
        protected readonly UIApplication _uiapp;
        protected Document _doc;
        protected StringBuilder _sbuilder;

        protected List<XYZ> listaPtoBorde;
        public List<PlanarFace> listaPlanarFace { get; set; }
        public List<List<PlanarFace>> listaGrupoPlanarFace { get; set; }

        

        protected GeometryElement geo;
        private bool IsComputeReferences;

        public Element _elemento { get; set; }
        public GeometriaBase(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;



            listaPtoBorde = new List<XYZ>();
            listaPlanarFace = new List<PlanarFace>();
            listaGrupoPlanarFace = new List<List<PlanarFace>>();
            IsComputeReferences = false;
        }

        public GeometriaBase(Document _doc)
        {

            this._doc = _doc;
            listaPtoBorde = new List<XYZ>();
            listaPlanarFace = new List<PlanarFace>();
            listaGrupoPlanarFace = new List<List<PlanarFace>>();
            IsComputeReferences = false;
        }

        public void M1_AsignarGeometriaObjecto(Element _elemento, bool IsComputeReferences_ = false)
        {
            IsComputeReferences = IsComputeReferences_;
            this._elemento = _elemento;
            //  ICollection<ElementId> runIds = stairs.GetStairsRuns();
            // _sbuilder.Clear();
            //_sbuilder.AppendLine($"M1_GetGEom----------------id: { _elemento.Id.IntegerValue}------------------------------------------------");

            M1_1_AsignarGeometriaObjectoOpction(_elemento);

            foreach (GeometryObject obj in geo)
            {
                if (obj is Solid)
                {
                    M3_AnalizarGeometrySolid(obj);
                }
                else if (obj is GeometryInstance)
                {
                    #region GEOMETRYINSTANCE O GEOMETRIA ANIDADA
                    M2_AnalizarInstanceGeometry(obj);
                    #endregion
                }
            }

            //LogNH.guardar_registro_StringBuilder(_sbuilder, ConstantesGenerales.rutaLogNh);
            //  Util.InfoMsg(_sbuilder.ToString());
        }

        protected void M1_1_AsignarGeometriaObjectoOpction(Element element)
        {

            #region PASO 3 OPCIONES DE GEOMETRIA
            //View3D _view3D_parabUSCAR = TiposFamilia3D.Get3DBuscar(_doc);
            Options opt = new Options();
            opt.ComputeReferences = IsComputeReferences; //TRUE SI ES QUE QUIERO ACOTAR CON LAS CARAS O BORDES O COLOCAR FAMILIAS FACEBASED
            opt.DetailLevel = ViewDetailLevel.Coarse;
            opt.IncludeNonVisibleObjects = false;//TRUE OBTENIENE LOS OBJETOS INVISIBLES Y/O VACIOS
            geo = element.get_Geometry(opt);// ESTO ES UNA LISTA DE GeometryObjects
            #endregion
        }


        private void M2_AnalizarInstanceGeometry(GeometryObject obj)
        {
            if (obj is GeometryInstance)
            {
                GeometryInstance instanciaanidada = obj as GeometryInstance; //INSTANCIA ANIDADA

                GeometryElement instanceGeometry = instanciaanidada.GetInstanceGeometry();  // en cooordenadas del proyecto
                                                                                            //  GeometryElement symbolGeometry = instanciaanidada.GetSymbolGeometry();  // en coordenas locales de familia

                //2) seguir las instancia
                foreach (GeometryObject obj2 in instanceGeometry)
                {

                    if (obj2 is Solid)
                    {

                        M3_AnalizarGeometrySolid(obj2);
                    }
                    else if (obj2 is GeometryInstance)
                    {
                        M2_AnalizarInstanceGeometry(obj2);
                    }

                }

            }
        }

        public virtual void M3_AnalizarGeometrySolid(GeometryObject obj2)
        {
            Solid solid2 = obj2 as Solid;
            if (solid2 != null && solid2.Faces.Size > 0)
            {
                listaPtoBorde.Clear();

                foreach (var face_ in solid2.Faces)
                {
                    if (!(face_ is PlanarFace)) continue;

                    PlanarFace face = face_ as PlanarFace;
                    if (face == null) continue;
                    listaPlanarFace.Add(face);

                    foreach (EdgeArray erray in face.EdgeLoops) //PERIMETROS CERRADOS O EDGELOOPS
                    {
                        foreach (Edge borde in erray) //COLECCION DE LINEAS DE BORDE
                        {
                            listaPtoBorde.Add(borde.AsCurve().GetEndPoint(0));
                            listaPtoBorde.Add(borde.AsCurve().GetEndPoint(1));

                        }
                    }
                }

                listaGrupoPlanarFace.Add(listaPlanarFace);
            }
        }



    }
}
