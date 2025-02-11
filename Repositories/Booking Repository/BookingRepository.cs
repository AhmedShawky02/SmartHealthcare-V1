using Microsoft.EntityFrameworkCore;
using SmartHealthcare.Context;
using SmartHealthcare.Interfaces.Booking_Interface;
using SmartHealthcare.Models;

namespace SmartHealthcare.Repositories.Booking_Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HealthcareDbContext _context;

        public BookingRepository(HealthcareDbContext context)
        {
            _context = context;
        }
        public async Task<BookingDoctor> CreateBookingForDoctorAsync(BookingDoctor bookingDoctor)
        {
            await _context.BookingDoctors.AddAsync(bookingDoctor);
            await _context.SaveChangesAsync();
            return bookingDoctor;
        }
        public async Task<List<BookingDoctor>> GetAllBookingsForDoctor()
        {
            return await _context.BookingDoctors.ToListAsync();

        }

        public async Task<BookingNurse> CreateBookingForNurseAsync(BookingNurse bookingNurse)
        {
            await _context.BookingNurses.AddAsync(bookingNurse);
            await _context.SaveChangesAsync();
            return bookingNurse;
        }


        public async Task<List<BookingNurse>> GetAllBookingsForNurse()
        {
            return await _context.BookingNurses.ToListAsync();
        }
    }
}
