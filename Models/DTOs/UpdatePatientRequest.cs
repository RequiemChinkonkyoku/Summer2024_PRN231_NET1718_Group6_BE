using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class UpdatePatientRequest
    {
        public string? Name { get; set; }

        public int? YearOfBirth { get; set; }

        public string? Address { get; set; }

        public int? Gender { get; set; }
    }
}
