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

            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, ParameterType.Text, "F_prefijo", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "a9016747-5d31-40a5-99da-7545c3cc5ef4");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, ParameterType.Text, "Temp_Sufijo", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "282df609-a3ec-4187-a1a6-9bab955d2992");

            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, ParameterType.Text, "CantidadLateralesCorte", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "be87c785-1540-45d7-8bb9-f46b0189824d");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, ParameterType.Text, "CantidadBarra", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "0368c956-9e15-41f5-a86b-375c0c55b11e");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, ParameterType.Text, "LargoParciales", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "53530261-a393-46a6-9ae0-01639eb6a3f8");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, ParameterType.Text, "LargoTotal", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "d0bcbf64-eaca-4d45-92c9-a3a5bda8e75e");

            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, ParameterType.Text, "LargoTraslapo", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "dd5855fa-ca8c-442a-9a05-525cffbd3544");
            //CantidadEstriboCONF
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, ParameterType.Text, "CantidadEstriboCONF", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "630baf2e-6d65-4a4c-b331-fcb9e7d63a7a");

            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, ParameterType.Text, "CantidadEstriboCONF", "Estribo", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "630baf2e-6d65-4a4c-b331-fcb9e7d63a7a");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, ParameterType.Text, "CantidadEstriboLAT", "Estribo", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "f34f3a0f-de70-4c05-b3c6-2a216d7f758a");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, ParameterType.Text, "CantidadEstriboTRABA", "Estribo", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "c6a11417-9b23-4742-ac77-ac9bf1d4246d");
            
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
