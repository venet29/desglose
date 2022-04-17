using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Desglose.Ayuda;
using Desglose.BuscarTipos;
using Desglose.DTO;

namespace Desglose.Tag
{
    public class TagBarra
    {
        Document _doc;
        private View _view;
        private int? _escala;
        private XYZ factorPorEscala;

        public TagBarra(XYZ posicion, string nombre, string nombreFamilia, Element ElementIndependentTagPath)
        {

            this.posicion = posicion;
            this.nombre = nombre;
            this.nombreFamilia = nombreFamilia;
            this.ElementIndependentTagPath = ElementIndependentTagPath;
            this.IsLibre = false;
            if (ElementIndependentTagPath != null)
            {
                IsOk = true;

                _doc = ElementIndependentTagPath?.Document;
                _view = ElementIndependentTagPath.Document?.ActiveView;
                _escala = _view?.Scale;
            }
            else
                IsOk = false;


        }

        public string valorTag { get; set; } = "";// se se uso temporamente para remplazar tag

        public XYZ posicion { get; set; }
        // public XYZ _TagHeadPosition { get; set; }
        public XYZ LeaderElbow { get; set; }
        public XYZ LeaderEnd { get; set; }
        public double anguloGrado { get; set; }
        public string nombre { get; set; }
        public string nombreFamilia { get; set; }
        public Element ElementIndependentTagPath { get; set; }
        public bool IsOk { get; set; } = true;
        public bool IsDIrectriz { get; set; } = true;
        public bool IsLibre { get; set; }

        public void CAmbiar(TagBarra tagP0_)
        {
            if (tagP0_ == null)
            {
                IsOk = false;
                return;
            }
            nombre = tagP0_.nombre;
            nombreFamilia = tagP0_.nombreFamilia;
            if (tagP0_.ElementIndependentTagPath != null)
            {
                ElementIndependentTagPath = tagP0_.ElementIndependentTagPath;
                IsOk = true;
            }
            else
            { IsOk = false; }
        }

        public TagBarra Copiar()
        {
            return new TagBarra(this.posicion, this.nombre, this.nombreFamilia, this.ElementIndependentTagPath);

        }


        public void DibujarTagPathReinforment(Element element, Document doc, View viewActual, XYZ desplazamientoPathReinSpanSymbol)
        {
            if (!IsOk) return;
            IndependentTag independentTag = IndependentTag.Create(doc, ElementIndependentTagPath.Id, viewActual.Id, new Reference(element), false,
                                                      TagOrientation.Vertical, desplazamientoPathReinSpanSymbol); //new XYZ(0, 0, 0)
            independentTag.TagHeadPosition = posicion;


        }

        public void DibujarTagrREbarLosa(Element element, Document doc, View viewActual, XYZ desplazamientoPathReinSpanSymbol)
        {
            if (valorTag != "")
            {
                // crea los texto de los lados de los estribos desglosado en corte de pilares
                AyudaCreartexto.M4_CrearTExtoSinTrans(doc, posicion, valorTag, "2.5mm Arial Narrow", 0, TipoCOloresTexto.negro);
                return;
            }
            if (!IsOk) return;

            IndependentTag independentTag = IndependentTag.Create(doc, ElementIndependentTagPath.Id, viewActual.Id, new Reference(element), false,
                                                      TagOrientation.Horizontal, desplazamientoPathReinSpanSymbol); //new XYZ(0, 0, 0)
            independentTag.TagHeadPosition = posicion;

        }


        public void DibujarTagRebarFund(Element element, Document doc, View viewActual, ConfiguracionTAgBarraDTo confBarraTab)
        {

            if (!IsOk) return;
            IndependentTag independentTag = IndependentTag.Create(doc, ElementIndependentTagPath.Id, viewActual.Id, new Reference(element), IsDIrectriz,
                                                      confBarraTab.tagOrientation, confBarraTab.desplazamientoPathReinSpanSymbol); //new XYZ(0, 0, 0)
            independentTag.TagHeadPosition = posicion;

            if (IsDIrectriz == true)
            {
                if (independentTag == null) return;
                if (LeaderElbow == null) return;
                independentTag.LeaderElbow = LeaderElbow;
                FamilySymbol tagSymbol = _doc.GetElement(independentTag.GetTypeId()) as FamilySymbol;
                if (tagSymbol != null)
                    tagSymbol.get_Parameter(BuiltInParameter.LEADER_ARROWHEAD).Set(new ElementId(-1));
            }

        }


        public void DibujarTagRebarV(Element element, Document doc, View viewActual, ConfiguracionTAgBarraDTo confBarraTab)
        {
            //   if (!IsOk) return;
            IndependentTag independentTag = IndependentTag.Create(doc, ElementIndependentTagPath.Id, viewActual.Id, new Reference(element), confBarraTab.IsDIrectriz,
                                                      confBarraTab.tagOrientation, confBarraTab.desplazamientoPathReinSpanSymbol); //new XYZ(0, 0, 0)
            int escala = viewActual.Scale;
            factorPorEscala = XYZ.Zero;

            DesplazamientoPorEscalaYPosicionSeleccion(confBarraTab, escala);
            independentTag.TagHeadPosition = posicion + factorPorEscala;
            AgregarDirectriz(confBarraTab, independentTag);

        }
        public void DibujarTagRebar_ConLibre(Rebar rebar, Document doc, View view, ConfiguracionTAgBarraDTo confBarraTag)
        {
            IndependentTag independentTag = IndependentTag.Create(doc, ElementIndependentTagPath.Id, view.Id, new Reference(rebar), IsDIrectriz,
                                                 confBarraTag.tagOrientation, confBarraTag.desplazamientoPathReinSpanSymbol); //new XYZ(0, 0, 0)

            if (IsLibre)
            {
                independentTag.LeaderEndCondition = LeaderEndCondition.Free;
                independentTag.LeaderEnd = LeaderEnd;
            }

            independentTag.TagHeadPosition = posicion;

            AgregarDirectriz(IsDIrectriz, LeaderElbow, independentTag);

        }
        private void DesplazamientoPorEscalaYPosicionSeleccion(ConfiguracionTAgBarraDTo confBarraTab, int escala)
        {
            if (confBarraTab.BarraTipo == TipoRebar.ELEV_BA_V)
            {
                if (escala == 75) factorPorEscala = -new XYZ(0, 0, 0.75);

                if (escala == 100) factorPorEscala = -new XYZ(0, 0, 1.5);
            }
            else if (confBarraTab.BarraTipo == TipoRebar.ELEV_BA_CABEZA_HORQ)
            {
                //factorPorEscala = (posicion - LeaderElbow) - new XYZ(0, 0, -1.38);
            }
            else if (confBarraTab.BarraTipo == TipoRebar.ELEV_BA_H)
            {
                if (confBarraTab.TipoCaraObjeto_ == TipoCaraObjeto.Inferior)
                {
                    if (escala == 75) factorPorEscala = -new XYZ(0, 0, 0.2);

                    if (escala == 100) factorPorEscala = -new XYZ(0, 0, 0.4);
                }
                else
                {
                    if (escala == 75) factorPorEscala = -new XYZ(0, 0, 0.1);

                    if (escala == 100) factorPorEscala = -new XYZ(0, 0, 0.1);
                }

            }
            else
            {
                if (escala == 75) factorPorEscala = -new XYZ(0, 0, 0.75);

                if (escala == 100) factorPorEscala = -new XYZ(0, 0, 1.5);
            }
        }

        public void DibujarTagRebarRefuerzoLosa(Element element, Document doc, View viewActual, ConfiguracionTAgBarraDTo confBarraTab)
        {
            if (!IsOk) return;
            IndependentTag independentTag = IndependentTag.Create(doc, ElementIndependentTagPath.Id, viewActual.Id, new Reference(element), confBarraTab.IsDIrectriz,
                                                      confBarraTab.tagOrientation, confBarraTab.desplazamientoPathReinSpanSymbol); //new XYZ(0, 0, 0)
            independentTag.TagHeadPosition = posicion;

            AgregarDirectriz(confBarraTab, independentTag);

        }

        public void DibujarTagEstribo(Element element, Document doc, View viewActual, ConfiguracionTAgEstriboDTo configuracionTAgEstriboDTo, XYZ posicionTag)
        {
            if (!IsOk) return;
            IndependentTag independentTag = IndependentTag.Create(doc, ElementIndependentTagPath.Id, viewActual.Id, new Reference(element), configuracionTAgEstriboDTo.IsDIrectriz,
                                                      configuracionTAgEstriboDTo.tagOrientation, configuracionTAgEstriboDTo.desplazamientoPathReinSpanSymbol); //new XYZ(0, 0, 0)
            independentTag.TagHeadPosition = posicionTag;
        }

        private void AgregarDirectriz(ConfiguracionTAgBarraDTo confBarraTab, IndependentTag independentTag)
        {
            if (!confBarraTab.IsDIrectriz) return;
            AgregarDirectriz(confBarraTab.IsDIrectriz, posicion + confBarraTab.LeaderElbow, independentTag);
        }
        private void AgregarDirectriz(bool IsDIrectriz, XYZ ptoelbow, IndependentTag independentTag)
        {
            if (!IsDIrectriz) return;

            if (independentTag == null) return;
                independentTag.LeaderElbow = ptoelbow;

                FamilySymbol tagSymbol = _doc.GetElement(independentTag.GetTypeId()) as FamilySymbol;

                var elem = Tipos_Arrow.ObtenerArrowheadPorNombre(_doc, "Filled Dot 2mm_" + _escala);
                if (elem != null)
                    tagSymbol.get_Parameter(BuiltInParameter.LEADER_ARROWHEAD).Set(elem.Id);
            
        }

    }

}
