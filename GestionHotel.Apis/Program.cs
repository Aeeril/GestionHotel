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

app.Run();
