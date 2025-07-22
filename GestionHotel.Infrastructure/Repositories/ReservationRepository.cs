using GestionHotel.Domain.Entities;
using GestionHotel.Domain.Interfaces;
using GestionHotel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionHotel.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly HotelDbContext _context;

        public ReservationRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.Chambres)
                .Include(r => r.Paiement)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Reservation>> GetReservationsParChambreEtPeriode(int chambreId, DateTime dateDebut, DateTime dateFin)
        {
            return await _context.Reservations
                .Include(r => r.Chambres)
                .Where(r =>
                    r.Chambres.Any(c => c.Id == chambreId) &&
                    r.DateDebut < dateFin &&
                    r.DateFin > dateDebut &&
                    r.Statut != StatutReservation.Annulee)
                .ToListAsync();
        }

        public async Task AnnulerReservationAsync(int reservationId, bool demandeParClient, bool forcerRemboursement)
        {
            var reservation = await _context.Reservations.FindAsync(reservationId);

            if (reservation == null)
                throw new Exception("Réservation introuvable.");

            var delaiAnnulation = (reservation.DateDebut - DateTime.Now).TotalHours;

            var remboursable = forcerRemboursement || (demandeParClient && delaiAnnulation >= 48);

            // Logique simulée de remboursement

            reservation.Statut = StatutReservation.Annulee;
            await _context.SaveChangesAsync();
        }
    }
}
