namespace BMS_Logistics.Application.Exceptions
{
    /// <summary>
    /// Se lanza cuando la petición del usuario es inválida o está mal construida.
    /// </summary>
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }
}
