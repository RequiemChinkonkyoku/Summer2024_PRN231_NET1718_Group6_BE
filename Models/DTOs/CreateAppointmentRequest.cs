﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class CreateAppointmentRequest
    {
        public int PatientId { get; set; }

        public int ScheduleId { get; set; }

        public int TreatmentId { get; set; }

        public int DentistId { get; set; }
    }
}
