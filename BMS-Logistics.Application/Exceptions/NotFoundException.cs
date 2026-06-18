namespace BMS_Logistics.Application.Exceptions
{
    /// <summary>
    /// Se lanza cuando la petición del usuario no se encontró en la base de datos.
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string name) : base($"{name} no encontrado.") { }
    }
}
