using AppointmentBooking.DTOs;
using AppointmentBooking.Models;
using AppointmentBooking.Repositories;

namespace AppointmentBooking.Services;

public interface IDoctorService
{
    Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto dto);
    Task<DoctorDto> GetDoctorAsync(int doctorId);
    Task<List<DoctorDto>> GetAllDoctorsAsync();
}

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public DoctorService(IDoctorRepository doctorRepository, IUserRepository userRepository, IAuthService authService)
    {
        _doctorRepository = doctorRepository;
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto dto)
    {
        var user = new User
        {
            Email = dto.Email,
            PasswordHash = _authService.HashPassword(dto.Password),
            Role = "Doctor",
            CreatedAt = DateTime.UtcNow
        };

        var createdUser = await _userRepository.CreateAsync(user);

        var doctor = new Doctor
        {
            Name = dto.Name,
            Specialization = dto.Specialization,
            Phone = dto.Phone,
            Email = dto.Email,
            UserId = createdUser.Id
        };

        var createdDoctor = await _doctorRepository.CreateAsync(doctor);

        return new DoctorDto
        {
            Id = createdDoctor.Id,
            Name = createdDoctor.Name,
            Specialization = createdDoctor.Specialization,
            Phone = createdDoctor.Phone,
            Email = createdDoctor.Email
        };
    }

    public async Task<DoctorDto> GetDoctorAsync(int doctorId)
    {
        var doctor = await _doctorRepository.GetByIdAsync(doctorId);
        if (doctor == null)
            throw new KeyNotFoundException("Doctor not found");

        return new DoctorDto
        {
            Id = doctor.Id,
            Name = doctor.Name,
            Specialization = doctor.Specialization,
            Phone = doctor.Phone,
            Email = doctor.Email
        };
    }

    public async Task<List<DoctorDto>> GetAllDoctorsAsync()
    {
        var doctors = await _doctorRepository.GetAllAsync();
        return doctors.Select(d => new DoctorDto
        {
            Id = d.Id,
            Name = d.Name,
            Specialization = d.Specialization,
            Phone = d.Phone,
            Email = d.Email
        }).ToList();
    }
}
