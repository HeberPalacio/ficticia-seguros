using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace FicticiaBackend.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Pasar al siguiente middleware/controlador
                await _next(context);
            }
            catch (Exception ex)
            {
                // Registrar el error
                _logger.LogError(ex, "Ocurrió un error no controlado");

                // Manejar la respuesta de error
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Configurar el tipo de respuesta
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                success = false,
                message = "Ocurrió un error inesperado. Por favor, intente nuevamente.",
                detail = ex.Message, // ⚠️ En producción puedes quitarlo
                statusCode = context.Response.StatusCode
            };

            var json = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(json);
        }
    }
}
