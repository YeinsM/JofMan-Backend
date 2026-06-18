namespace BMS_Logistics.Application.Exceptions
{
    /// <summary>
    /// Se lanza cuando un modelo o DTO no pasa las validaciones correspondientes.
    /// </summary>
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(IDictionary<string, string[]> errors)
            : base("El modelo enviado contiene errores de validación.")
        {
            Errors = errors;
        }
    }
}
