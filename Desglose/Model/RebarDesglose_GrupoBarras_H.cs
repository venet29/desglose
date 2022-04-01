using Desglose.Ayuda;
using Desglose.Entidades;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Desglose.Extension;

namespace Desglose.Model
{
    public class RebarDesglose_GrupoBarras_H
    {
        static int nextId = 0;
        public int contGrupo { get; private set; }
        public List<RebarDesglose_Barras_H> _GrupoRebarDesglose { get; set; } // contiene las  barras en la misma linea
        public List<RebarDesglose_GrupoBarras_H> _ListaRebarDesglose_GrupoBarrasRepetidas { get; set; }  // contiene grupos de lineas de  barras en la misma linea

        public XYZ _ptoInicial { get; set; }
        public XYZ _ptoFinal { get; set; }
        public bool Isok { get; set; }
        public Line _curvePrincipal { get; set; }
        public CrearTrasformadaSobreVectorDesg Trasform { get; }
        public casoAgrupar _casoAgrupar { get; set; }
        public TipobarraH TipobarraH_ { get; set; }
        public string replaceWithText { get; set; }
        public string textobelow { get; set; }
    

      

        public static RebarDesglose_GrupoBarras_H Creador_RebarDesglose_GrupoBarras(List<RebarDesglose_Barras_H> _grupoRebarDesglose, CrearTrasformadaSobreVectorDesg trasform)
        {

            if (_grupoRebarDesglose == null) return new RebarDesglose_GrupoBarras_H();
            if (_grupoRebarDesglose.Count == 0) return new RebarDesglose_GrupoBarras_H();

            XYZ ptoInicial = _grupoRebarDesglose[0].ptoInicial;
            XYZ ptoFinal = _grupoRebarDesglose.Last().ptoFinal;
            Line _NewcurvePrincipal = Line.CreateBound(ptoInicial, ptoFinal);

            return new RebarDesglose_GrupoBarras_H(_grupoRebarDesglose, _NewcurvePrincipal, trasform);
        }

        private RebarDesglose_GrupoBarras_H(List<RebarDesglose_Barras_H> _grupoRebarDesglose, Line _curvePrincipal, CrearTrasformadaSobreVectorDesg trasform)
        {
            nextId += 1;
            this.contGrupo = nextId;
            this._GrupoRebarDesglose = _grupoRebarDesglose;
            this._curvePrincipal = _curvePrincipal;
            Trasform = trasform;
            XYZ p0 = _curvePrincipal.GetEndPoint(0);
            this._ptoInicial = p0.AsignarZ(Math.Round(p0.Z,5));
            XYZ p1 = _curvePrincipal.GetEndPoint(1);
            this._ptoFinal = p1.AsignarZ(Math.Round(p1.Z, 5));
            this._ListaRebarDesglose_GrupoBarrasRepetidas = new List<RebarDesglose_GrupoBarras_H>();
            this.Isok = true;
            this._casoAgrupar = casoAgrupar.NoAnalizada;
            this.TipobarraH_ = TipobarraH.NONE;

        }

        public bool ObtenerTextos()
        {
            try
            {
                replaceWithText = "";
                textobelow = "";

                //estribo
                int cantidadEstribo = _GrupoRebarDesglose.Count(c => c._tipoBarraEspecifico == TipoRebar.ELEV_ES_V);

                if (cantidadEstribo != 0)
                {
                    var primer= _GrupoRebarDesglose.Find(c => c._tipoBarraEspecifico == TipoRebar.ELEV_ES_V);
                    int diametro=primer._rebarDesglose._rebar.ObtenerDiametroInt();
                    int Espacia= (int)primer._rebarDesglose._rebar.ObtenerEspaciento_cm();
                    replaceWithText = $"{cantidadEstribo}E.Ø{diametro}a{Espacia}";
                }


                //trabas
                int cantidadTrab = _GrupoRebarDesglose.Count(c => c._tipoBarraEspecifico == TipoRebar.ELEV_ES_VT);

                if (cantidadTrab != 0)
                {
                    var primer = _GrupoRebarDesglose.Find(c => c._tipoBarraEspecifico == TipoRebar.ELEV_ES_VT);
                    int diametro = primer._rebarDesglose._rebar.ObtenerDiametroInt();
                    int Espacia = (int)primer._rebarDesglose._rebar.ObtenerEspaciento_cm();
                    textobelow = $"+{cantidadTrab}TR.Ø{diametro}a{Espacia}";
                }

                if (replaceWithText == "" && textobelow != "")
                {
                    replaceWithText = textobelow;
                    textobelow = "";
                }

            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }

        public RebarDesglose_GrupoBarras_H()
        {
            this.Isok = false;
        }
    }
}
