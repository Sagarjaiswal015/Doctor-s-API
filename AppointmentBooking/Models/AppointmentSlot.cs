namespace AppointmentBooking.Models;

public class AppointmentSlot
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int MaxCapacity { get; set; }
    public int CurrentBookings { get; set; }
    public bool IsAvailable => CurrentBookings < MaxCapacity;
}
