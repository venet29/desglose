using Desglose.RebarLosa.DTO;
using Desglose.Ayuda;

namespace Desglose.Tag
{
    public  class GeomeTagArgs
    {
        public double angulorad { get; set; }
        public double diferenciaZInicialFinal { get; set; }
        public VectorDireccionLosaExternaInclinadaDTO _vectorDireccionLosaInicialExternaInclinadaDTO { get; set; }
        public VectorDireccionLosaExternaInclinadaDTO _vectorDireccionLosaFinalExternaInclinadaDTO { get; set; }



        public double deltaZ { get; set; }
        public double largoREcorridoDeltaZ { get; set; }

        //solo para refuelro losa -(tipo vigs)
        public string tipoPosicionEstribo { get; set; }
        public int _numeroTramosbarra { get; internal set; }
        public DireccionPata _ubicacionPata { get; internal set; }
    }
}
