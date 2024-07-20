using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class AddProfessionResponse
    {
        public bool Success { get; set; }

        public string ErrorMessage { get; set; }

        public List<Profession> Professions { get; set; }
    }
}
