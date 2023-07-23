using Secure_Api_Jwt.Models;

namespace Secure_Api_Jwt.Services
{
    public interface IReserveService
    {
        public Task<IEnumerable<reservation>> GetReservationsAsync();
        public Task<reservation> GetReservationAsync(int id);
        public Task<string> ReserveAsync(reservation newreservation);
        public Task<string> UpdateReservation(reservation newreservation);
        public Task<string> DeleteReservation(int id);


    }
}
