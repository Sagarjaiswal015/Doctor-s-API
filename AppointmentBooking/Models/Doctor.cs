namespace AppointmentBooking.Models;

public class Doctor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Specialization { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public ICollection<AppointmentSlot> AppointmentSlots { get; set; } = new List<AppointmentSlot>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
