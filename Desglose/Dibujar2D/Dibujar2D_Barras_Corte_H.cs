using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Desglose.Anotacion;
using Desglose.Ayuda;
using Desglose.Calculos;
using Desglose.DTO;
using Desglose.Extension;
using Desglose.Familias;
using Desglose.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Desglose.Dibujar2D
{
    //solo se usa para signar la propieda si es lateral, o pertenece alguna linea de barra
    public class RebarDesglose_Barras_H2
    {
        public RebarDesglose_Barras_H RebarDesglose_Barras_H_ { get; set; }
        public TipobarraH TipobarraH_ { get; set; }
    }
    internal class Dibujar2D_Barras_Corte_H
    {
        private UIApplication _uiapp;
        private GruposListasEstribo_HCorte _GruposListasEstribo;
        private DatosHost _dtosHost;
        private Config_EspecialCorte _Config_EspecialCorte;
        private List<RebarDesglose_Barras_H> listaBArrasEnElev;

        public List<RebarDesglose_Barras_H2> ListaBarrasSuperiores { get; private set; }
        public List<RebarDesglose_Barras_H2> ListaBarrasInferior { get; private set; }
        ViewSection _section;
        private List<RebarDesglose_Barras_H2> listaBArrasEnElev_laterales;

        public Dibujar2D_Barras_Corte_H(UIApplication uiapp, GruposListasEstribo_HCorte gruposListasEstribo, Config_EspecialCorte config_EspecialCorte)
        {
            _uiapp = uiapp;
            _GruposListasEstribo = gruposListasEstribo;
            _dtosHost = _GruposListasEstribo._DatosHost;
            _Config_EspecialCorte = config_EspecialCorte;
            listaBArrasEnElev = gruposListasEstribo.listaBArrasEnElev;
        }

        internal bool PreDibujar(ViewSection section, View viewOriginal)
        {
            _section = section;
            try
            {

                listaBArrasEnElev_laterales = listaBArrasEnElev.Where(c => c.diametroMM <= _Config_EspecialCorte.DiamtroLateralMax)
                                                                                            .Select(c => new RebarDesglose_Barras_H2()
                                                                                            {
                                                                                                RebarDesglose_Barras_H_ = c,
                                                                                                TipobarraH_ = TipobarraH.Lateral
                                                                                            }).ToList();

                List<RebarDesglose_Barras_H2> listaBArrasEnElev_NOlaterales = listaBArrasEnElev.Where(c => c.diametroMM > _Config_EspecialCorte.DiamtroLateralMax)
                                                                              .Select(c => new RebarDesglose_Barras_H2()
                                                                              {
                                                                                  RebarDesglose_Barras_H_ = c,
                                                                                  TipobarraH_ = TipobarraH.Linea1INF
                                                                              }).ToList();

                double zMax = listaBArrasEnElev_NOlaterales.Max(c => c.RebarDesglose_Barras_H_._rebarDesglose.trasform.EjecutarTransformInvertida(c.RebarDesglose_Barras_H_.ptoMedio).Z);
                double zmin = listaBArrasEnElev_NOlaterales.Min(c => c.RebarDesglose_Barras_H_._rebarDesglose.trasform.EjecutarTransformInvertida(c.RebarDesglose_Barras_H_.ptoMedio).Z);
                double Zmedio = (zMax + zmin) / 2;
                ListaBarrasSuperiores = listaBArrasEnElev_NOlaterales.Where(c => c.RebarDesglose_Barras_H_._rebarDesglose.trasform.EjecutarTransformInvertida(c.RebarDesglose_Barras_H_.ptoMedio).Z > Zmedio)
                                                                     .OrderByDescending(c => c.RebarDesglose_Barras_H_._rebarDesglose.trasform.EjecutarTransformInvertida(c.RebarDesglose_Barras_H_.ptoMedio).Z).ToList();
                ListaBarrasInferior = listaBArrasEnElev_NOlaterales.Where(c => c.RebarDesglose_Barras_H_._rebarDesglose.trasform.EjecutarTransformInvertida(c.RebarDesglose_Barras_H_.ptoMedio).Z < Zmedio)

                                                                    .OrderBy(c => c.RebarDesglose_Barras_H_._rebarDesglose.trasform.EjecutarTransformInvertida(c.RebarDesglose_Barras_H_.ptoMedio).Z)
                                                                    .ToList();

                if (_Config_EspecialCorte.ParaBarraHorizontalEnCorteViga == TipoTagBArraHorizontalENcorte.mostrarDiamtro)
                {
                    //Lista SUperior
                    for (int i = 0; i < ListaBarrasSuperiores.Count; i++)
                    {
                        // if ()
                    }

                    //Lista Inferior
                    for (int i = 0; i < ListaBarrasInferior.Count; i++)
                    {
                        // if ()
                    }
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error  Dibujar2D_Barras_Corte_H_PreDibujar  ex:{ex.Message}");
                return false;
            }
            return true;
        }

        internal bool Dibujar(XYZ posicionInicialbarras)
        {
            try
            {           
                int _dire = _Config_EspecialCorte.dire;
                CrearTrasformadaSobreVectorDesg _Trasform = _Config_EspecialCorte.Trasform_;
                //1)laterales
                AnotacionMultipleBarra _AnotacionMultipleBarraLAt = new AnotacionMultipleBarra(_uiapp, _section, _Config_EspecialCorte.dire);

                var resultaLat = CalculoPtoTagBArraHorizontal_Corte.PtoInferior(listaBArrasEnElev_laterales, _section, _Trasform, _dire);
                if (resultaLat.Isok)
                {
                    XYZ _origen = posicionInicialbarras + _section.RightDirection * 0;
                    XYZ taghead = _origen + _section.RightDirection * 0.3 + new XYZ(0, 0, -0.05);

                    //con las dos de mas abajo
                    List<ElementId> listat = CalculoPtoTagBArraHorizontal_Corte.lista2BarrasMAsInferior(listaBArrasEnElev_laterales, _section, _Trasform, _dire);
                    //con todas las barras
                    //List<ElementId> listat = listaBArrasEnElev_laterales.Select(c => c.RebarDesglose_Barras_H_._rebarDesglose._rebar.Id).ToList();

                    AnotacionMultipleBarraDTO _AnotacionMultipleBarraDTO = new AnotacionMultipleBarraDTO()
                    {
                        Origen_ = _origen,
                        taghead_ = taghead,
                        nombrefamilia = CONSTFami.NOmbre_FAMILIA_LAT// "M_Structural MRA Rebar SectionLat_"
                    };
                    _AnotacionMultipleBarraLAt.COpiarParametrosShare(listaBArrasEnElev_laterales);

                    _AnotacionMultipleBarraLAt.CreateAnnotation(listat, _AnotacionMultipleBarraDTO);
                }



                AnotacionMultipleBarra _AnotacionMultipleBarra = new AnotacionMultipleBarra(_uiapp, _section, _Config_EspecialCorte.dire);
                //2)  a)inferior
                var resultaInf = CalculoPtoTagBArraHorizontal_Corte.PtoInferior(ListaBarrasInferior, _section, _Trasform, _dire);
                //3) a)superiores
                var resultaSup = CalculoPtoTagBArraHorizontal_Corte.PtoSuperior(ListaBarrasSuperiores, _section, _Trasform, _dire);




                resultaInf.resultInserccion = posicionInicialbarras.AsignarZ(resultaInf.resultInserccion.Z);
                resultaSup.resultInserccion = posicionInicialbarras.AsignarZ(resultaSup.resultInserccion.Z);

                //2)  b)inferior
                if (resultaInf.Isok)
                {
                    XYZ OrigenAUX_ = resultaInf.resultInserccion + _section.RightDirection * 0 + new XYZ(0, 0, -0.5);

                    string _auxNombreFAmili = (_Config_EspecialCorte.ParaBarraHorizontalEnCorteViga == TipoTagBArraHorizontalENcorte.mostrarDiamtro
                                                ? CONSTFami.NOmbre_Section_Diam
                                                : CONSTFami.NOmbre_Section_SegunElev);

                    AnotacionMultipleBarraDTO _AnotacionMultipleBarraInfDTO = new AnotacionMultipleBarraDTO()
                    {
                        Origen_ = OrigenAUX_,
                        taghead_ = OrigenAUX_ + _section.RightDirection * 0.3,
                        nombrefamilia = _auxNombreFAmili //"MRA Rebar Section_"
                    };
                    List<ElementId> listaSup = ListaBarrasInferior.Select(c => c.RebarDesglose_Barras_H_._rebarDesglose._rebar.Id).ToList();
                    _AnotacionMultipleBarra.CreateAnnotation(listaSup, _AnotacionMultipleBarraInfDTO);
                }

                //3) b)superiores

                if (resultaSup.Isok)
                {

                    XYZ OrigenAUX_ = resultaSup.resultInserccion + _section.RightDirection * 0 + new XYZ(0, 0, 0.5);

                    string _auxNombreFAmili = (_Config_EspecialCorte.ParaBarraHorizontalEnCorteViga == TipoTagBArraHorizontalENcorte.mostrarDiamtro
                                                ? CONSTFami.NOmbre_Section_Diam
                                                : CONSTFami.NOmbre_Section_SegunElev);

                    AnotacionMultipleBarraDTO _AnotacionMultipleBarraSuPDTO = new AnotacionMultipleBarraDTO()
                    {
                        Origen_ = OrigenAUX_,
                        taghead_ = OrigenAUX_ + _section.RightDirection * 0.3,
                        nombrefamilia = _auxNombreFAmili
                    };
                    List<ElementId> listaINF = ListaBarrasSuperiores.Select(c => c.RebarDesglose_Barras_H_._rebarDesglose._rebar.Id).ToList();
                    _AnotacionMultipleBarra.CreateAnnotation(listaINF, _AnotacionMultipleBarraSuPDTO);
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error  Dibujar2D_Barras_Corte_H_PreDibujar  ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}