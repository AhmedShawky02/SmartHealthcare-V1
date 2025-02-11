using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHealthcare.Dtos.Booking
{
    public class BookingDoctorCreateDto
    {
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int DoctorId { get; set; }
    }
}
