using GestionHotel.Apis.Endpoints.Menage;
using GestionHotel.Application.Interfaces;
using GestionHotel.Application.Services;
using GestionHotel.Domain.Interfaces;
using GestionHotel.Infrastructure.Data;
using GestionHotel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// EF Core + SQLite
builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// DI - Dépendances
builder.Services.AddScoped<IChambreRepository, ChambreRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<IMenageService, MenageService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Swagger + Controllers
builder.Services.AddControllers(); // 👈 Nécessaire pour activer [ApiController]
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();

// Active les routes des contrôleurs
app.MapControllers();

// Endpoint minimal pour réserver
app.MapPost("/api/reservations", async (
    ReservationService service,
    GestionHotel.Application.DTOs.ReservationRequestDto dto) =>
{
    var result = await service.ReserverAsync(dto);
    return Results.Ok(result);
}).RequireAuthorization(new AuthorizeAttribute { Roles = "Client" });

// Endpoint minimal pour annuler une réservation
app.MapPost("/api/reservations/annuler/{id:int}", async (
    ReservationService service,
    int id) =>
{
    await service.AnnulerReservationAsync(id, true, false);
    return Results.Ok("Réservation annulée.");
}).RequireAuthorization(new AuthorizeAttribute { Roles = "Client" });

// Endpoint pour annuler une réservation en tant que réceptionniste
app.MapPost("/api/reservations/annuler-par-reception", async (
    ReservationService service,
    int reservationId,
    bool rembourser) =>
{
    await service.AnnulerReservationAsync(reservationId, demandeParClient: false, forcerRemboursement: rembourser);
    return Results.Ok("Réservation annulée par la réception.");
}).RequireAuthorization(new AuthorizeAttribute { Roles = "Receptionniste" });

// Endpoint pour le check-in
app.MapPost("/api/reservations/checkin", async (
    ReservationService service,
    GestionHotel.Application.DTOs.CheckInRequestDto dto) =>
{
    await service.CheckInAsync(dto);
    return Results.Ok("Check-in effectué avec succès.");
}).RequireAuthorization(new AuthorizeAttribute { Roles = "Receptionniste" });

// Endpoint pour le check-out
app.MapPost("/api/reservations/checkout", async (
    ReservationService service,
    GestionHotel.Application.DTOs.CheckOutRequestDto dto) =>
{
    await service.CheckOutAsync(dto.ReservationId);
    return Results.Ok("Check-out effectué avec succès.");
}).RequireAuthorization(new AuthorizeAttribute { Roles = "Receptionniste" });

// Endpoint pour obtenir toutes les réservations actives
app.MapGet("/api/reservations/actives", async (
    ReservationService service) =>
{
    var actives = await service.GetReservationsActivesAsync();
    return Results.Ok(actives);
}).RequireAuthorization(new AuthorizeAttribute { Roles = "Receptionniste" });

app.MapMenageEndpoints();

// Ajout d'utilsateurs test
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
    if (!dbContext.Utilisateurs.Any())
    {
        dbContext.Utilisateurs.AddRange(
            new GestionHotel.Domain.Entities.Utilisateur
            {
                Nom = "receptionniste",
                Email = "receptionniste@test.com",
                MotDePasse = "test",
                Role = GestionHotel.Domain.Entities.RoleUtilisateur.Receptionniste
            },
            new GestionHotel.Domain.Entities.Utilisateur
            {
                Nom = "client",
                Email = "client@test.com",
                MotDePasse = "test",
                Role = GestionHotel.Domain.Entities.RoleUtilisateur.Client
            },
            new GestionHotel.Domain.Entities.Utilisateur
            {
                Nom = "menage",
                Email = "menage@test.com",
                MotDePasse = "test",
                Role = GestionHotel.Domain.Entities.RoleUtilisateur.PersonnelMenage
            }
        );
        dbContext.SaveChanges();
    }
}

app.MapPost("/api/auth/login", async (IAuthService authService, string email, string password) =>
{
    try
    {
        var token = await authService.Authenticate(email, password);
        return Results.Ok(new { Token = token });
    }
    catch (UnauthorizedAccessException)
    {
        return Results.Unauthorized();
    }
});

app.Run();
