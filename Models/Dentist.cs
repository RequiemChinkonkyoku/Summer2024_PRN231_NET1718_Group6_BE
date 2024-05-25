﻿using System;
using System.Collections.Generic;

namespace Models;

public partial class Dentist
{
    public int DentistId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public int? Type { get; set; }

    public string? ContractType { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<AppointmentDetail> AppointmentDetails { get; set; } = new List<AppointmentDetail>();

    public virtual ICollection<DentistMedicalService> DentistMedicalServices { get; set; } = new List<DentistMedicalService>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
