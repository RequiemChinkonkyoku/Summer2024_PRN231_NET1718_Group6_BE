﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class GetPatientListResponse
    {
        public int PatientId { get; set; }

        public string? Name { get; set; }
    }
}
