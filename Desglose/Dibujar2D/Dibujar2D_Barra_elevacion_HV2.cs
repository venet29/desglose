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
    public class Dibujar2D_Barra_elevacion_HV2: Dibujar2D_Barra_BASE
    {

        protected GruposListasTraslapoIguales_HV2 _GruposListasTraslapoIguales_HV2;

        public Dibujar2D_Barra_elevacion_HV2(UIApplication _uiapp, GruposListasTraslapoIguales_HV2 gruposListasTraslapoIguales, Config_EspecialElev _Config_EspecialElv)
            :base( _uiapp)

        {
            _GruposListasTraslapoIguales_HV2 = gruposListasTraslapoIguales;
            _config_EspecialElv = _Config_EspecialElv;
        }

      
        public bool PreDibujar_HV2(bool isId)
        {

            try
            {

                SeleccionarElementosV _SeleccionarElementosV = new SeleccionarElementosV(_uiapp, true);
                _SeleccionarElementosV.M1_1_CrearWorkPLane_EnCentroViewSecction();

                // var lista = CrearListaPtos.M2_ListaPtoSimple(_uiapp, 1);
                // if (lista.Count == 0) return false;

                var trasnf = _config_EspecialElv.Trasform_;

                //posicionInicial = trasnf.EjecutarTransform( lista[0]);
                //XYZ posicionAUX = XYZ.Zero;

                XYZ direccionMuevenBarrasFAlsa = new XYZ(0, 0, -1);
                _config_EspecialElv.direccionMuevenBarrasFAlsa = direccionMuevenBarrasFAlsa;


                foreach (RebarDesglose_GrupoBarras_H itemGRUOP in _GruposListasTraslapoIguales_HV2.soloListaPrincipales)
                {
                    //   var BarraTipo = item._GrupoRebarDesglose[0];
                    //RebarElevDTO _RebarElevDTOANterior = null;
                 
                    for (int i = 0; i < itemGRUOP._GrupoRebarDesglose.Count; i++)
                    {
                        //if (Util.IsPar(i))
                        //    posicionAUX = posicionInicial;
                        //else//FALTA:hay q mejorar esta parte para que segun la seleccion es la direeccion donde se extiende las barra
                        //    posicionAUX = posicionInicial + trasnf.EjecutarTransform(Util.CmToFoot(2)* direccionMuevenBarrasFAlsa);

                        RebarDesglose_Barras_H item1 = itemGRUOP._GrupoRebarDesglose[i];
                        item1.contBarra = itemGRUOP._ListaRebarDesglose_GrupoBarrasRepetidas.Count + 1;
                        RebarElevDTO _RebarElevDTO = item1.ObtenerRebarElevDTO_HV2(_uiapp, isId, _config_EspecialElv, itemGRUOP.CantidadBArras);

                        GenerarBarra_2DH2(_RebarElevDTO);
                    }
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
