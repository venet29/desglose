using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Desglose.Ayuda;
using Desglose.BuscarTipos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Familias
{
    public class ManejadorCargarFAmilias
    {
        #region 0)propiedades            
        string rutaRaiz = Desglose.Ayuda.InfoSystema.ObtenerRutaDeFamilias();
        // public Dictionary<string, string> listaImagenes = new Dictionary<string, string>(20);
        List<Tuple<string, string>> listaRutasFamilias = new List<Tuple<string, string>>();
        public Dictionary<string, string> listaImagenes = new Dictionary<string, string>(20);
        private  bool IsRecargar;

        public Document doc { get; set; }
        #endregion

        #region 1)constructor

        public ManejadorCargarFAmilias(UIApplication uidoc, bool IsRecargar=false)
        {
            this.doc = uidoc.ActiveUIDocument.Document;
            rutaRaiz = rutaRaiz + AgregarVErsion(uidoc.Application.VersionNumber);
            this.IsRecargar = IsRecargar;
        }

        private string AgregarVErsion(string _VerSIon)
        {
            //dejar todo en la misma carpeta
            return @"\";

            if (_VerSIon.Contains("2018")) return @"\18\";

            if (_VerSIon.Contains("2019")) return @"\19\";

            if (_VerSIon.Contains("2020")) return @"\20\";

            if (_VerSIon.Contains("2021")) return @"\21\";

            if (_VerSIon.Contains("2022")) return @"\22\";
            return "";
        }
        #endregion

        #region 2)metodos 

        public void cargarFamilias_run()
        {
            if (!Directory.Exists(ConstNH.CONST_COT)) return;


            listaRutasFamilias = FactoryCargarFamilias.CrearDiccionarioRutasFamilias(rutaRaiz);
            CargarFamilias(doc, listaRutasFamilias);
        }

        private void CargarFamilias(Document doc, List<Tuple<string, string>> listafami)
        {
            try
            {
                using (Transaction trans = new Transaction(doc))
                {
                    trans.Start("CargarFamilias-NH");

                    //recorrer diccionario con familias
                    foreach (Tuple<string, string> NombreFamilia in listafami)
                    {
                        //buscar si existe familia
                        Family fam = null;
                        string NombreFAmilia = NombreFamilia.Item1;
                        string RutaFAmilis = NombreFamilia.Item2;
                        fam = TiposFamilyRebar.getFamilyRebarShape(NombreFAmilia, doc);

                        // Element elem = FiltroGetFamilySymbolByName.FindElementByName(doc, typeof(Family), NombreFamilia.Key);
                        //si no encuentra familia cargarla
                        if (fam == null && File.Exists(RutaFAmilis))
                        {
                            doc.LoadFamily(RutaFAmilis, out fam);
                        }
                        else if (IsRecargar && File.Exists(RutaFAmilis))
                        {
                            doc.LoadFamily(RutaFAmilis, out fam);
                        }
                    }

                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;

                return;
            }

        }





        #endregion
    }



}
