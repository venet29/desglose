﻿using Autodesk.Revit.DB;
using Desglose.Entidades;

namespace Desglose.Ayuda
{
    public class PtosCurvaAuxDTO
    {
        public XYZ PtoInicial { get; internal set; }
        public XYZ PtoFinal { get; internal set; }
        public XYZ PtoMedio { get; internal set; }

        public XYZ PtoInicialTransformada { get; internal set; }
        public XYZ PtoFinalTransformada { get; internal set; }
        public XYZ PtoMedioTransformada { get; internal set; }

        public double largoCurve { get; set; }
        public parametrosRebar ParametrosRebar { get; internal set; }
    }
}
