using System.ComponentModel.DataAnnotations;

namespace Pspcl.Core.Domain
{
    public class StockMaterial
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int SerialNumberFrom { get; set; }
        public int SerialNumberTo { get; set; }
        public  Stock Stock { get; set; }
        public int StockId { get; set; } // Foreign key
        public int DisplayOrder { get; set; }
        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
