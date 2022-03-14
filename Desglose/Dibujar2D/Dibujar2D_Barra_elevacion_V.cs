using Desglose.Calculos;
using Desglose.DTO;
using Desglose.Model;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Linq;
using Desglose.Ayuda;

namespace Desglose.Dibujar2D
{
    public class Dibujar2D_Barra_elevacion_V: Dibujar2D_Barra_BASE
    {


        public Dibujar2D_Barra_elevacion_V(UIApplication _uiapp, GruposListasTraslapoIguales_V gruposListasTraslapoIguales, Config_EspecialElev _Config_EspecialElv)
            :base( _uiapp,  gruposListasTraslapoIguales,  _Config_EspecialElv)

        {

        }

        public bool PreDibujar(bool isId)
        {

            try
            {
                SeleccionarElementosV _SeleccionarElementosV = new SeleccionarElementosV(_uiapp,true);
                _SeleccionarElementosV.M1_1_CrearWorkPLane_EnCentroViewSecction();

                var lista = CrearListaPtos.M2_ListaPtoSimple(_uiapp, 1);
                if (lista.Count == 0) return false;

                posicionInicial = lista[0];
                XYZ posicionAUX = XYZ.Zero;

                XYZ direccionMuevenBarrasFAlsa = _view.RightDirection;
                _config_EspecialElv.direccionMuevenBarrasFAlsa = direccionMuevenBarrasFAlsa;


                foreach (RebarDesglose_GrupoBarras_V itemGRUOP in _GruposListasTraslapoIguales_v.soloListaPrincipales)
                {
                    //   var BarraTipo = item._GrupoRebarDesglose[0];
                    RebarElevDTO _RebarElevDTOANterior=null;
                    bool IsPrimero = true;
                    for (int i = 0; i < itemGRUOP._GrupoRebarDesglose.Count; i++)
                    {
                        if (Util.IsPar(i))
                            posicionAUX = posicionInicial;
                        else
                            posicionAUX = posicionInicial+Util.CmToFoot(2)* direccionMuevenBarrasFAlsa;

                        RebarDesglose_Barras_V item1 = itemGRUOP._GrupoRebarDesglose[i];
                        item1.contBarra = itemGRUOP._ListaRebarDesglose_GrupoBarrasRepetidas.Count + 1;
                        RebarElevDTO _RebarElevDTO = item1.ObtenerRebarElevDTO(posicionAUX, _uiapp, isId, _config_EspecialElv);

                        GenerarBarra_2D(_RebarElevDTO);

                        if(!IsPrimero)
                             CrearDimensionENtreBArras(_RebarElevDTO, _RebarElevDTOANterior);

                        _RebarElevDTOANterior = _RebarElevDTO;
                        IsPrimero = false;
                    }




                    posicionInicial = posicionInicial + direccionMuevenBarrasFAlsa * Util.CmToFoot(50);
                }

            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }


    }
}
