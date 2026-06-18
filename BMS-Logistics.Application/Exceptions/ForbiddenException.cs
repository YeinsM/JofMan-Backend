namespace BMS_Logistics.Application.Exceptions
{
    /// <summary>
    /// Se lanza cuando el usuario está autenticado pero no tiene permisos.
    /// </summary>
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message = "No tiene permisos para realizar esta acción.") : base(message) { }
    }
}
