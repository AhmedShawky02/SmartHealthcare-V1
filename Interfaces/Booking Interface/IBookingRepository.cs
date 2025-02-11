using SmartHealthcare.Models;

namespace SmartHealthcare.Interfaces.Booking_Interface
{
    public interface IBookingRepository
    {
        Task<List<BookingDoctor>> GetAllBookingsForDoctor();
        Task<BookingDoctor> CreateBookingForDoctorAsync(BookingDoctor bookingDoctor);

        Task<List<BookingNurse>> GetAllBookingsForNurse();
        Task<BookingNurse> CreateBookingForNurseAsync(BookingNurse bookingDoctor);
    }
}
