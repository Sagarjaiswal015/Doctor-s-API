using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AppointmentBooking.DTOs;
using AppointmentBooking.Services;
using System.Security.Claims;

namespace AppointmentBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;
    private readonly IAppointmentSlotService _slotService;
    private readonly IAppointmentService _appointmentService;
    private readonly ILogger<DoctorsController> _logger;

    public DoctorsController(IDoctorService doctorService, IAppointmentSlotService slotService, IAppointmentService appointmentService, ILogger<DoctorsController> logger)
    {
        _doctorService = doctorService;
        _slotService = slotService;
        _appointmentService = appointmentService;
        _logger = logger;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<DoctorDto>> CreateDoctor([FromBody] CreateDoctorDto dto)
    {
        try
        {
            _logger.LogInformation($"Creating doctor: {dto.Email}");
            var doctor = await _doctorService.CreateDoctorAsync(dto);
            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, doctor);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating doctor: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<DoctorDto>>> GetAllDoctors()
    {
        try
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();
            return Ok(doctors);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching doctors: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DoctorDto>> GetDoctor(int id)
    {
        try
        {
            var doctor = await _doctorService.GetDoctorAsync(id);
            return Ok(doctor);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching doctor: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [Authorize(Roles = "Doctor")]
    [HttpGet("{id}/schedule")]
    public async Task<ActionResult<List<AppointmentSlotDto>>> GetDoctorSchedule(int id)
    {
        try
        {
            var slots = await _slotService.GetDoctorSlotsAsync(id);
            return Ok(slots);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching schedule: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [Authorize(Roles = "Doctor")]
    [HttpGet("{id}/appointments")]
    public async Task<ActionResult<List<AppointmentDto>>> GetDoctorAppointments(int id)
    {
        try
        {
            var appointments = await _appointmentService.GetDoctorAppointmentsAsync(id);
            return Ok(appointments);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching appointments: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }
}
