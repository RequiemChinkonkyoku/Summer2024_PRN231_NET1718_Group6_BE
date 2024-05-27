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

    public virtual DbSet<Dentist> Dentists { get; set; }

    public virtual DbSet<DentistMedicalService> DentistMedicalServices { get; set; }

    public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }

    public virtual DbSet<MedicalService> MedicalServices { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

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
            entity.HasKey(e => e.AppointmentId).HasName("PK__appointm__D067651E7EA9AF69");

            entity.ToTable("appointment");

            entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");
            entity.Property(e => e.ArrivalDate)
                .HasColumnType("date")
                .HasColumnName("arrivalDate");
            entity.Property(e => e.PatientId).HasColumnName("patientID");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TimeSlot).HasColumnName("timeSlot");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("type");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__appointme__patie__45F365D3");
        });

        modelBuilder.Entity<AppointmentDetail>(entity =>
        {
            entity.HasKey(e => e.AppointmentDetailId).HasName("PK__appointm__B5CE973C1646E6A0");

            entity.ToTable("appointmentDetails");

            entity.Property(e => e.AppointmentDetailId).HasColumnName("appointmentDetailID");
            entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");
            entity.Property(e => e.DentistId).HasColumnName("dentistID");
            entity.Property(e => e.MedicalServiceId).HasColumnName("medicalServiceID");
            entity.Property(e => e.ScheduleId).HasColumnName("scheduleID");

            entity.HasOne(d => d.Appointment).WithMany(p => p.AppointmentDetails)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__appointme__appoi__46E78A0C");

            entity.HasOne(d => d.Dentist).WithMany(p => p.AppointmentDetails)
                .HasForeignKey(d => d.DentistId)
                .HasConstraintName("FK__appointme__denti__47DBAE45");

            entity.HasOne(d => d.MedicalService).WithMany(p => p.AppointmentDetails)
                .HasForeignKey(d => d.MedicalServiceId)
                .HasConstraintName("FK__appointme__medic__49C3F6B7");

            entity.HasOne(d => d.Schedule).WithMany(p => p.AppointmentDetails)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("FK__appointme__sched__48CFD27E");
        });

        modelBuilder.Entity<Dentist>(entity =>
        {
            entity.HasKey(e => e.DentistId).HasName("PK__dentist__3816049879CAF6EB");

            entity.ToTable("dentist");

            entity.Property(e => e.DentistId).HasColumnName("dentistID");
            entity.Property(e => e.ContractType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("contractType");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<DentistMedicalService>(entity =>
        {
            entity.HasKey(e => e.DentistMedicalServiceId).HasName("PK__dentistM__C98360CAAD9EECF1");

            entity.ToTable("dentistMedicalService");

            entity.Property(e => e.DentistMedicalServiceId).HasColumnName("dentistMedicalServiceID");
            entity.Property(e => e.DentistId).HasColumnName("dentistID");
            entity.Property(e => e.MedicalServiceId).HasColumnName("medicalServiceID");

            entity.HasOne(d => d.Dentist).WithMany(p => p.DentistMedicalServices)
                .HasForeignKey(d => d.DentistId)
                .HasConstraintName("FK__dentistMe__denti__4AB81AF0");

            entity.HasOne(d => d.MedicalService).WithMany(p => p.DentistMedicalServices)
                .HasForeignKey(d => d.MedicalServiceId)
                .HasConstraintName("FK__dentistMe__medic__4BAC3F29");
        });

        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__medicalR__D825197ED46F1B31");

            entity.ToTable("medicalRecord");

            entity.Property(e => e.RecordId).HasColumnName("recordID");
            entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");
            entity.Property(e => e.Diagnosis)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("diagnosis");
            entity.Property(e => e.FollowUpDate)
                .HasColumnType("date")
                .HasColumnName("followUpDate");
            entity.Property(e => e.Note)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("note");
            entity.Property(e => e.PatientId).HasColumnName("patientID");
            entity.Property(e => e.Treatment)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("treatment");

            entity.HasOne(d => d.Appointment).WithMany(p => p.MedicalRecords)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__medicalRe__appoi__4CA06362");

            entity.HasOne(d => d.Patient).WithMany(p => p.MedicalRecords)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__medicalRe__patie__4D94879B");
        });

        modelBuilder.Entity<MedicalService>(entity =>
        {
            entity.HasKey(e => e.MedicalServiceId).HasName("PK__medicalS__BE76D8FB29CFC7C6");

            entity.ToTable("medicalService");

            entity.Property(e => e.MedicalServiceId).HasColumnName("medicalServiceID");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__patient__A17005CCD28EA245");

            entity.ToTable("patient");

            entity.Property(e => e.PatientId).HasColumnName("patientID");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__schedule__A532EDB46E86F391");

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
                .HasConstraintName("FK__schedule__dentis__4E88ABD4");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__transact__9B57CF52BC024A7B");

            entity.ToTable("transaction");

            entity.Property(e => e.TransactionId).HasColumnName("transactionID");
            entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.TransactionTime).HasColumnName("transactionTime");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__transacti__appoi__4F7CD00D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
