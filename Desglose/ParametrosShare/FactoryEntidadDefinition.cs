using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Desglose.Ayuda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.ParametrosShare
{
    public class FactoryEntidadDefinition
    {
        public static List<EntidadDefinition> _lista;

        internal static List<EntidadDefinition> CrearListaConParametrosFundaciones(UIApplication uiapp)
        {
            Util.ErrorMsg("Parametros de funsacioneas no implementadas");
            _lista = new List<EntidadDefinition>();
            return _lista;
        }

        internal static List<EntidadDefinition> CrearListaConParametrosEscalera(UIApplication uiapp)
        {
            Util.ErrorMsg("Parametros de funsacioneas no implementadas");
            _lista = new List<EntidadDefinition>();
            return _lista;
        }

        internal static EntidadDefinition AsignarNuevoParametroALista(string nombrepara)
        {
            return new EntidadDefinition(nombrepara);
        }


        internal static List<EntidadDefinition> CrearListaConParametrosDesglose(UIApplication uiapp, bool repetido = false)
        {

            _lista = new List<EntidadDefinition>();

            //solo desglose
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, ParameterType.Text, "IdMLB", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "a133d20d-32bf-4780-88e9-9826c3d4a5d2");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, ParameterType.Text, "MLB", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "Tipo de grupo de barra", "0f022cc2-8b49-470a-8687-2d8e4feabd3f");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, ParameterType.Text, "OpcionCuantia", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "d711e591-ffd0-48d9-985b-da3039cbfd6a");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, ParameterType.ReinforcementLength, "LargoSumaParciales", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "014e7044-cdb9-4bba-80e0-5251d13a2dda");
            return _lista;

        }



        public static void AsignarNuevoParametroALista(UIApplication uiapp, BuiltInCategory builtInCategory, ParameterType parameterType, string nombreParametro,
                                                    string nombreGrupo, bool IsModificable, bool EsOcultoCuandoNOvalor, bool EsVisible, string Description, string _guid = "")
        {
            try
            {
                EntidadDefinition _entidadDefinition1 = new EntidadDefinition(uiapp, builtInCategory, parameterType, nombreParametro, nombreGrupo, IsModificable, EsOcultoCuandoNOvalor, EsVisible, Description, _guid);
                if (_entidadDefinition1 == null)
                {
                    Util.ErrorMsg($"No se pudo crear parametro compartido {nombreParametro}");
                    return;
                }
                _lista.Add(_entidadDefinition1);
            }
            catch (Exception)
            {

                Util.ErrorMsg($"Error al crear parametro compartido {nombreParametro}");
            }
        }
        public static void AsignarNuevoParametroALista(UIApplication uiapp, BuiltInCategory[] builtInCategory, ParameterType parameterType, string nombreParametro, string nombreGrupo,
                                            bool IsModificable, bool EsOcultoCuandoNOvalor, bool EsVisible, string Description, string _guid = "")
        {
            try
            {
                EntidadDefinition _entidadDefinition1 = new EntidadDefinition(uiapp, builtInCategory, parameterType, nombreParametro,
                                                                              nombreGrupo, IsModificable, EsOcultoCuandoNOvalor, EsVisible, Description, _guid);
                if (_entidadDefinition1 == null)
                {
                    Util.ErrorMsg($"No se pudo crear parametro compartido {nombreParametro}");
                    return;
                }
                _lista.Add(_entidadDefinition1);
            }
            catch (Exception)
            {
                Util.ErrorMsg($"Error al crear parametro compartido {nombreParametro}");
            }
        }
    }
}
