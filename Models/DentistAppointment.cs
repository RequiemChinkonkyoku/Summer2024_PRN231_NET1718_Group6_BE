using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public partial class DentistAppointment
    {
        public int AppointmentId { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? ArrivalDate { get; set; }

        public int? TimeSlot { get; set; }

        public int? Status { get; set; }

        public int? CustomerId { get; set; }

        public int? PatientId { get; set; }

        public int? TreatmentId { get; set; }

        public int? DentistId { get; set; }

        public int? ScheduleId { get; set; }
    }
}
