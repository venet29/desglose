﻿using Autodesk.Revit.DB;
using System;

namespace Desglose.Tag.TipoBarraH
{
    public class GeomeTagSinPataH : GeomeTagBaseH, IGeometriaTag
    {


        public GeomeTagSinPataH(Document doc, XYZ ptoIni, XYZ ptoFin, XYZ posiciontag) :
            base(doc, ptoIni, ptoFin, posiciontag)
        { }

        public GeomeTagSinPataH() { }

        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {

                double AnguloRadian = args.angulorad;
                M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian);
                M2_CAlcularPtosDeTAg();
                M3_DefinirRebarShape();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagSinPataH> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBaseH _geomeTagBase)
        {

            //_geomeTagBase.TagP0_A.IsOk = false;
            //_geomeTagBase.TagP0_B.IsOk = false;
            //_geomeTagBase.TagP0_D.IsOk = false;
            //_geomeTagBase.TagP0_E.IsOk = false;
            _geomeTagBase.TagP0_C.IsOk = false;
            // _geomeTagBase.TagP0_L.IsOk = false;
            //_geomeTagBase.TagP0_C.CAmbiar(_geomeTagBase.TagP0_A);
        }
    }
}