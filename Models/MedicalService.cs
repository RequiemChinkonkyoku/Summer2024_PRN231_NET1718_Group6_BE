using System;
using System.Collections.Generic;

namespace Models;

public partial class MedicalService
{
    public int MedicalServiceId { get; set; }

    public string? Name { get; set; }

    public int? Price { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<AppointmentDetail> AppointmentDetails { get; set; } = new List<AppointmentDetail>();

    public virtual ICollection<DentistMedicalService> DentistMedicalServices { get; set; } = new List<DentistMedicalService>();
}
