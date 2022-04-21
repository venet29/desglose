
using Autodesk.Revit.DB;
using Desglose.Ayuda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.DTO
{
    class TraslapoBarrasH
    {
        private XYZ ptoInicial_trans;
        private XYZ ptofinal_trans1;

        private TipobarraH tipobarr;

        public XYZ ptoInicial { get; set; }
        public XYZ ptofinal { get; set; }
        public double largoTraslapo { get; set; }
        public TraslapoBarrasH(XYZ ptoInicial_trans, XYZ ptofinal_trans1, XYZ ptoInicial, XYZ ptofinal, TipobarraH tipobarr, double largoTraslapo)
        {
            this.ptoInicial_trans = ptoInicial_trans;
            this.ptofinal_trans1 = ptofinal_trans1;
            this.ptoInicial = ptoInicial;
            this.ptofinal = ptofinal;
            this.tipobarr = tipobarr;
            this.largoTraslapo = largoTraslapo;
        }

        internal XYZ ObtenerPtoInserccion()
        {
            return (ptoInicial + ptofinal) / 2 ;

            switch (tipobarr)
            {

                case TipobarraH.Linea1SUP:
                    return (ptoInicial + ptofinal) / 2+  new XYZ(0,0,Util.CmToFoot(5)) ;
                case TipobarraH.Linea2SUP:
                    return (ptoInicial + ptofinal) / 2 + new XYZ(0, 0, Util.CmToFoot(5));
                case TipobarraH.Linea3SUP:
                    return (ptoInicial + ptofinal) / 2 + new XYZ(0, 0, Util.CmToFoot(5));
                case TipobarraH.Linea4SUP:
                    return (ptoInicial + ptofinal) / 2 + new XYZ(0, 0, Util.CmToFoot(5));
                case TipobarraH.Linea5SUP:
                    return (ptoInicial + ptofinal) / 2 + new XYZ(0, 0, Util.CmToFoot(5));
                case TipobarraH.Linea1INF:
                    return (ptoInicial + ptofinal) / 2 - new XYZ(0, 0, Util.CmToFoot(5));

                case TipobarraH.Linea2INF:
                    return (ptoInicial + ptofinal) / 2 - new XYZ(0, 0, Util.CmToFoot(5));
                case TipobarraH.Linea3INF:
                    return (ptoInicial + ptofinal) / 2 - new XYZ(0, 0, Util.CmToFoot(5));
                case TipobarraH.Linea4INF:
                    return (ptoInicial + ptofinal) / 2 - new XYZ(0, 0, Util.CmToFoot(5));
                case TipobarraH.Linea5INF:
                    return (ptoInicial + ptofinal) / 2 - new XYZ(0, 0, Util.CmToFoot(5));
                default:
                    return (ptoInicial + ptofinal) / 2 + new XYZ(0, 0, Util.CmToFoot(5));
            }
        }
    }
}
