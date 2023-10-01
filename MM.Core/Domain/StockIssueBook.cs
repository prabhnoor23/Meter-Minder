using System.ComponentModel.DataAnnotations;

namespace Pspcl.Core.Domain
{
    public class StockIssueBook
    {
        [Key]
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public DateTime CurrentDate { get; set; }
		public DateTime SrNoDate { get; set; }
		public string SerialNumber { get; set; }
        public int DivisionId { get; set; } // Foreign key
        public int SubDivisionId { get; set; }  // Foreign key
        public int CircleId { get; set; }  // Foreign key
        public string JuniorEngineerName { get; set; }
        public string Image { get; set; }
        public string SrControlNumber { get; set; }

    }
}
