using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class AddMedicalRecordRequest
    {
        public string? Diagnosis { get; set; }

        public string? Note { get; set; }

        public int? Status { get; set; }

        public int? AppointmentId { get; set; }
    }
}
