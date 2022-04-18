using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Desglose.Ayuda;
using Desglose.Extension;
using Desglose.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Desglose.Calculos
{
    class ExtenderSOloEstriboDto
    {

        public RebarDesglose_GrupoBarras_V _RebarDesglose_GrupoBarras_V { get; set; }
        public DatosHost DatosHost { get; set; }
        public Element _elementhostBarras { get; set; }
        public int idHostEleme { get; set; }
        public XYZ _ptoFinal { get;  set; }
        public XYZ _ptoInicial { get;  set; }

        public ExtenderSOloEstriboDto(RebarDesglose_GrupoBarras_V item, DatosHost _DatosHost)
        {
            this._RebarDesglose_GrupoBarras_V = item;
            DatosHost = _DatosHost;
            _elementhostBarras = _DatosHost.host;
            idHostEleme = _DatosHost.host.Id.IntegerValue;
            _ptoFinal=item._ptoFinal;
            _ptoInicial = item._ptoInicial;
            // _RebarDesglose_GrupoBarras_V._GrupoRebarDesglose.Where(c => c._tipoBarraEspecifico == Ayuda.TipoRebar.ELEV_ES).FirstOrDefault();
        }



    }

    internal class ExtenderSOloEstribo
    {
        private UIApplication _uiapp;
        //private List<RebarDesglose_GrupoBarras_V> gruposRebarMismaLinea;

        public ExtenderSOloEstribo(UIApplication uiapp, List<RebarDesglose_GrupoBarras_V> gruposRebarMismaLinea)
        {
            _uiapp = uiapp;
            this.GruposRebarMismaLinea = gruposRebarMismaLinea;
        }

        public List<RebarDesglose_GrupoBarras_V> GruposRebarMismaLinea { get; internal set; }

        internal bool Extender()
        {
            try
            {
                List<ExtenderSOloEstriboDto> _ListaExtenderSOloEstriboDto = new List<ExtenderSOloEstriboDto>();

                ///
                foreach (RebarDesglose_GrupoBarras_V item in GruposRebarMismaLinea)
                {
                    RebarDesglose_Barras_V _newRebarDesglose_Barras_V = item._GrupoRebarDesglose.Where(c => c._tipoBarraEspecifico == Ayuda.TipoRebar.ELEV_ES).FirstOrDefault();

                    DatosHost _DatosHost = new DatosHost(_uiapp, _newRebarDesglose_Barras_V._rebarDesglose);

                    if (!_DatosHost.ObtenerHost()) continue;

                    var _newExtenderSOloEstriboDto = new ExtenderSOloEstriboDto(item, _DatosHost);
                    _ListaExtenderSOloEstriboDto.Add(_newExtenderSOloEstriboDto);
                }

                var listagrupoIgualHost = _ListaExtenderSOloEstriboDto.GroupBy(c => new { idHost = c.idHostEleme });

                foreach (var itemGrup in listagrupoIgualHost)
                {

                    List<ExtenderSOloEstriboDto> _ListExtenderSOloEstriboDto = itemGrup.OrderBy(c=> c._ptoInicial.Z).ToList();
                    double idHost = itemGrup.Key.idHost;
                    DatosHost _DatosHost= _ListExtenderSOloEstriboDto[0].DatosHost;

                    PlanarFace ZinicilHost = _DatosHost.host.ObtenerCaraSegun_Direccion(new XYZ(0, 0, -1));
                    double Zmin = ZinicilHost.Origin.Z;
                    PlanarFace ZMAxHost = _DatosHost.host.ObtenerCaraSegun_Direccion(new XYZ(0, 0, 1));
                    double Zmax = ZMAxHost.Origin.Z;

                    if (_ListExtenderSOloEstriboDto.Count ==0) continue;
                    _ListExtenderSOloEstriboDto.First()._RebarDesglose_GrupoBarras_V._ptoInicial = _ListExtenderSOloEstriboDto.First()._ptoInicial.AsignarZ(Zmin);
                    _ListExtenderSOloEstriboDto.Last()._RebarDesglose_GrupoBarras_V._ptoInicial = _ListExtenderSOloEstriboDto.Last()._ptoInicial.AsignarZ(Zmax);

                    if (_ListExtenderSOloEstriboDto.Count < 2) continue;
                    for (int j = 0; j < _ListExtenderSOloEstriboDto.Count-1; j++)
                    {
                        double diferencia = _ListExtenderSOloEstriboDto[j+1]._ptoInicial.Z - _ListExtenderSOloEstriboDto[j]._ptoFinal.Z;
                        if (diferencia < Util.CmToFoot(30))
                        {
                            double Zintermedio = (_ListExtenderSOloEstriboDto[j+1]._ptoInicial.Z + _ListExtenderSOloEstriboDto[j]._ptoFinal.Z) / 2;
                            _ListExtenderSOloEstriboDto[j + 1]._RebarDesglose_GrupoBarras_V._ptoInicial= _ListExtenderSOloEstriboDto[j+1]._ptoInicial.AsignarZ(Zintermedio);
                            _ListExtenderSOloEstriboDto[j]._RebarDesglose_GrupoBarras_V._ptoFinal=_ListExtenderSOloEstriboDto[j]._ptoFinal.AsignarZ(Zintermedio);
                        }
                    }

                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
    }
}