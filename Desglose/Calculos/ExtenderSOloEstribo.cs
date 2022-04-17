using Autodesk.Revit.UI;
using Desglose.Model;
using System;
using System.Collections.Generic;

namespace Desglose.Calculos
{
    internal class ExtenderSOloEstribo
    {
        private UIApplication _uiapp;
        private List<RebarDesglose_GrupoBarras_V> gruposRebarMismaLinea;

        public ExtenderSOloEstribo(UIApplication uiapp, List<RebarDesglose_GrupoBarras_V> gruposRebarMismaLinea)
        {
            _uiapp = uiapp;
            this.gruposRebarMismaLinea = gruposRebarMismaLinea;
        }

        public List<RebarDesglose_GrupoBarras_V> GruposRebarMismaLinea { get; internal set; }

        internal bool Extender()
        {
            throw new NotImplementedException();
        }
    }
}