using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class UpdateTreatmentRequest
    {
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name should be between 2 and 100 characters")]
        public string? Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Price must be a positive number")]
        public int? Price { get; set; }

        [StringLength(500, ErrorMessage = "Description should not exceed 500 characters")]
        public string? Description { get; set; }

        [Range(0, 1, ErrorMessage = "Status must be either 0 (inactive) or 1 (active)")]
        public int? Status { get; set; }
    }
}
