using System;
using System.Collections.Generic;

namespace Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public DateTime? ArrivalDate { get; set; }

    public string? Type { get; set; }

    public int? Status { get; set; }

    public int? TimeSlot { get; set; }

    public int? PatientId { get; set; }

    public virtual ICollection<AppointmentDetail> AppointmentDetails { get; set; } = new List<AppointmentDetail>();

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual Patient? Patient { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
