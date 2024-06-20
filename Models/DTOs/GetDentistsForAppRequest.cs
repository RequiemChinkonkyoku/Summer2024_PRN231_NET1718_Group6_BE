using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class GetDentistsForAppRequest
    {
        public int TreatmentId { get; set; }
        public DateTime Date { get; set; }
        public int TimeSlot { get; set; }
    }
}
