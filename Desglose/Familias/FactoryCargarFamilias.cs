using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Familias
{
    public class FactoryCargarFamilias
    {


      
        public static List<Tuple<string, string>> CrearDiccionarioRutasFamilias(string rutaRaiz)
        {
            //    var ssd = (1, 2);
            List<Tuple<string, string>> listaRutasFamilias = new List<Tuple<string, string>>();


            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_FCorte_", rutaRaiz + @"M_Structural MRA Rebar_FCorte_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_LCorte_", rutaRaiz + @"M_Structural MRA Rebar_LCorte_.rfa"));
            
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_MLB_", rutaRaiz + @"M_Structural MRA Rebar_MLB_.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar Section", rutaRaiz + @"M_Structural MRA Rebar Section_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar SectionLAT", rutaRaiz + @"M_Structural MRA Rebar SectionLat_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar Section_SegunElev_", rutaRaiz + @"M_Structural MRA Rebar Section_SegunElev.rfa"));


            return listaRutasFamilias;

        }


    }
}
