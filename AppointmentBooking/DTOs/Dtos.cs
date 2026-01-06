namespace AppointmentBooking.DTOs;

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public string Token { get; set; }
    public int UserId { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}

public class DoctorDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Specialization { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}

public class AppointmentSlotDto
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int MaxCapacity { get; set; }
    public int CurrentBookings { get; set; }
    public bool IsAvailable { get; set; }
}

public class CreateAppointmentSlotDto
{
    public int DoctorId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int MaxCapacity { get; set; }
}

public class BookAppointmentDto
{
    public int AppointmentSlotId { get; set; }
    public string PatientName { get; set; }
    public string PatientPhone { get; set; }
    public string PatientEmail { get; set; }
    public string Reason { get; set; }
}

public class AppointmentDto
{
    public int Id { get; set; }
    public int AppointmentSlotId { get; set; }
    public int DoctorId { get; set; }
    public string DoctorName { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string PatientName { get; set; }
    public string Status { get; set; }
    public DateTime BookedAt { get; set; }
}

public class CreateDoctorDto
{
    public string Name { get; set; }
    public string Specialization { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
