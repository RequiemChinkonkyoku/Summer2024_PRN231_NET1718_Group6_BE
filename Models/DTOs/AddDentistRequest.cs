using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class AddDentistRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name should be between 2 and 100 characters")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Password should be at least 3 characters")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Type is required")]
        [Range(0, 2, ErrorMessage = "Type must be between 0 and 2")]
        public int? Type { get; set; }

        [Required(ErrorMessage = "ContractType is required")]
        [StringLength(50, ErrorMessage = "ContractType should not exceed 50 characters")]
        public string? ContractType { get; set; }

        //[Required(ErrorMessage = "Status is required")]
        //[Range(0, 1, ErrorMessage = "Status must be between 0 and 1")]
        //public int? Status { get; set; }
    }
}
