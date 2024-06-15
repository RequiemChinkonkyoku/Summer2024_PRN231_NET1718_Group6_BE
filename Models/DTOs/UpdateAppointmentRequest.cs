using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class UpdateAppointmentRequest
    {
        public int? AppointmentId { get; set; }

        public int? PatientId { get; set; }

        public DateTime? ArrivalDate { get; set; }

        public int? TreatmentId { get; set; }

        public int? DentistId { get; set; }

        public int? ScheduleId { get; set; }
    }
}
