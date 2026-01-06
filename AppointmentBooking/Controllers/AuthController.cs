using Microsoft.AspNetCore.Mvc;
using AppointmentBooking.DTOs;
using AppointmentBooking.Services;

namespace AppointmentBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IUserService userService, ILogger<AuthController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            _logger.LogInformation($"Login attempt for email: {request.Email}");
            var response = await _userService.LoginAsync(request);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning($"Login failed: {ex.Message}");
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Login error: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> Register([FromBody] LoginRequest request)
    {
        try
        {
            _logger.LogInformation($"Register attempt for email: {request.Email}");
            await _userService.RegisterAsync(request.Email, request.Password, "User");
            var response = await _userService.LoginAsync(request);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Register failed: {ex.Message}");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Register error: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }
}
