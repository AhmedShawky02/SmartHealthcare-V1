using SmartHealthcare.Dtos.Booking;
using SmartHealthcare.Models;

namespace SmartHealthcare.Mapping
{
    public static class ToBookingsDto
    {
        public static BookingDtoForDoctor ToBookingsDtoConversion(this BookingDoctor bookingDoctor)
        {
            return new BookingDtoForDoctor()
            {
                BookingDoctorId = bookingDoctor.BookingDoctorId,
                Date = bookingDoctor.Date,
                UserId = bookingDoctor.UserId,
                DoctorId = bookingDoctor.DoctorId
            };
        }

        public static IEnumerable<BookingDtoForDoctor> ToBookingsDtoConversion(this IEnumerable<BookingDoctor> bookingDoctor)
        {
            return bookingDoctor.Select(booking => booking.ToBookingsDtoConversion());
        }

        public static BookingDtoForNurse ToBookingsDtoConversion(this BookingNurse bookingNures)
        {
            return new BookingDtoForNurse()
            {
                BookingNurseId = bookingNures.BookingNurseId,
                Date = bookingNures.Date,
                UserId = bookingNures.UserId,
                NurseId = bookingNures.NurseId
            };
        }

        public static IEnumerable<BookingDtoForNurse> ToBookingsDtoConversion(this IEnumerable<BookingNurse> bookingNures)
        {
            return bookingNures.Select(booking => booking.ToBookingsDtoConversion());
        }

    }
}
