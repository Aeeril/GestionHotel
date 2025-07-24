using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GestionHotel.Domain.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace GestionHotel.Application.Services
{
    public class NotificationPreSejourService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly EmailService _emailService;
        private readonly ILogger<NotificationPreSejourService> _logger;

        public NotificationPreSejourService(IServiceScopeFactory scopeFactory, ILogger<NotificationPreSejourService> logger)
        {
            _scopeFactory = scopeFactory;
            _emailService = new EmailService();
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var reservationRepository = scope.ServiceProvider.GetRequiredService<IReservationRepository>();

                var demain = DateTime.Today.AddDays(1); //Pour test : var maintenant = DateTime.Now; 

                var reservations = await reservationRepository.GetAllAsync();
                var cibles = reservations
                    .Where(r => r.DateDebut.Date == demain && r.Client != null && !string.IsNullOrEmpty(r.Client.Email)) //pour test : .Where(r => r.DateDebut <= maintenant.AddMinutes(1) && r.Client != null && !string.IsNullOrEmpty(r.Client.Email))
                    .ToList();

                foreach (var res in cibles)
                {
                    try
                    {
                        var client = res.Client;
                        if (client == null || string.IsNullOrWhiteSpace(client.Email)) continue;

                        await _emailService.SendEmailAsync(
                            client.Email,
                            "Rappel de votre séjour à l'hôtel",
                            $"Bonjour {client.Nom ?? "client"},\n\nNous vous rappelons que votre séjour commence demain ({res.DateDebut:dd/MM/yyyy}). À bientôt !"
                        );
                        _logger.LogInformation($"Rappel envoyé à {client.Email}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Email non envoyé : {ex.Message}");
                    }
                }
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Pour test : await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
        }
    }
}
