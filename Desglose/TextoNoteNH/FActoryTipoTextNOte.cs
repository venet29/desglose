using Desglose.UTILES.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.TextoNoteNH
{
    public class FActoryTipoTextNOte
    {
        public static List<TipoTextoDTO> ObtenerLista()
        {
            List<TipoTextoDTO> _list = new List<TipoTextoDTO>();

            TipoTextoDTO _newACotar = new TipoTextoDTO("AcotarBarra-NH", 255, 0, 0, _TEXT_FONT: "Arial Narrow", _TEXT_SIZE: 0.00656168, _TEXT_TAB_SIZE: 0.04166667); //0.00656168 = 2mm   //  0.04166667= 12.7 mm
            TipoTextoDTO _newTextoHorqy = new TipoTextoDTO("TextoHorq", 255, 255, 255, _TEXT_FONT: "RomanS", _TEXT_SIZE: 0.00656168, _TEXT_TAB_SIZE: 0.04166667);


            _list.Add(_newACotar);
            _list.Add(_newTextoHorqy);

            return _list;
        }
    }
}
