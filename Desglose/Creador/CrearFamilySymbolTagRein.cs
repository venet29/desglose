
using Autodesk.Revit.DB;
using Desglose.Ayuda;
using Desglose.BuscarTipos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Creador
{
    class JtFamilyLoadOptions : IFamilyLoadOptions
    {
        public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
        {
            overwriteParameterValues = true;
            return true;
        }

        public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
        {
            source = FamilySource.Family;
            overwriteParameterValues = true;
            return true;
        }
    }
    public class CrearFamilySymbolTagRein
    {

        private Document _doc;
        private UbicacionLosa _ubicacionEnlosa;
        private string _TipoBarra;
        private object _LargoMin_1;
        private double _EspesorLosa_1;

        public object LargoPathreiforment { get; }

        public CrearFamilySymbolTagRein(Document doc)
        {
            this._doc = doc;

        }
 


        public Element ObtenerLetraGirada(string nombreIndependentTagPath_modif, string nombreFamiliaGEnerica_, double AnguloGRado, int escala)
        {

            Element IndependentTagPath = null;
            try
            {
                Family fam = null;

                //Los buscar segum el nombre

                IndependentTagPath = TiposPathReinTags.M1_GetFamilySymbol_nh(nombreIndependentTagPath_modif, _doc);


                if (IndependentTagPath != null) return IndependentTagPath;

                //fam = ((FamilySymbol)IndependentTagPath).Family;
                fam = TiposPathReinTagsFamilia.M1_GetFamilySymbol_nh(nombreFamiliaGEnerica_, _doc);


                // si lo encuentra el modifca los parametros
                if (fam != null)
                {

                    //crea una copia de la la familianhs
                    string newNombreFamiliaRebarShape = CreaBuevoTipoIndependentTagPath_pathReinLetra(fam, _doc, nombreIndependentTagPath_modif, nombreFamiliaGEnerica_, Util.GradosToRadianes(AnguloGRado), escala);
                    // rebarshape = ChangeFamilyRebar.SetDimensionRebarShape1(fam, doc, dimBarras_, nombreFamiliaRebarShape, factor / largobaa, true);
                    IndependentTagPath = TiposPathReinTags.M1_GetFamilySymbol_nh(nombreIndependentTagPath_modif, _doc);
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'ObtenerLetraGirada'  ex:{ex.Message}");
            }
            return IndependentTagPath;
        }


        public static string CreaBuevoTipoIndependentTagPath_pathReinLetra(Family f, Document rvtDoc, string nombreIndependentTagPath_modif, string NuevoNombreFamiliaGenerica, double Angulo, int escala)
        {
            if (f == null) return NuevoNombreFamiliaGenerica;
            Document famdoc;

            string parametro = "TagAngle";

            if (f.IsEditable && f.Name == NuevoNombreFamiliaGenerica)
            {
                famdoc = rvtDoc.EditFamily(f);

                if (null != famdoc)
                {
                    try
                    {
                        using (Transaction tranew = new Transaction(famdoc))
                        {
                            tranew.Start("TagAngle-NH");
                            FamilyManager familyManager = famdoc.FamilyManager;
                            FamilyType newFamilyType = familyManager.NewType(nombreIndependentTagPath_modif);

                            FamilyParameter familyParam = familyManager.get_Parameter(parametro);
                            if (null != familyParam) familyManager.Set(familyParam, -Angulo);

                            string parametro100 = "visible_100";
                            FamilyParameter familyParam100 = familyManager.get_Parameter(parametro100);
                            if (null != familyParam100) familyManager.Set(familyParam100, (escala == 100 ? 1 : 0));

                            string parametro75 = "visible_75";
                            FamilyParameter familyParam75 = familyManager.get_Parameter(parametro75);
                            if (null != familyParam75) familyManager.Set(familyParam75, (escala == 75 ? 1 : 0));

                            string parametro50 = "visible_50";
                            FamilyParameter familyParam50 = familyManager.get_Parameter(parametro50);
                            if (null != familyParam50) familyManager.Set(familyParam50, (escala == 50 ? 1 : 0));

                            tranew.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.ErrorMsg($"  EX:{ex.Message}");
                    }
                    IFamilyLoadOptions opt = new JtFamilyLoadOptions();
                    Family f2 = famdoc.LoadFamily(rvtDoc, opt);
                    //ParameterUtil.FindParaByName(f2, "Label");
                }
            }



            return NuevoNombreFamiliaGenerica;

        }


    }
}
