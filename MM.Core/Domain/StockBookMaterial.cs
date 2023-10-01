using System.ComponentModel.DataAnnotations;

namespace Pspcl.Core.Domain
{
    public class StockBookMaterial
    {
        [Key]
        public int Id { get; set; }
        public int MaterialGroupId { get; set; } // Foreign key
        public int StockIssueBookId { get; set; }  // Foreign key
        public int MaterialId { get; set; }  // Foreign key
        public int Quantity { get; set; }
        public string Make { get; set; }


    }
}
