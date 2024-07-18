using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Models;

public partial class DentalClinicDbContext : DbContext
{
    public DentalClinicDbContext()
    {
    }

    public DentalClinicDbContext(DbContextOptions<DentalClinicDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AppointmentDetail> AppointmentDetails { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Dentist> Dentists { get; set; }

    public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Profession> Professions { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Treatment> Treatments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=(local); database=DentalClinicDB; uid=sa; pwd=12345; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__appointm__D067651E971A57CA");

            entity.ToTable("appointment");

            entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");
            entity.Property(e => e.ArrivalDate)
                .HasColumnType("date")
                .HasColumnName("arrivalDate");
            entity.Property(e => e.BookingPrice).HasColumnName("bookingPrice");
            entity.Property(e => e.CreateDate)
                .HasColumnType("date")
                .HasColumnName("createDate");
            entity.Property(e => e.CustomerId).HasColumnName("customerID");
            entity.Property(e => e.PatientId).HasColumnName("patientID");
            entity.Property(e => e.ServicePrice).HasColumnName("servicePrice");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TimeSlot).HasColumnName("timeSlot");
            entity.Property(e => e.TotalPrice).HasColumnName("totalPrice");

            entity.HasOne(d => d.Customer).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__appointme__custo__29572725");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__appointme__patie__2A4B4B5E");
        });

        modelBuilder.Entity<AppointmentDetail>(entity =>
        {
            entity.HasKey(e => e.AppointmentDetailId).HasName("PK__appointm__B5CE973C1F839E86");

            entity.ToTable("appointmentDetails");

            entity.Property(e => e.AppointmentDetailId).HasColumnName("appointmentDetailID");
            entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");
            entity.Property(e => e.DentistId).HasColumnName("dentistID");
            entity.Property(e => e.ScheduleId).HasColumnName("scheduleID");
            entity.Property(e => e.TreatmentId).HasColumnName("treatmentID");

            entity.HasOne(d => d.Appointment).WithMany(p => p.AppointmentDetails)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__appointme__appoi__3F466844");

            entity.HasOne(d => d.Dentist).WithMany(p => p.AppointmentDetails)
                .HasForeignKey(d => d.DentistId)
                .HasConstraintName("FK__appointme__denti__412EB0B6");

            entity.HasOne(d => d.Schedule).WithMany(p => p.AppointmentDetails)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("FK__appointme__sched__4222D4EF");

            entity.HasOne(d => d.Treatment).WithMany(p => p.AppointmentDetails)
                .HasForeignKey(d => d.TreatmentId)
                .HasConstraintName("FK__appointme__treat__403A8C7D");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__customer__B611CB9DBA65D980");

            entity.ToTable("customer");

            entity.Property(e => e.CustomerId).HasColumnName("customerID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PasswordHash)
                .IsUnicode(false)
                .HasColumnName("passwordHash");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Dentist>(entity =>
        {
            entity.HasKey(e => e.DentistId).HasName("PK__dentist__3816049888705821");

            entity.ToTable("dentist");

            entity.Property(e => e.DentistId).HasColumnName("dentistID");
            entity.Property(e => e.ContractType)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("contractType");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PasswordHash)
                .IsUnicode(false)
                .HasColumnName("passwordHash");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__medicalR__D825197E96D7C891");

            entity.ToTable("medicalRecord");

            entity.Property(e => e.RecordId).HasColumnName("recordID");
            entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");
            entity.Property(e => e.Diagnosis)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("diagnosis");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("note");
            entity.Property(e => e.PatientId).HasColumnName("patientID");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Appointment).WithMany(p => p.MedicalRecords)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__medicalRe__appoi__2E1BDC42");

            entity.HasOne(d => d.Patient).WithMany(p => p.MedicalRecords)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__medicalRe__patie__2D27B809");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__patient__A17005CC672051AE");

            entity.ToTable("patient");

            entity.Property(e => e.PatientId).HasColumnName("patientID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.CustomerId).HasColumnName("customerID");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.YearOfBirth).HasColumnName("yearOfBirth");

            entity.HasOne(d => d.Customer).WithMany(p => p.Patients)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__patient__custome__267ABA7A");
        });

        modelBuilder.Entity<Profession>(entity =>
        {
            entity.HasKey(e => e.ProfessionId).HasName("PK__professi__2FE38803FCA5A810");

            entity.ToTable("profession");

            entity.Property(e => e.ProfessionId).HasColumnName("professionID");
            entity.Property(e => e.DentistId).HasColumnName("dentistID");
            entity.Property(e => e.TreatmentId).HasColumnName("treatmentID");

            entity.HasOne(d => d.Dentist).WithMany(p => p.Professions)
                .HasForeignKey(d => d.DentistId)
                .HasConstraintName("FK__professio__denti__38996AB5");

            entity.HasOne(d => d.Treatment).WithMany(p => p.Professions)
                .HasForeignKey(d => d.TreatmentId)
                .HasConstraintName("FK__professio__treat__398D8EEE");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__schedule__A532EDB41A528BA8");

            entity.ToTable("schedule");

            entity.Property(e => e.ScheduleId).HasColumnName("scheduleID");
            entity.Property(e => e.DentistId).HasColumnName("dentistID");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TimeSlot).HasColumnName("timeSlot");
            entity.Property(e => e.WorkDate)
                .HasColumnType("date")
                .HasColumnName("workDate");

            entity.HasOne(d => d.Dentist).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.DentistId)
                .HasConstraintName("FK__schedule__dentis__3C69FB99");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__transact__9B57CF5236CE7B49");

            entity.ToTable("transaction");

            entity.Property(e => e.TransactionId).HasColumnName("transactionID");
            entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");
            entity.Property(e => e.CustomerId).HasColumnName("customerID");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TransactionTime)
                .HasColumnType("datetime")
                .HasColumnName("transactionTime");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__transacti__appoi__31EC6D26");

            entity.HasOne(d => d.Customer).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__transacti__custo__30F848ED");
        });

        modelBuilder.Entity<Treatment>(entity =>
        {
            entity.HasKey(e => e.TreatmentId).HasName("PK__treatmen__D7AA5888E6ED1C15");

            entity.ToTable("treatment");

            entity.Property(e => e.TreatmentId).HasColumnName("treatmentID");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
