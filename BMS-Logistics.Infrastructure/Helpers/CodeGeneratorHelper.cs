namespace BMS_Logistics.Infrastructure.Helpers
{
    public static class CodeGeneratorHelper
    {
        // <summary>
        /// Genera un codigo aleatorio cono una cadena (string).
        /// </summary>
        /// Devuelve una cadena de seis digitos representando un codigo aleatorio entre 100000 y 199999.
        public static string GenerateCode()
        {
            // Crea una instancia de la clase aleatoria (Random).
            Random random = new();

            // Genera un numero aleatorio entre 100000 y 200000.
            // Conviérte en una cadena con una longitud fija de seis dígitos.
            string Code = random.Next(100000, 200000).ToString("000000");

            // Retorna el codigo generado.
            return Code;
        }
    }
}
