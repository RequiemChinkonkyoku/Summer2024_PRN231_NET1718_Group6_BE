using System;
using System.Collections.Generic;

namespace Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? Price { get; set; }

    public DateTime? TransactionTime { get; set; }

    public int? Status { get; set; }

    public int? CustomerId { get; set; }

    public int? AppointmentId { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual Customer? Customer { get; set; }
}
