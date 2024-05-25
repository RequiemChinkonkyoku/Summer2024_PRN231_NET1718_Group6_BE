using System;
using System.Collections.Generic;

namespace Models;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public DateTime? WorkDate { get; set; }

    public int? TimeSlot { get; set; }

    public int? Status { get; set; }

    public int? DentistId { get; set; }

    public virtual ICollection<AppointmentDetail> AppointmentDetails { get; set; } = new List<AppointmentDetail>();

    public virtual Dentist? Dentist { get; set; }
}
