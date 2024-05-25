using System;
using System.Collections.Generic;

namespace Models;

public partial class MedicalRecord
{
    public int RecordId { get; set; }

    public string? Treatment { get; set; }

    public string? Diagnosis { get; set; }

    public string? Note { get; set; }

    public DateTime? FollowUpDate { get; set; }

    public int? PatientId { get; set; }

    public int? AppointmentId { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual Patient? Patient { get; set; }
}
