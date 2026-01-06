using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AppointmentBooking.DTOs;
using AppointmentBooking.Services;

namespace AppointmentBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SlotsController : ControllerBase
{
    private readonly IAppointmentSlotService _slotService;
    private readonly ILogger<SlotsController> _logger;

    public SlotsController(IAppointmentSlotService slotService, ILogger<SlotsController> logger)
    {
        _slotService = slotService;
        _logger = logger;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<AppointmentSlotDto>> CreateSlot([FromBody] CreateAppointmentSlotDto dto)
    {
        try
        {
            _logger.LogInformation($"Creating appointment slot for doctor: {dto.DoctorId}");
            var slot = await _slotService.CreateSlotAsync(dto);
            return CreatedAtAction(nameof(GetSlot), new { id = slot.Id }, slot);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating slot: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentSlotDto>> GetSlot(int id)
    {
        try
        {
            var slot = await _slotService.GetSlotAsync(id);
            return Ok(slot);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching slot: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("available")]
    public async Task<ActionResult<List<AppointmentSlotDto>>> GetAvailableSlots()
    {
        try
        {
            var slots = await _slotService.GetAvailableSlotsAsync();
            return Ok(slots);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching available slots: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("doctor/{doctorId}")]
    public async Task<ActionResult<List<AppointmentSlotDto>>> GetDoctorSlots(int doctorId)
    {
        try
        {
            var slots = await _slotService.GetDoctorSlotsAsync(doctorId);
            return Ok(slots);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching doctor slots: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }
}
