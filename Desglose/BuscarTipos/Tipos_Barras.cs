using Desglose.Ayuda;
using Desglose.Ayuda.ParaBarras.Entidades;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Desglose.BuscarTipos
{
    public class Tipos_Barras
    {
        public static List<EntidadBarras> ListaBarraTipo { get; set; }

        public static EntidadBarras elemetEncontrado;

        public static string M1_Buscar_nombreTipoBarras_porTipoRebar(TipoRebar name)
        {
            BuscarDiccionario(name);
            return(elemetEncontrado != null ? elemetEncontrado.nombre : "");
        }

        public static TipoBarraGeneral M2_Buscar_TipoGrupoBarras_pornombre(string name)
        {
            BuscarDiccionario(name);
            return (elemetEncontrado != null ? elemetEncontrado.grupo : TipoBarraGeneral.NONE);
        }

        public static EntidadBarras M3_Buscar_EntidadBarras_porTipoRebar(TipoRebar TipoRebar)
        {
            BuscarDiccionario(TipoRebar);
            if(elemetEncontrado == null) BuscarDiccionario(TipoRebar.NONE);
            return elemetEncontrado;
        }

        public static void Limpiar() => ListaBarraTipo = new List<EntidadBarras>();

        private static void GenerarLista() => ListaBarraTipo = FactoryEntidadBarras.ObtenerListaEntidades();



        private static bool BuscarDiccionario(object nombre)
        {
            elemetEncontrado = null;
            if (ListaBarraTipo == null)
            {
                GenerarLista();
               
            }
            else if (ListaBarraTipo.Count == 0)
            {
                GenerarLista();
            }

            EntidadBarras result = null;
            if (nombre is string)
               result = ListaBarraTipo.Where(c => c.nombre == ((string)nombre)).FirstOrDefault();
            else if (nombre is TipoRebar)
                result = ListaBarraTipo.Where(c => c.tipoRebar == ((TipoRebar)nombre)).FirstOrDefault();
            else if (nombre is TipoBarraGeneral)
                result = ListaBarraTipo.Where(c => c.grupo == ((TipoBarraGeneral)nombre)).FirstOrDefault();

            if (result != null) 
                elemetEncontrado = result;
            else
                result = ListaBarraTipo.Where(c => c.tipoRebar == (TipoRebar.NONE)).FirstOrDefault();



            return (elemetEncontrado == null ? false : true);
        }


    }


}
