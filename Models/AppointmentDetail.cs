using System;
using System.Collections.Generic;

namespace Models;

public partial class AppointmentDetail
{
    public int AppointmentDetailId { get; set; }

    public int? AppointmentId { get; set; }

    public int? TreatmentId { get; set; }

    public int? DentistId { get; set; }

    public int? ScheduleId { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual Dentist? Dentist { get; set; }

    public virtual Schedule? Schedule { get; set; }

    public virtual Treatment? Treatment { get; set; }
}
