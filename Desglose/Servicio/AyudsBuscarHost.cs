using Autodesk.Revit.DB;
using Desglose.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Servicio
{
    class AyudsBuscarHostDTo
    {
        public RebarDesglose _rebarDesglose { get; set; }
        public int  idHost { get; set; }
        public AyudsBuscarHostDTo(RebarDesglose rebarDesglose,int getHostId)
        {
            _rebarDesglose = rebarDesglose;
            idHost = getHostId;
        }


    }
    class AyudsBuscarHost
    {
        public static RebarDesglose _Result_HostDTo { get;  set; }

        internal static bool BuscarHostMAsRepetido(List<RebarDesglose_Barras_H> listaBArras)
        {
            try
            {
                _Result_HostDTo = null;
                var result = listaBArras.Select(c => new AyudsBuscarHostDTo(c._rebarDesglose, c._rebarDesglose._rebar.GetHostId().IntegerValue)).ToList();

                var groups = result.GroupBy(x => x.idHost);
               var _PrimerResult_HostDTo = groups.OrderByDescending(x => x.Count()).First();

                foreach (var item in _PrimerResult_HostDTo)
                {
                    _Result_HostDTo = item._rebarDesglose;
                }
                if (_Result_HostDTo != null) return true;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
