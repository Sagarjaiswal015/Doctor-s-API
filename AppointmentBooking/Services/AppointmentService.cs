using AppointmentBooking.DTOs;
using AppointmentBooking.Models;
using AppointmentBooking.Repositories;

namespace AppointmentBooking.Services;

public interface IAppointmentService
{
    Task<AppointmentDto> BookAppointmentAsync(int userId, BookAppointmentDto dto);
    Task<AppointmentDto> GetAppointmentAsync(int appointmentId);
    Task<List<AppointmentDto>> GetUserAppointmentsAsync(int userId);
    Task<List<AppointmentDto>> GetDoctorAppointmentsAsync(int doctorId);
    Task<AppointmentDto> CancelAppointmentAsync(int appointmentId);
}

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IAppointmentSlotRepository _slotRepository;
    private readonly IDoctorRepository _doctorRepository;

    public AppointmentService(IAppointmentRepository appointmentRepository, IAppointmentSlotRepository slotRepository, IDoctorRepository doctorRepository)
    {
        _appointmentRepository = appointmentRepository;
        _slotRepository = slotRepository;
        _doctorRepository = doctorRepository;
    }

    public async Task<AppointmentDto> BookAppointmentAsync(int userId, BookAppointmentDto dto)
    {
        var slot = await _slotRepository.GetByIdAsync(dto.AppointmentSlotId);
        if (slot == null)
            throw new KeyNotFoundException("Appointment slot not found");

        if (!slot.IsAvailable)
            throw new InvalidOperationException("No availability in this slot");

        var appointment = new Appointment
        {
            AppointmentSlotId = dto.AppointmentSlotId,
            DoctorId = slot.DoctorId,
            UserId = userId,
            PatientName = dto.PatientName,
            PatientPhone = dto.PatientPhone,
            PatientEmail = dto.PatientEmail,
            Reason = dto.Reason,
            Status = "Confirmed",
            BookedAt = DateTime.UtcNow
        };

        var createdAppointment = await _appointmentRepository.CreateAsync(appointment);

        slot.CurrentBookings++;
        await _slotRepository.UpdateAsync(slot);

        return await ToDto(createdAppointment);
    }

    public async Task<AppointmentDto> GetAppointmentAsync(int appointmentId)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
        if (appointment == null)
            throw new KeyNotFoundException("Appointment not found");

        return await ToDto(appointment);
    }

    public async Task<List<AppointmentDto>> GetUserAppointmentsAsync(int userId)
    {
        var appointments = await _appointmentRepository.GetByUserIdAsync(userId);
        var result = new List<AppointmentDto>();
        foreach (var app in appointments)
        {
            result.Add(await ToDto(app));
        }
        return result;
    }

    public async Task<List<AppointmentDto>> GetDoctorAppointmentsAsync(int doctorId)
    {
        var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctorId);
        var result = new List<AppointmentDto>();
        foreach (var app in appointments)
        {
            result.Add(await ToDto(app));
        }
        return result;
    }

    public async Task<AppointmentDto> CancelAppointmentAsync(int appointmentId)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
        if (appointment == null)
            throw new KeyNotFoundException("Appointment not found");

        appointment.Status = "Cancelled";
        var updated = await _appointmentRepository.UpdateAsync(appointment);

        var slot = await _slotRepository.GetByIdAsync(appointment.AppointmentSlotId);
        if (slot != null && slot.CurrentBookings > 0)
        {
            slot.CurrentBookings--;
            await _slotRepository.UpdateAsync(slot);
        }

        return await ToDto(updated);
    }

    private async Task<AppointmentDto> ToDto(Appointment appointment)
    {
        var slot = await _slotRepository.GetByIdAsync(appointment.AppointmentSlotId);
        var doctor = await _doctorRepository.GetByIdAsync(appointment.DoctorId);

        return new AppointmentDto
        {
            Id = appointment.Id,
            AppointmentSlotId = appointment.AppointmentSlotId,
            DoctorId = appointment.DoctorId,
            DoctorName = doctor?.Name,
            StartTime = slot?.StartTime ?? DateTime.MinValue,
            EndTime = slot?.EndTime ?? DateTime.MinValue,
            PatientName = appointment.PatientName,
            Status = appointment.Status,
            BookedAt = appointment.BookedAt
        };
    }
}
