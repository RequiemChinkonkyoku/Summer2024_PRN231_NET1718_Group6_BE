using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class CreateScheduleRequest
    {
        public int DentistId { get; set; }

        public List<DayOfWeekTimeSlot> DayOfWeekTimeSlots { get; set; }

        public DateTime StartDate { get; set; }

        public int RepeatForWeeks { get; set; }
    }

    public class DayOfWeekTimeSlot
    {
        public DayOfWeek DayOfWeek { get; set; }

        public int TimeSlot { get; set; }
    }
}
