using Microsoft.EntityFrameworkCore;
using Secure_Api_Jwt.Models;

namespace Secure_Api_Jwt.Services
{
    public class ReserveService : IReserveService
    {
        private readonly Hotel_RESERVATIONContext context;

        public ReserveService(Hotel_RESERVATIONContext context)
        {
            this.context = context;
        }
        public async Task<string> DeleteReservation(int id)
        {
            var reservation = await context.reservations.FindAsync(id);
            if (reservation == null) { return "Reservation is not available"; }
            else
            {
                context.reservations.Remove(reservation);
                try
                {
                    await context.SaveChangesAsync();
                    return string.Empty;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public async Task<reservation> GetReservationAsync(int id)
        {
            var reservation = await context.reservations.FindAsync(id);
            if (reservation == null)
            {
                return new reservation();
            }
            return reservation;
        }

        public async Task<IEnumerable<reservation>> GetReservationsAsync()
        {
            var reservations = await context.reservations.ToListAsync();
            if (reservations==null)
            {
                return new List<reservation>();
            }
            return reservations;
        }

        public async Task<string> ReserveAsync(reservation newreservation)
        {
            var reservation = await context.reservations.FindAsync(newreservation.Id);
            if (reservation !=null)
            {
                return "U Already reserved before";
            }
            else
            {
                await context.reservations.AddAsync(newreservation);
                try
                {
                    await context.SaveChangesAsync();
                    return string.Empty;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public async Task<string> UpdateReservation(reservation newreservation)
        {
            var reservation = await context.reservations.FindAsync(newreservation.Id);
            if (reservation == null)
            {
                return "cant find reservation";
            }
            else
            {
                context.Entry(newreservation).State = EntityState.Modified;
                try
                {
                    await context.SaveChangesAsync();
                    return string.Empty;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
