using System;
using System.Collections.Generic;

namespace Models;

public partial class Patient
{
    public int PatientId { get; set; }

    public string? Name { get; set; }

    public int? YearOfBirth { get; set; }

    public string? Address { get; set; }

    public int? Gender { get; set; }

    public int? Status { get; set; }

    public int? CustomerId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}
