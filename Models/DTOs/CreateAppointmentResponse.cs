using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class CreateAppointmentResponse
    {
        public bool Success { get; set; }

        public string ErrrorMessage { get; set; }

        public Appointment Appointment { get; set; }
    }
}
