using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHealthcare.Dtos.DepartmentsDto;
using SmartHealthcare.Interfaces.Departments_Interface;
using SmartHealthcare.Mapping;
using SmartHealthcare.Models;

namespace SmartHealthcare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepo;

        public DepartmentsController(IDepartmentRepository departmentRepo)
        {
           _departmentRepo = departmentRepo;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllDepartments()
        {
            try
            {
                var departments = await _departmentRepo.GetAllDepartmentsAsync();

                if (!departments.Any())
                {
                    return NotFound("No Departments found.");
                }

                var departmentsDto = departments.ToDepartmentDtoConversion();
                return Ok(departmentsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById([FromRoute] int id)
        {
            try
            {
                var department = await _departmentRepo.GetDepartmentById(id);

                if (department == null)
                {
                    return NotFound("Department not found.");
                }

                var departmentDto = department.ToDepartmentDtoConversion();

                return Ok(departmentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateDepartment([FromForm] CreateDepartmentDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var departmentName = await _departmentRepo.GetDepartmentByName(createDto.name!);

                if (departmentName != null)
                {
                    return BadRequest("Department name already exists.");
                }

                string? filePath = null;

                if (createDto.Picture != null)
                {
                    var fileName = $"{Guid.NewGuid()}_{createDto.Picture.FileName}";

                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Pictures");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    filePath = Path.Combine("Pictures", fileName);

                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await createDto.Picture.CopyToAsync(stream);
                    }

                }

                var department = new Department()
                {
                    Name = createDto.name!,
                    Picture = filePath
                };

                await _departmentRepo.CreateDepartmentAsync(department);
                return CreatedAtAction(nameof(GetDepartmentById), new { id = department.DepartmentId },
                    new { id = department.DepartmentId, message = "Medical center added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
