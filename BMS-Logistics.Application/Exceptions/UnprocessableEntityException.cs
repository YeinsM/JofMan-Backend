namespace BMS_Logistics.Application.Exceptions
{
    /// <summary>
    /// Se lanza cuando el servidor entiende la solicitud, pero no puede procesarla por errores de negocio.
    /// </summary>
    public class UnprocessableEntityException : Exception
    {
        public UnprocessableEntityException(string message) : base(message) { }
    }
}
