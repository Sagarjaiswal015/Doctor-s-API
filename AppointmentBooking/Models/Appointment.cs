namespace AppointmentBooking.Models;

public class Appointment
{
    public int Id { get; set; }
    public int AppointmentSlotId { get; set; }
    public AppointmentSlot AppointmentSlot { get; set; }
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public string PatientName { get; set; }
    public string PatientPhone { get; set; }
    public string PatientEmail { get; set; }
    public string Reason { get; set; }
    public string Status { get; set; } = "Confirmed";
    public DateTime BookedAt { get; set; } = DateTime.UtcNow;
}
