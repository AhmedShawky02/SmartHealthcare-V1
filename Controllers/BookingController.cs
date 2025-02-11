using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHealthcare.Dtos.Booking;
using SmartHealthcare.Dtos.DoctorsDto;
using SmartHealthcare.Interfaces.Booking_Interface;
using SmartHealthcare.Interfaces.Doctors_Interface;
using SmartHealthcare.Interfaces.Nurses_Interface;
using SmartHealthcare.Interfaces.Users_Interface;
using SmartHealthcare.Mapping;
using SmartHealthcare.Models;
using System.Numerics;

namespace SmartHealthcare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly IUserRepository _userRepo;
        private readonly IDoctorRepository _doctorRepo;
        private readonly INurseRepository _nurseRepo;

        public BookingController(IBookingRepository bookingRepo,
            IUserRepository userRepo,
            IDoctorRepository doctorRepo,
            INurseRepository nurseRepo)
        {
            _bookingRepo = bookingRepo;
            _userRepo = userRepo;
            _doctorRepo = doctorRepo;
            _nurseRepo = nurseRepo;
        }

        [HttpGet("GetAllBookingsForDoctor")]
        public async Task<IActionResult> GetAllBookingsForDoctor()
        {
            try
            {
                var bookings = await _bookingRepo.GetAllBookingsForDoctor();

                if (!bookings.Any())
                {
                    return NotFound("No bookings found.");
                }

                var bookingsDto = bookings.ToBookingsDtoConversion();
                return Ok(bookingsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("CreateBookingForDoctor")]
        public async Task<IActionResult> CreateBookingForDoctor([FromBody] BookingDoctorCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingUser = await _userRepo.GetUserById(createDto.UserId);
                if (existingUser == null)
                {
                    return NotFound("User not Found.");
                }

                var existingDoctor = await _doctorRepo.GetDoctorById(createDto.DoctorId);
                if (existingDoctor == null)
                {
                    return NotFound("Doctor not Found.");
                }

                if (createDto.Date <= DateTime.UtcNow)
                {
                    return BadRequest("Error: Cannot select a past date or today's date for booking.");
                }

                var booking = new BookingDoctor()
                {
                    Date = createDto.Date,
                    UserId = createDto.UserId,
                    DoctorId = createDto.DoctorId
                };

                await _bookingRepo.CreateBookingForDoctorAsync(booking);

                return StatusCode(201, new { id = booking.BookingDoctorId, message = "Booking added successfully." });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllBookingsForNurse")]
        public async Task<IActionResult> GetAllBookingsForNurse()
        {
            try
            {
                var bookings = await _bookingRepo.GetAllBookingsForNurse();

                if (!bookings.Any())
                {
                    return NotFound("No bookings found.");
                }

                var bookingsDto = bookings.ToBookingsDtoConversion();
                return Ok(bookingsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("CreateBookingForNurse")]
        public async Task<IActionResult> CreateBookingForNurse([FromBody] BookingNurseCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingUser = await _userRepo.GetUserById(createDto.UserId);
                if (existingUser == null)
                {
                    return NotFound("User not Found.");
                }

                var existingNurse = await _nurseRepo.GetNurseById(createDto.NurseId);
                if (existingNurse == null)
                {
                    return NotFound("Nurse not Found.");
                }

                if (createDto.Date <= DateTime.UtcNow)
                {
                    return BadRequest("Error: Cannot select a past date or today's date for booking.");
                }

                var booking = new BookingNurse()
                {
                    Date = createDto.Date,
                    UserId = createDto.UserId,
                    NurseId = createDto.NurseId,
                };

                await _bookingRepo.CreateBookingForNurseAsync(booking);

                return StatusCode(201, new { id = booking.BookingNurseId, message = "Booking added successfully." });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
