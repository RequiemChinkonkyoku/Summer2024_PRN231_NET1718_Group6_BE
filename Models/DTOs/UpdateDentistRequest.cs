using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class UpdateDentistRequest
    {
        public int DentistId { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public int? Type { get; set; }

        public string? ContractType { get; set; }

        public int? Status { get; set; }
    }
}
