using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHealthcare.Dtos.DoctorsDto;
using SmartHealthcare.Dtos.NursesDto;
using SmartHealthcare.Interfaces.MedicalCenters_Interface;
using SmartHealthcare.Interfaces.Nurses_Interface;
using SmartHealthcare.Mapping;
using SmartHealthcare.Models;

namespace SmartHealthcare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NursesController : ControllerBase
    {
        private readonly INurseRepository _nurseRepo;
        private readonly IMedicalCenterRepository _medicalCenterRepo;

        public NursesController(INurseRepository nurseRepo , IMedicalCenterRepository medicalCenterRepo)
        {
            _nurseRepo = nurseRepo;
            _medicalCenterRepo = medicalCenterRepo;
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllNurses()
        {
            try
            {
                var nurses = await _nurseRepo.GetAllNursesAsync();

                if (!nurses.Any())
                {
                    return NotFound("No nurses found.");
                }

                var nursesDto = nurses.ToNurseDtoConversion();
                return Ok(nursesDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNurseById([FromRoute] int id)
        {
            try
            {
                var nurse = await _nurseRepo.GetNurseById(id);

                if (nurse == null)
                {
                    return NotFound("Nurse not found.");
                }

                var nurseDto = nurse.ToNurseDtoConversion();

                return Ok(nurseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateNurse([FromForm] NurseCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var medicalCenter = await _medicalCenterRepo.GetmedicalCenterById(createDto.CenterId);

                if (medicalCenter == null)
                {
                    return NotFound("Medical Center not found.");
                }

                string? filePath = null;

                if (createDto.ProfilePicture != null)
                {
                    var fileName = $"{Guid.NewGuid()}_{createDto.ProfilePicture.FileName}";

                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Nurses Picture");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    filePath = Path.Combine("Nurses Picture", fileName);

                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await createDto.ProfilePicture.CopyToAsync(stream);
                    }

                }

                var nurse = new Nurse()
                {
                    Name = createDto.Name,
                    Info = createDto.Info,
                    ProfilePicture = filePath,
                    Age = createDto.Age,
                    CenterId = createDto.CenterId,
                };

                await _nurseRepo.CreateNurseAsync(nurse);

                return CreatedAtAction(nameof(GetNurseById), new { id = nurse.NurseId },
                    new { id = nurse.NurseId, message = "Nurse added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
