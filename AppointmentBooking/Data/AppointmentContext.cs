using Microsoft.EntityFrameworkCore;
using AppointmentBooking.Models;

namespace AppointmentBooking.Data;

public class AppointmentContext : DbContext
{
    public AppointmentContext(DbContextOptions<AppointmentContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<AppointmentSlot> AppointmentSlots { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<Doctor>()
            .HasKey(d => d.Id);

        modelBuilder.Entity<Doctor>()
            .HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId);

        modelBuilder.Entity<AppointmentSlot>()
            .HasKey(a => a.Id);

        modelBuilder.Entity<AppointmentSlot>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.AppointmentSlots)
            .HasForeignKey(a => a.DoctorId);

        modelBuilder.Entity<Appointment>()
            .HasKey(a => a.Id);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.AppointmentSlot)
            .WithMany()
            .HasForeignKey(a => a.AppointmentSlotId);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId);
    }
}
