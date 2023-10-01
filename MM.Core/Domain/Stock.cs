using System.ComponentModel.DataAnnotations;

namespace Pspcl.Core.Domain
{
    public class Stock
    {
        [Key]
        public int Id { get; set; } // primary key
        public string TransactionId { get; set; }
        public DateTime? GrnDate { get; set; }
        public string? GrnNumber { get; set; }
        public string TestReportReference { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal Rate { get; set; }
        public string PrefixNumber { get; set; }
        public MaterialGroup MaterialGroup { get; set; }
        public int MaterialGroupId { get; set; }
        public int MaterialTypeId { get; set; }
        public int MaterialId { get; set; }
        public string? Rating { get; set; }
        public string Make { get; set; }
    }
}


