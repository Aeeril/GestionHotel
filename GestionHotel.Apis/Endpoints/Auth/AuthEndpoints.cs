
using GestionHotel.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GestionHotel.Apis.Endpoints.Auth
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/auth").WithTags("Authentification");

            group.MapPost("/login", async (IAuthService authService, [FromBody] LoginRequest request) =>
            {
                try
                {
                    var token = await authService.Authenticate(request.Email, request.Password);
                    return Results.Ok(new { Token = token });
                }
                catch (UnauthorizedAccessException)
                {
                    return Results.Unauthorized();
                }
            })
            .WithName("Login")
            .Produces(200)
            .Produces(401);
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
