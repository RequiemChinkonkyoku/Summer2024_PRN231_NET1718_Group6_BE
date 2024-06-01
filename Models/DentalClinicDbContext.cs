using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
        => optionsBuilder.UseSqlServer(GetConnectionString());


    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true, true)
        .Build();
        var strConn = config["ConnectionStrings:DentalClinicDB"];

        return strConn;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__appointm__D067651E6821671C");

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
                .HasConstraintName("FK__appointme__custo__3C69FB99");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__appointme__patie__3D5E1FD2");
        });

        modelBuilder.Entity<AppointmentDetail>(entity =>
        {
            entity.HasKey(e => e.AppointmentDetailId).HasName("PK__appointm__B5CE973CA1B9E610");

            entity.ToTable("appointmentDetails");

            entity.Property(e => e.AppointmentDetailId).HasColumnName("appointmentDetailID");
            entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");
            entity.Property(e => e.DentistId).HasColumnName("dentistID");
            entity.Property(e => e.ScheduleId).HasColumnName("scheduleID");
            entity.Property(e => e.TreatmentId).HasColumnName("treatmentID");

            entity.HasOne(d => d.Appointment).WithMany(p => p.AppointmentDetails)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__appointme__appoi__52593CB8");

            entity.HasOne(d => d.Dentist).WithMany(p => p.AppointmentDetails)
                .HasForeignKey(d => d.DentistId)
                .HasConstraintName("FK__appointme__denti__5441852A");

            entity.HasOne(d => d.Schedule).WithMany(p => p.AppointmentDetails)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("FK__appointme__sched__5535A963");

            entity.HasOne(d => d.Treatment).WithMany(p => p.AppointmentDetails)
                .HasForeignKey(d => d.TreatmentId)
                .HasConstraintName("FK__appointme__treat__534D60F1");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__customer__B611CB9D93B974C7");

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
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Dentist>(entity =>
        {
            entity.HasKey(e => e.DentistId).HasName("PK__dentist__381604984669AA19");

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
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__medicalR__D825197E7E12116B");

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
                .HasConstraintName("FK__medicalRe__appoi__412EB0B6");

            entity.HasOne(d => d.Patient).WithMany(p => p.MedicalRecords)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__medicalRe__patie__403A8C7D");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__patient__A17005CC48B26DDA");

            entity.ToTable("patient");

            entity.Property(e => e.PatientId).HasColumnName("patientID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.CustomerId).HasColumnName("customerID");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Customer).WithMany(p => p.Patients)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__patient__custome__398D8EEE");
        });

        modelBuilder.Entity<Profession>(entity =>
        {
            entity.HasKey(e => e.ProfessionId).HasName("PK__professi__2FE388033ADB09F8");

            entity.ToTable("profession");

            entity.Property(e => e.ProfessionId).HasColumnName("professionID");
            entity.Property(e => e.DentistId).HasColumnName("dentistID");
            entity.Property(e => e.TreatmentId).HasColumnName("treatmentID");

            entity.HasOne(d => d.Dentist).WithMany(p => p.Professions)
                .HasForeignKey(d => d.DentistId)
                .HasConstraintName("FK__professio__denti__4BAC3F29");

            entity.HasOne(d => d.Treatment).WithMany(p => p.Professions)
                .HasForeignKey(d => d.TreatmentId)
                .HasConstraintName("FK__professio__treat__4CA06362");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__schedule__A532EDB436F67C4F");

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
                .HasConstraintName("FK__schedule__dentis__4F7CD00D");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__transact__9B57CF528D0E69C6");

            entity.ToTable("transaction");

            entity.Property(e => e.TransactionId).HasColumnName("transactionID");
            entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");
            entity.Property(e => e.CustomerId).HasColumnName("customerID");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TransactionTime).HasColumnName("transactionTime");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__transacti__appoi__44FF419A");

            entity.HasOne(d => d.Customer).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__transacti__custo__440B1D61");
        });

        modelBuilder.Entity<Treatment>(entity =>
        {
            entity.HasKey(e => e.TreatmentId).HasName("PK__treatmen__D7AA588899941EEC");

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
