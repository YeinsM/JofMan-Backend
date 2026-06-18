namespace BMS_Logistics.Application.Exceptions
{
    /// <summary>
    /// Se lanza cuando el usuario no está autenticado o el token es inválido.
    /// </summary>
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = "No está autorizado para acceder a este recurso.") : base(message) { }
    }
}
