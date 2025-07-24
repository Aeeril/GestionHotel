using GestionHotel.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionHotel.Apis.Endpoints.Menage
{
    public static class MenageEndpoints
    {
        public static void MapMenageEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/menage").WithTags("Ménage")
                .RequireAuthorization(new AuthorizeAttribute { Roles = "PersonnelMenage" });

            group.MapGet("/chambres-a-nettoyer", async (IMenageService menageService) =>
            {
                var chambres = await menageService.GetChambresANettoyerAsync();
                return Results.Ok(chambres);
            })
            .WithName("GetChambresANettoyer")
            .Produces(200);

            group.MapPost("/chambres/{id}/nettoyee", async (int id, IMenageService menageService) =>
            {
                await menageService.MarquerChambreCommeNettoyeeAsync(id);
                return Results.NoContent();
            })
            .WithName("MarquerChambreCommeNettoyee")
            .Produces(204);

            group.MapPost("/chambres/{id}/dommage", async (int id, [FromBody] string description, IMenageService menageService) =>
            {
                await menageService.SignalerDommageAsync(id, description);
                return Results.NoContent();
            })
            .WithName("SignalerDommage")
            .Produces(204);
        }
    }
}
