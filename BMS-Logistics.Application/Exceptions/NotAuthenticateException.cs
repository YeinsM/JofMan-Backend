namespace BMS_Logistics.Application.Exceptions
{
    /// <summary>
    /// Se lanza cuando el usuario no está autenticado.
    /// </summary>
    public class NotAuthenticateException : Exception
    {
        public NotAuthenticateException() : base("Usuario no autenticado.") { }
    }
}
