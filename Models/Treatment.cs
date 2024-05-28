using System;
using System.Collections.Generic;

namespace Models;

public partial class Treatment
{
    public int TreatmentId { get; set; }

    public string? Name { get; set; }

    public int? Price { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<AppointmentDetail> AppointmentDetails { get; set; } = new List<AppointmentDetail>();

    public virtual ICollection<Profession> Professions { get; set; } = new List<Profession>();
}
