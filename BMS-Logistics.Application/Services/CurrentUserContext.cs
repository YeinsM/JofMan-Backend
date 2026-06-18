using BMS_Logistics.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BMS_Logistics.Application.Services
{
    public class CurrentUserContext : ICurrentUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId
        {
            get
            {
                //var idClaim = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //return int.TryParse(idClaim, out var id) ? id : 0;

                // Retrieve the user's identity from the HttpContext
                var identity = _httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;

                if (identity == null || !identity.IsAuthenticated)
                    throw new UnauthorizedAccessException("Usuario no autenticado");

                // Get the “id” claim from the JWT
                var encryptedId = identity.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

                if (string.IsNullOrEmpty(encryptedId))
                    throw new UnauthorizedAccessException("Claim id no encontrado");


                if (!int.TryParse(encryptedId, out int userId))
                    throw new UnauthorizedAccessException("ID de usuario inválido");

                return userId;
            }
        }

        public string UserName
        {
            get
            {
                //return _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? string.Empty;

                // Retrieve the user's identity from the HttpContext
                var identity = _httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;

                if (identity == null || !identity.IsAuthenticated)
                    throw new UnauthorizedAccessException("Usuario no autenticado");

                // Get the “id” claim from the JWT
                var encryptedUsername = identity.Claims
                    .FirstOrDefault(c => c.Type == "username")?.Value;

                if (string.IsNullOrEmpty(encryptedUsername))
                    throw new UnauthorizedAccessException("Claim username no encontrado");


                return encryptedUsername;
            }
        }
    }
}
