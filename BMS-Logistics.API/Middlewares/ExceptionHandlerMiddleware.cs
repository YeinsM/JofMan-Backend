using BMS_Logistics.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace BMS_Logistics.API.Middlewares
{
    /// <summary>
    /// Middleware encargado de interceptar TODAS las excepciones
    /// y devolver un mensaje de error formateado en JSON.
    /// </summary>
    public class ExceptionHandlerMiddleware : IMiddleware
    {
        /// <summary>
        /// Método principal del middleware.
        /// Se ejecuta antes de entrar al controlador y captura cualquier excepción.
        /// </summary>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                // Ejecuta la solicitud normalmente
                await next(context);
            }
            catch (Exception e)
            {
                // Cada respuesta que manejamos será JSON
                context.Response.ContentType = "application/json";

                // Procesa la excepción detectando su tipo
                await HandleExceptionAsync(context, e);
            }

        }

        /// <summary>
        /// Método encargado de analizar qué tipo de excepción ocurrió
        /// y devolver el código HTTP adecuado.
        /// </summary>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // 1. NOT FOUND (404)
            if (exception is NotFoundException)
            {
                await WriteResponseAsync(context,
                    HttpStatusCode.NotFound,
                    "NotFound",
                    exception.Message);

                return;
            }

            // 2. BAD REQUEST (400)
            if (exception is BadRequestException)
            {
                await WriteResponseAsync(context,
                    HttpStatusCode.BadRequest,
                    "BadRequest",
                    exception.Message);

                return;
            }

            // 3. VALIDATION ERROR (400)
            if (exception is ValidationException vex)
            {
                await WriteResponseAsync(context,
                    HttpStatusCode.BadRequest,
                    "ValidationError",
                    JsonSerializer.Serialize(vex.Errors));

                return;
            }

            // 4. UNAUTHORIZED (401)
            if (exception is UnauthorizedException)
            {
                await WriteResponseAsync(context,
                    HttpStatusCode.Unauthorized,
                    "Unauthorized",
                    exception.Message);

                return;
            }

            // 5. FORBIDDEN (403)
            if (exception is ForbiddenException)
            {
                await WriteResponseAsync(context,
                    HttpStatusCode.Forbidden,
                    "Forbidden",
                    exception.Message);

                return;
            }


            // 6. CONFLICT (409)            
            if (exception is ConflictException)
            {
                await WriteResponseAsync(context,
                    HttpStatusCode.Conflict,
                    "Conflict",
                    exception.Message);

                return;
            }


            // 7. UNPROCESSABLE ENTITY (422)
            if (exception is UnprocessableEntityException)
            {
                await WriteResponseAsync(context,
                    HttpStatusCode.UnprocessableEntity,
                    "UnprocessableEntity",
                    exception.Message);

                return;
            }

            // 8. ERROR conocido: Base64 inválido → BadRequest (400)           
            if (exception.Message.Contains("The input is not a valid Base-64 string"))
            {
                await WriteResponseAsync(context,
                    HttpStatusCode.BadRequest,
                    "BadRequest",
                    "Los datos ingresados no son válidos.");

                return;
            }

            // 9. CUALQUIER OTRA EXCEPCIÓN → InternalServerError (500)          
            await WriteResponseAsync(context,
                HttpStatusCode.InternalServerError,
                "ServerError",
                $"Ha ocurrido un error interno en el servidor: {exception.Message}");
        }

        /// <summary>
        /// Método reutilizable que escribe la respuesta en formato ProblemDetails.
        /// </summary>
        /// 
        private async Task WriteResponseAsync(HttpContext context, HttpStatusCode httpStatusCode, string type, string detail)
        {
            context.Response.StatusCode = (int)httpStatusCode;

            var problem = new ProblemDetails
            {
                Status = (int)httpStatusCode,
                Type = type,
                Detail = detail
            };

            string json = JsonSerializer.Serialize(problem);
            await context.Response.WriteAsync(json);
        }
    }
}
