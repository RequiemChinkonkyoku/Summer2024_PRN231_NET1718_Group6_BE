using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class GetDentistsForAppResponse
    {
        public int DentistId { get; set; }
        public string DentistName { get; set; }
        public int ScheduleId { get; set; }
    }
}
