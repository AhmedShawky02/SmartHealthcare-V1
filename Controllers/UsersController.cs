using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using SmartHealthcare.Dtos.UsersDto;
using SmartHealthcare.Interfaces.Helps_Interface;
using SmartHealthcare.Interfaces.Users_Interface;
using SmartHealthcare.Mapping;
using SmartHealthcare.Models;
using SmartHealthcare.Repositories.Helps_Repository;
using System.Security.Claims;

namespace SmartHealthcare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHashRepository _passwordHashRepo;
        private readonly ITokenRepository _tokenRepository;

        public UsersController(IUserRepository userRepo,
            IPasswordHashRepository passwordHashRepo,
            ITokenRepository tokenRepository)
        {
            _userRepo = userRepo;
            _passwordHashRepo = passwordHashRepo;
            _tokenRepository = tokenRepository;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> register([FromBody] UserCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userByName = await _userRepo.GetUserByName(createDto.Name!);
                var userByEmail = await _userRepo.GetUserByEmail(createDto.Email!);

                if (userByName != null)
                {
                    return BadRequest("The name is already taken.");
                }

                if (userByEmail != null)
                {
                    return BadRequest("Email already registered.");
                }

                var PasswordHash = _passwordHashRepo.Hash(createDto.Password!);

                if (createDto.Age <= 0)
                {
                    return BadRequest("Age must be greater than zero.");
                }

                if (createDto.Gender != 1 && createDto.Gender != 0)
                {
                    return BadRequest("Gender must be 0 or 1.");
                }

                var user = new User()
                {
                    Name = createDto.Name!,
                    Email = createDto.Email!,
                    Password = PasswordHash,
                    Age = createDto.Age,
                    Gender = createDto.Gender,
                    ForgetToken = null
                };

                await _userRepo.CreateUserAsync(user);

                return Ok(new
                {
                    UserId = user.UserId,
                    Message = "User registered successfully"
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> login([FromBody] UserLoginDto LoginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userRepo.GetUserByEmail(LoginDto.Email!);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var storedPasswordHash = user.Password;

                if (!_passwordHashRepo.Verified(storedPasswordHash, LoginDto.Password!))
                {
                    return BadRequest("Username or password is incorrect");
                }

                return Ok(new
                {
                    token = await _tokenRepository.CreateToken(user),
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("SendTokenToEmail")]
        public async Task<IActionResult> SendTokenToEmail([FromBody] string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest("Invalid email address.");
                }

                bool IsSend = await _userRepo.SendEmailAsync(email);

                if (!IsSend)
                {
                    return NotFound("There is an error here, try again.");
                }
                return Ok("Check your email");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (string.IsNullOrWhiteSpace(resetDto.Password) ||
                    string.IsNullOrWhiteSpace(resetDto.ConfirmPassword))
                {
                    return BadRequest("Password and ConfirmPassword are required.");
                }

                if (resetDto.Password != resetDto.ConfirmPassword)
                {
                    return BadRequest("Passwords do not match.");
                }

                var email = User.FindFirstValue(ClaimTypes.Email);

                if (string.IsNullOrEmpty(email))
                {
                    return Unauthorized("User email not found in token.");
                }

                var user = await _userRepo.GetUserByEmail(email);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                if (user.ForgetToken != resetDto.TokenOTP)
                {
                    return BadRequest("Invalid token.");
                }

                if (_passwordHashRepo.Verified(user.Password, resetDto.Password))
                {
                    return BadRequest("New password cannot be the same as the old password.");
                }

                var passwordHash = _passwordHashRepo.Hash(resetDto.Password);

                user.Password = passwordHash;
                user.ForgetToken = null;

                await _userRepo.UpdateUserAsync(user);

                return Ok(new { Message = "Password updated successfully." });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userRepo.GetAllUsersAsync();

                if (!users.Any())
                {
                    return NotFound("No users found."); 
                }

                var usersDto = users.ToUserDtoConversion();
                return Ok(usersDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            try
            {
                var user = await _userRepo.GetUserById(id);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var userDto = user.ToUserDtoConversion();

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
