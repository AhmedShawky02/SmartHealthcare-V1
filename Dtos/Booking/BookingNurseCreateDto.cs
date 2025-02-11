using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHealthcare.Dtos.Booking
{
    public class BookingNurseCreateDto
    {
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int NurseId { get; set; }
    }
}
