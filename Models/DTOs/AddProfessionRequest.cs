using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class AddProfessionRequest
    {
        public int TreatmentId { get; set; }

        public List<int> DentistIds { get; set; }
    }
}
