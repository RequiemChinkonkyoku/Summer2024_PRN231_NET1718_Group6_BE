using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models;

public partial class MedicalRecord
{
    [Key]
    public int RecordId { get; set; }

    public string? Diagnosis { get; set; }

    public string? Note { get; set; }

    public int? Status { get; set; }

    public int? PatientId { get; set; }

    public int? AppointmentId { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual Patient? Patient { get; set; }
}
