namespace GrampanchayatSystem.Models
{
    public class Certificate
    {
        public int Id { get; set; }
        public string CitizenName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Birth / Death
        public string Details { get; set; } = string.Empty;
        public string RequestedByUserId { get; set; } = string.Empty;
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending"; // Pending / Approved / Rejected
        public string PaymentStatus { get; set; } = "Unpaid"; // Unpaid / Paid
        public string? PaymentReference { get; set; }
        public string? CertificateNumber { get; set; }
    }
}