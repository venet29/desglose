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

            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar Section_", rutaRaiz + @"M_Structural MRA Rebar Section_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar SectionLAT_", rutaRaiz + @"M_Structural MRA Rebar SectionLat_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar Section_SegunElev_", rutaRaiz + @"M_Structural MRA Rebar Section_SegunElev.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar LT", rutaRaiz + @"M_Structural MRA Rebar LT.rfa"));//_L_
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar LP", rutaRaiz + @"M_Structural MRA Rebar LP.rfa"));//_C_ 
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar FBarra", rutaRaiz + @"M_Structural MRA Rebar FBarra.rfa"));//_F_
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar FBarraCompleto", rutaRaiz + @"M_Structural MRA Rebar FBarraCompleto.rfa"));//_F_
            

            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar EV", rutaRaiz + @"M_Structural MRA Rebar EV.rfa"));//M_Structural MRA Rebar_VIGA.rfa"
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar EVL", rutaRaiz + @"M_Structural MRA Rebar EVL.rfa"));//M_Structural MRA Rebar_VIGAL.rfa
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar EVT", rutaRaiz + @"M_Structural MRA Rebar EVT.rfa"));//M_Structural MRA Rebar_VIGALT.rfa

            return listaRutasFamilias;

        }


    }
}
