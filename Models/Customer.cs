﻿using System;
using System.Collections.Generic;

namespace Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? PasswordHash { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
