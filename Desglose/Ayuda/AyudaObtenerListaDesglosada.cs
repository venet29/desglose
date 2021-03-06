using Desglose.Model;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Desglose.Extension;

namespace Desglose.Ayuda
{
    public class AyudaObtenerListaDesglosada
    {
        public static List<RebarDesglose> Lista_RebarDesglose { get; set; }


        //Agregar posicion linea
        public static bool ObtenerLista(List<Element> _Listrebar, UIApplication uiapp)
        {
            View _view = uiapp.ActiveUIDocument.ActiveView;
            try
            {
                Lista_RebarDesglose = new List<RebarDesglose>();
                foreach (Rebar _rebar in _Listrebar)
                {
                    RebarDesglose _RebarDesglose = new RebarDesglose(uiapp, _rebar);

                    _RebarDesglose.Ejecutar();

                    if (!Util.IsParallel(_RebarDesglose._normal.GetXY0(), _view.RightDirection))
                        continue;

                    Lista_RebarDesglose.Add(_RebarDesglose);
                }
            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener lista rebar: {ex.Message}");
                return false;
            }
            return true;
        }



        //Agregar posicion linea
        public static bool AgregarPosicionLinea(double diamLaterales)
        {
            try
            {
                if (Lista_RebarDesglose == null) return true;

                var ListaBarrasAnalizadas = Lista_RebarDesglose.Where(c => c._tipoBarraEspecifico == TipoRebar.ELEV_BA_H ||
                                                                     c._tipoBarraEspecifico == TipoRebar.ELEV_BA_V)
                                                                    .ToList();

                if (ListaBarrasAnalizadas.Count == 0) return true;

                var lista_laterales = ListaBarrasAnalizadas.Where(c => c.Diametro_MM <= diamLaterales).ToList();
                lista_laterales.ForEach(c => c.TipobarraH_ = TipobarraH.Lateral);
                lista_laterales.ForEach(c => c._tipoBarraEspecifico = TipoRebar.ELEV_ES_VL);

                var lista_Nolaterales = ListaBarrasAnalizadas.Where(c => c.Diametro_MM > diamLaterales).ToList();
                lista_Nolaterales.ForEach(c => c.TipobarraH_ = TipobarraH.LineaNOLateral);

                double zMax = lista_Nolaterales.Max(c => c.CurvaMasLargo_WraperRebarLargo.ptoInicial.Z);
                double zmin = lista_Nolaterales.Min(c => c.CurvaMasLargo_WraperRebarLargo.ptoFinal.Z);
                double Zmedio = (zMax + zmin) / 2;


                var ListaBarraSuperiore = lista_Nolaterales.Where(c => c.CurvaMasLargo_WraperRebarLargo.ptoFinal.Z >= Zmedio - 0.01).ToList();


                double rangoInicialL2 = Util.CmToFoot(2);
                double rangoInicialL3 = Util.CmToFoot(15);
                double rangoInicialL4 = Util.CmToFoot(26);
                //superior configurar primera linea
                ListaBarraSuperiore.Where(c => Util.IsSimilarValor(c.CurvaMasLargo_WraperRebarLargo.ptoFinal.Z, zMax, Util.MmToFoot(c.Diametro_MM) * 1))


                                    .ToList().ForEach(c => c.TipobarraH_ = TipobarraH.Linea1SUP);

                ListaBarraSuperiore.Where(c => zMax - rangoInicialL2 > c.CurvaMasLargo_WraperRebarLargo.ptoFinal.Z &&
                                               zMax - rangoInicialL3 < c.CurvaMasLargo_WraperRebarLargo.ptoFinal.Z)
                                    .ToList().ForEach(c => c.TipobarraH_ = TipobarraH.Linea2SUP);

                ListaBarraSuperiore.Where(c => zMax - rangoInicialL3 > c.CurvaMasLargo_WraperRebarLargo.ptoFinal.Z &&
                                               zMax - rangoInicialL4 < c.CurvaMasLargo_WraperRebarLargo.ptoFinal.Z)
                                                     .ToList().ForEach(c => c.TipobarraH_ = TipobarraH.Linea3SUP);

                ListaBarraSuperiore.Where(c => zMax - rangoInicialL4 > c.CurvaMasLargo_WraperRebarLargo.ptoFinal.Z &&
                                                        Zmedio < c.CurvaMasLargo_WraperRebarLargo.ptoFinal.Z)
                                              .ToList().ForEach(c => c.TipobarraH_ = TipobarraH.Linea4SUP);

                //inferior configurar primera linea
                var ListaBarraInferior = lista_Nolaterales.Where(c => c.CurvaMasLargo_WraperRebarLargo.ptoFinal.Z < Zmedio - 0.01).ToList();

                ListaBarraInferior.Where(c => Util.IsSimilarValor(c.CurvaMasLargo_WraperRebarLargo.ptoFinal.Z, zmin, Util.MmToFoot(c.Diametro_MM) * 1))
                                .ToList().ForEach(c => c.TipobarraH_ = TipobarraH.Linea1INF);

                ListaBarraInferior.Where(c => zmin + rangoInicialL2 < c.CurvaMasLargo_WraperRebarLargo.ptoFinal.Z &&
                                               zmin + rangoInicialL3 > c.CurvaMasLargo_WraperRebarLargo.ptoFinal.Z)
                                .ToList().ForEach(c => c.TipobarraH_ = TipobarraH.Linea2INF);

                ListaBarraInferior.Where(c => zmin + rangoInicialL3 < c.CurvaMasLargo_WraperRebarLargo.ptoFinal.Z &&
                                               zmin + rangoInicialL4 > c.CurvaMasLargo_WraperRebarLargo.ptoFinal.Z)
                                                     .ToList().ForEach(c => c.TipobarraH_ = TipobarraH.Linea3INF);

                ListaBarraInferior.Where(c => zmin + rangoInicialL4 < c.CurvaMasLargo_WraperRebarLargo.ptoFinal.Z && Zmedio > c.CurvaMasLargo_WraperRebarLargo.ptoFinal.Z)
                                              .ToList().ForEach(c => c.TipobarraH_ = TipobarraH.Linea4INF);
            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener lista rebar: {ex.Message}");
                return false;
            }
            return true;
        }

    }
}
