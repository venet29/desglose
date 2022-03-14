using Desglose.Tag;
using System.Collections.Generic;

namespace Desglose.Tag
{
    public interface IGeometriaTag
    {
        List<TagBarra> listaTag { get; set; }
    
        void M1_ObtnerPtosInicialYFinalDeBarra(double anguloRoomRad);
        void M2_CAlcularPtosDeTAg(bool IsGarficarEnForm = false);

        void M3_DefinirRebarShape();
        bool M4_IsFAmiliaValida();
       // void Ejecutar(double AnguloRadian);
        bool Ejecutar(GeomeTagArgs args);

    }

}
