using AppointmentBooking.DTOs;
using AppointmentBooking.Models;
using AppointmentBooking.Repositories;

namespace AppointmentBooking.Services;

public interface IAppointmentSlotService
{
    Task<AppointmentSlotDto> CreateSlotAsync(CreateAppointmentSlotDto dto);
    Task<AppointmentSlotDto> GetSlotAsync(int slotId);
    Task<List<AppointmentSlotDto>> GetDoctorSlotsAsync(int doctorId);
    Task<List<AppointmentSlotDto>> GetAvailableSlotsAsync();
}

public class AppointmentSlotService : IAppointmentSlotService
{
    private readonly IAppointmentSlotRepository _slotRepository;
    private readonly IDoctorRepository _doctorRepository;

    public AppointmentSlotService(IAppointmentSlotRepository slotRepository, IDoctorRepository doctorRepository)
    {
        _slotRepository = slotRepository;
        _doctorRepository = doctorRepository;
    }

    public async Task<AppointmentSlotDto> CreateSlotAsync(CreateAppointmentSlotDto dto)
    {
        var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);
        if (doctor == null)
            throw new KeyNotFoundException("Doctor not found");

        var slot = new AppointmentSlot
        {
            DoctorId = dto.DoctorId,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            MaxCapacity = dto.MaxCapacity,
            CurrentBookings = 0
        };

        var createdSlot = await _slotRepository.CreateAsync(slot);
        return ToDto(createdSlot);
    }

    public async Task<AppointmentSlotDto> GetSlotAsync(int slotId)
    {
        var slot = await _slotRepository.GetByIdAsync(slotId);
        if (slot == null)
            throw new KeyNotFoundException("Slot not found");

        return ToDto(slot);
    }

    public async Task<List<AppointmentSlotDto>> GetDoctorSlotsAsync(int doctorId)
    {
        var slots = await _slotRepository.GetByDoctorIdAsync(doctorId);
        return slots.Select(ToDto).ToList();
    }

    public async Task<List<AppointmentSlotDto>> GetAvailableSlotsAsync()
    {
        var slots = await _slotRepository.GetAvailableSlotsAsync();
        return slots.Select(ToDto).ToList();
    }

    private AppointmentSlotDto ToDto(AppointmentSlot slot)
    {
        return new AppointmentSlotDto
        {
            Id = slot.Id,
            DoctorId = slot.DoctorId,
            StartTime = slot.StartTime,
            EndTime = slot.EndTime,
            MaxCapacity = slot.MaxCapacity,
            CurrentBookings = slot.CurrentBookings,
            IsAvailable = slot.IsAvailable
        };
    }
}
