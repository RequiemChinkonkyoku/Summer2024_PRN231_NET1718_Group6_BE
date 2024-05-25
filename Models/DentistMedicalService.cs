using System;
using System.Collections.Generic;

namespace Models;

public partial class DentistMedicalService
{
    public int DentistMedicalServiceId { get; set; }

    public int? DentistId { get; set; }

    public int? MedicalServiceId { get; set; }

    public virtual Dentist? Dentist { get; set; }

    public virtual MedicalService? MedicalService { get; set; }
}
