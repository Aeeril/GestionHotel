using GestionHotel.Application.DTOs;
using GestionHotel.Application.Interfaces;
using GestionHotel.Domain.Entities;
using GestionHotel.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionHotel.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepo;
        private readonly IChambreRepository _chambreRepo;
        private readonly IClientRepository _clientRepo;

        public ReservationService(
            IReservationRepository reservationRepo,
            IChambreRepository chambreRepo,
            IClientRepository clientRepo)
        {
            _reservationRepo = reservationRepo;
            _chambreRepo = chambreRepo;
            _clientRepo = clientRepo;
        }

        public async Task<ReservationResultDto> ReserverAsync(ReservationRequestDto dto)
        {
            var client = await _clientRepo.GetByIdAsync(dto.ClientId);
            if (client == null)
                return new ReservationResultDto { EstValide = false, Message = "Client introuvable" };

            var chambres = new List<Chambre>();
            foreach (var id in dto.ChambreIds)
            {
                var chambre = await _chambreRepo.GetByIdAsync(id);
                if (chambre == null)
                    continue;

                // Vérification de disponibilité
                var reservationsExistantes = await _reservationRepo
                    .GetReservationsParChambreEtPeriode(id, dto.DateDebut, dto.DateFin);

                if (!reservationsExistantes.Any())
                    chambres.Add(chambre);
            }

            if (!chambres.Any())
                return new ReservationResultDto { EstValide = false, Message = "Aucune chambre disponible sur cette période" };

            var reservation = new Reservation
            {
                ClientId = client.Id,
                Client = client,
                Chambres = chambres,
                DateDebut = dto.DateDebut,
                DateFin = dto.DateFin,
                Paiement = new Paiement
                {
                    NumeroCarte = dto.NumeroCarte,
                    Montant = chambres.Sum(c => c.Tarif),
                    EstEffectue = true,
                    DatePaiement = DateTime.Now
                },
                Statut = StatutReservation.Confirmee
            };

            await _reservationRepo.AddAsync(reservation);

            return new ReservationResultDto
            {
                EstValide = true,
                ReservationId = reservation.Id,
                Message = "Réservation confirmée"
            };
        }

        public async Task<bool> AnnulerReservationAsync(int reservationId)
        {
            var reservation = await _reservationRepo.GetByIdAsync(reservationId);
            if (reservation == null)
                return false;

            var heuresAvantDebut = (reservation.DateDebut - DateTime.Now).TotalHours;

            // Règle de remboursement
            if (heuresAvantDebut < 48)
                return false; // non remboursable automatiquement

            reservation.Statut = StatutReservation.Annulee;
            await _reservationRepo.UpdateAsync(reservation);

            return true;
        }
    }
}
