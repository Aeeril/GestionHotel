using GestionHotel.Application.Services;
using GestionHotel.Domain.Interfaces;
using GestionHotel.Infrastructure.Data;
using GestionHotel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

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

// Swagger + Controllers
builder.Services.AddControllers(); // 👈 Nécessaire pour activer [ApiController]
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Active les routes des contrôleurs
app.MapControllers();

// Endpoint minimal pour réserver
app.MapPost("/api/reservations", async (
    ReservationService service,
    GestionHotel.Application.DTOs.ReservationRequestDto dto) =>
{
    var result = await service.ReserverAsync(dto);
    return Results.Ok(result);
});

// Endpoint minimal pour annuler une réservation
app.MapPost("/api/reservations/annuler/{id:int}", async (
    ReservationService service,
    int id) =>
{
    await service.AnnulerReservationAsync(id, true, false);
    return Results.Ok("Réservation annulée.");
});

// Endpoint pour annuler une réservation en tant que réceptionniste
app.MapPost("/api/reservations/annuler-par-reception", async (
    ReservationService service,
    int reservationId,
    bool rembourser) =>
{
    await service.AnnulerReservationAsync(reservationId, demandeParClient: false, forcerRemboursement: rembourser);
    return Results.Ok("Réservation annulée par la réception.");
});

// Endpoint pour le check-in
app.MapPost("/api/reservations/checkin", async (
    ReservationService service,
    GestionHotel.Application.DTOs.CheckInRequestDto dto) =>
{
    await service.CheckInAsync(dto);
    return Results.Ok("Check-in effectué avec succès.");
});

// Endpoint pour le check-out
app.MapPost("/api/reservations/checkout", async (
    ReservationService service,
    GestionHotel.Application.DTOs.CheckOutRequestDto dto) =>
{
    await service.CheckOutAsync(dto.ReservationId);
    return Results.Ok("Check-out effectué avec succès.");
});

// Endpoint pour obtenir toutes les réservations actives
app.MapGet("/api/reservations/actives", async (
    ReservationService service) =>
{
    var actives = await service.GetReservationsActivesAsync();
    return Results.Ok(actives);
});

app.Run();
