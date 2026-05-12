using System.ComponentModel.DataAnnotations;

namespace GrampanchayatSystem.Models
{
    public class Citizen
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^[6-9][0-9]{9}$", ErrorMessage = "Enter valid 10-digit mobile number")]
        public string Mobile { get; set; }
    }
}