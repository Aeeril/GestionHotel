using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GestionHotel.Domain.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionHotel.Application.Services
{
    public class NotificationPostSejourService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly EmailService _emailService;
        private readonly ILogger<NotificationPostSejourService> _logger;

        public NotificationPostSejourService(IServiceScopeFactory scopeFactory, ILogger<NotificationPostSejourService> logger)
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
                var reservationRepo = scope.ServiceProvider.GetRequiredService<IReservationRepository>();

                var demain = DateTime.Today.AddDays(1); // Pour test : var aujourdHui = DateTime.Today; 

                var reservations = await reservationRepo.GetAllAsync();
                var cibles = reservations
                    .Where(r => r.DateFin.Date == demain && r.Client is not null && !string.IsNullOrWhiteSpace(r.Client.Email)) // pour test : .Where(r => r.DateFin.Date == aujourdHui && r.Client != null && !string.IsNullOrEmpty(r.Client.Email)) 
                    .ToList();

                foreach (var res in cibles)
                {
                    try
                    {
                        var client = res.Client;
                        if (client is null || string.IsNullOrWhiteSpace(client.Email)) continue;

                        await _emailService.SendEmailAsync(
                            client.Email,
                            "Merci pour votre séjour !",
                            $"Bonjour {client.Nom ?? "client"},\n\nMerci d’avoir séjourné chez nous ! Nous espérons que tout s’est bien passé. N’hésitez pas à nous donner votre avis :)\n\nÀ bientôt !"
                        );

                        _logger.LogInformation($"Email post-séjour envoyé à {client.Email}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Email non envoyé : {ex.Message}"); // Mailtrap a une limite gratuite de 50 emails
                    }
                }
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // pour test : await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
        }
    }
}
