namespace BMS_Logistics.Application.Exceptions
{
    /// <summary>
    /// Se lanza cuando una operación causa un conflicto, por ejemplo, un registro duplicado.
    /// </summary>
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}
