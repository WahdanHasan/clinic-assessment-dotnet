using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment_redone.Data.Models.Db.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_assessment.data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClinicUser>()
                        .Property(b => b.Id)
                        .UseIdentityAlwaysColumn();

            modelBuilder.Entity<Appointment>()
                        .Property(a => a.Id)
                        .UseIdentityAlwaysColumn();

            modelBuilder.Entity<Patient>()
                        .HasMany(p => p.patientAppointments)
                        .WithOne(a => a.Patient)
                        .IsRequired()
                        .HasForeignKey(a => a.PatientId);

            modelBuilder.Entity<Doctor>()
                        .HasMany(d => d.doctorAppointments)
                        .WithOne(a => a.Doctor)
                        .IsRequired()
                        .HasForeignKey(a => a.DoctorId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        }

        public DbSet<ClinicUser> ClinicUser { get; set; }

        public DbSet<Employee> Employee { get; set; }

        public DbSet<Doctor> Doctor { get; set; }

        public DbSet<Patient> Patient { get; set; }

        public DbSet<Role> Role { get; set; }

        public DbSet<UserRoles> UserRole { get; set; }

        public DbSet<Permission> Permission { get; set; }

        public DbSet<RolePermissions> RolePermission { get; set; }

        public DbSet<Appointment> Appointment { get; set; }

    }
}
