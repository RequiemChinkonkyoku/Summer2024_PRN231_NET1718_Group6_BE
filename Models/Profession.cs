using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models;

public partial class Profession
{
    [Key]
    public int ProfessionId { get; set; }

    public int? DentistId { get; set; }

    public int? TreatmentId { get; set; }

    public virtual Dentist? Dentist { get; set; }

    public virtual Treatment? Treatment { get; set; }
}
