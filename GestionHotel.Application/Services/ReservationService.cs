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

        public async Task AnnulerReservationAsync(int reservationId, bool demandeParClient, bool forcerRemboursement)
        {
            var reservation = await _reservationRepo.GetByIdAsync(reservationId);

            if (reservation == null)
                throw new Exception("Réservation introuvable.");

            var delaiAnnulation = (reservation.DateDebut - DateTime.Now).TotalHours;

            var remboursable = forcerRemboursement || (demandeParClient && delaiAnnulation >= 48);

            reservation.Statut = StatutReservation.Annulee;
            await _reservationRepo.UpdateAsync(reservation);
        }

        public async Task CheckInAsync(CheckInRequestDto dto)
        {
            var reservation = await _reservationRepo.GetByIdAsync(dto.ReservationId);
            if (reservation == null)
                throw new Exception("Réservation introuvable.");

            reservation.EstCheckIn = true;
            reservation.PaiementEffectue = dto.PaiementEffectue;

            await _reservationRepo.UpdateAsync(reservation);
        }

        public async Task CheckOutAsync(int reservationId)
        {
            var reservation = await _reservationRepo.GetByIdAsync(reservationId);
            if (reservation == null)
                throw new Exception("Réservation introuvable.");

            foreach (var chambre in reservation.Chambres)
            {
                chambre.Etat = EtatChambre.ARefaire;
                chambre.EstPropre = false;
                await _chambreRepo.UpdateAsync(chambre);
            }

            if (!reservation.PaiementEffectue)
            {
                reservation.Paiement ??= new Paiement();
                reservation.Paiement.Montant = reservation.Chambres.Sum(c => c.Tarif);
                reservation.Paiement.DatePaiement = DateTime.Now;
                reservation.Paiement.EstEffectue = true;
                reservation.PaiementEffectue = true;
            }

            reservation.Statut = StatutReservation.Terminee;

            await _reservationRepo.UpdateAsync(reservation);
        }

        public async Task<List<Reservation>> GetReservationsActivesAsync()
        {
            var aujourdHui = DateTime.Today;

            var toutes = await _reservationRepo.GetAllAsync();

            return toutes
                .Where(r =>
                    (r.Statut == StatutReservation.EnCours || r.Statut == StatutReservation.Confirmee) &&
                    r.DateDebut.Date <= aujourdHui &&
                    r.DateFin.Date >= aujourdHui)
                .ToList();
        }
    }
}
