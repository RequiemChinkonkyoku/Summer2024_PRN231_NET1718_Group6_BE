using System;
using System.Collections.Generic;

namespace Models;

public partial class Patient
{
    public int PatientId { get; set; }

    public string? Name { get; set; }

    public int? Age { get; set; }

    public string? Address { get; set; }

    public int? Gender { get; set; }

    public int? AccountId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}
