using AppointmentBooking.Data;
using AppointmentBooking.Models;

namespace AppointmentBooking.Repositories;

public interface IUserRepository
{
    Task<User> GetByEmailAsync(string email);
    Task<User> GetByIdAsync(int id);
    Task<User> CreateAsync(User user);
}

public class UserRepository : IUserRepository
{
    private readonly AppointmentContext _context;

    public UserRepository(AppointmentContext context)
    {
        _context = context;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await Task.FromResult(_context.Users.FirstOrDefault(u => u.Email == email));
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await Task.FromResult(_context.Users.FirstOrDefault(u => u.Id == id));
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
}

public interface IDoctorRepository
{
    Task<Doctor> GetByIdAsync(int id);
    Task<List<Doctor>> GetAllAsync();
    Task<Doctor> CreateAsync(Doctor doctor);
    Task<Doctor> UpdateAsync(Doctor doctor);
}

public class DoctorRepository : IDoctorRepository
{
    private readonly AppointmentContext _context;

    public DoctorRepository(AppointmentContext context)
    {
        _context = context;
    }

    public async Task<Doctor> GetByIdAsync(int id)
    {
        return await Task.FromResult(_context.Doctors.FirstOrDefault(d => d.Id == id));
    }

    public async Task<List<Doctor>> GetAllAsync()
    {
        return await Task.FromResult(_context.Doctors.ToList());
    }

    public async Task<Doctor> CreateAsync(Doctor doctor)
    {
        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();
        return doctor;
    }

    public async Task<Doctor> UpdateAsync(Doctor doctor)
    {
        _context.Doctors.Update(doctor);
        await _context.SaveChangesAsync();
        return doctor;
    }
}

public interface IAppointmentSlotRepository
{
    Task<AppointmentSlot> GetByIdAsync(int id);
    Task<List<AppointmentSlot>> GetByDoctorIdAsync(int doctorId);
    Task<List<AppointmentSlot>> GetAvailableSlotsAsync();
    Task<AppointmentSlot> CreateAsync(AppointmentSlot slot);
    Task<AppointmentSlot> UpdateAsync(AppointmentSlot slot);
}

public class AppointmentSlotRepository : IAppointmentSlotRepository
{
    private readonly AppointmentContext _context;

    public AppointmentSlotRepository(AppointmentContext context)
    {
        _context = context;
    }

    public async Task<AppointmentSlot> GetByIdAsync(int id)
    {
        return await Task.FromResult(_context.AppointmentSlots.FirstOrDefault(s => s.Id == id));
    }

    public async Task<List<AppointmentSlot>> GetByDoctorIdAsync(int doctorId)
    {
        return await Task.FromResult(_context.AppointmentSlots.Where(s => s.DoctorId == doctorId).ToList());
    }

    public async Task<List<AppointmentSlot>> GetAvailableSlotsAsync()
    {
        return await Task.FromResult(_context.AppointmentSlots.Where(s => s.IsAvailable && s.StartTime > DateTime.UtcNow).ToList());
    }

    public async Task<AppointmentSlot> CreateAsync(AppointmentSlot slot)
    {
        _context.AppointmentSlots.Add(slot);
        await _context.SaveChangesAsync();
        return slot;
    }

    public async Task<AppointmentSlot> UpdateAsync(AppointmentSlot slot)
    {
        _context.AppointmentSlots.Update(slot);
        await _context.SaveChangesAsync();
        return slot;
    }
}

public interface IAppointmentRepository
{
    Task<Appointment> GetByIdAsync(int id);
    Task<List<Appointment>> GetByUserIdAsync(int userId);
    Task<List<Appointment>> GetByDoctorIdAsync(int doctorId);
    Task<Appointment> CreateAsync(Appointment appointment);
    Task<Appointment> UpdateAsync(Appointment appointment);
}

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppointmentContext _context;

    public AppointmentRepository(AppointmentContext context)
    {
        _context = context;
    }

    public async Task<Appointment> GetByIdAsync(int id)
    {
        return await Task.FromResult(_context.Appointments.FirstOrDefault(a => a.Id == id));
    }

    public async Task<List<Appointment>> GetByUserIdAsync(int userId)
    {
        return await Task.FromResult(_context.Appointments.Where(a => a.UserId == userId).ToList());
    }

    public async Task<List<Appointment>> GetByDoctorIdAsync(int doctorId)
    {
        return await Task.FromResult(_context.Appointments.Where(a => a.DoctorId == doctorId).ToList());
    }

    public async Task<Appointment> CreateAsync(Appointment appointment)
    {
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
        return appointment;
    }

    public async Task<Appointment> UpdateAsync(Appointment appointment)
    {
        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();
        return appointment;
    }
}
