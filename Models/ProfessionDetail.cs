using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public partial class ProfessionDetail
    {
        [Key]
        public int ProfessionId { get; set; }
        public int DentistId { get; set; }
        public string DentistName { get; set; }
        public string DentistEmail { get; set; }
        public string TreatmentName { get; set; }
        public string TreatmentDescription { get; set; }
        public int? TreatmentPrice { get; set; }
    }
}
