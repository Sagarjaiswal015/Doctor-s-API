using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AppointmentBooking.DTOs;
using AppointmentBooking.Services;
using System.Security.Claims;

namespace AppointmentBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly ILogger<AppointmentsController> _logger;

    public AppointmentsController(IAppointmentService appointmentService, ILogger<AppointmentsController> logger)
    {
        _appointmentService = appointmentService;
        _logger = logger;
    }

    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> BookAppointment([FromBody] BookAppointmentDto dto)
    {
        try
        {
            var userIdClaim = User.FindFirst("sub");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized(new { message = "Invalid token" });

            _logger.LogInformation($"Booking appointment for user: {userId}");
            var appointment = await _appointmentService.BookAppointmentAsync(userId, dto);
            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error booking appointment: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentDto>> GetAppointment(int id)
    {
        try
        {
            var appointment = await _appointmentService.GetAppointmentAsync(id);
            return Ok(appointment);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching appointment: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [Authorize(Roles = "User")]
    [HttpGet("my-appointments")]
    public async Task<ActionResult<List<AppointmentDto>>> GetMyAppointments()
    {
        try
        {
            var userIdClaim = User.FindFirst("sub");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized(new { message = "Invalid token" });

            var appointments = await _appointmentService.GetUserAppointmentsAsync(userId);
            return Ok(appointments);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching user appointments: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [Authorize(Roles = "User")]
    [HttpPost("{id}/cancel")]
    public async Task<ActionResult<AppointmentDto>> CancelAppointment(int id)
    {
        try
        {
            _logger.LogInformation($"Canceling appointment: {id}");
            var appointment = await _appointmentService.CancelAppointmentAsync(id);
            return Ok(appointment);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error canceling appointment: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }
}
