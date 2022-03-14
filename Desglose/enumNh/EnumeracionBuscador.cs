
namespace Desglose.Ayuda
{
   public class EnumeracionBuscador
    {
        public static T ObtenerEnumGenerico<T>(T valor, string v)
        {
            T temp = valor;
            T result = (T)System.Enum.Parse(typeof(T), v);
            return result;
        }
    }
}
