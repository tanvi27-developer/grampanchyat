using System.ComponentModel.DataAnnotations;

namespace GrampanchayatSystem.Models
{
    public class TaxRecord
    {
        public int Id { get; set; }

        [Required]
        public string CitizenName { get; set; } = string.Empty;

        [Required]
        public string TaxType { get; set; } = string.Empty;

        [Range(0, 100000000)]
        public decimal Amount { get; set; }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public bool IsPaid { get; set; }
    }
}
