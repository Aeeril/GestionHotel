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
builder.Services.AddControllers(); // 👈 Important pour activer les [ApiController]
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
app.UseAuthorization(); // Si tu prévois d'ajouter de la sécurité

// Controller endpoints (comme ChambresController, etc.)
app.MapControllers(); // 👈 Active les routes des contrôleurs

// Endpoint direct (comme POST /api/reservations)
app.MapPost("/api/reservations", async (
    ReservationService service,
    GestionHotel.Application.DTOs.ReservationRequestDto dto) =>
{
    var result = await service.ReserverAsync(dto);
    return Results.Ok(result);
});

app.Run();

