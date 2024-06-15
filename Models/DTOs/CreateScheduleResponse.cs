using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class CreateScheduleResponse
    {
        public bool Success { get; set; }

        public string ErrorMessage { get; set; }

        public List<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
