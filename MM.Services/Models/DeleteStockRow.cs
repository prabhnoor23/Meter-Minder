

namespace Pspcl.Services.Models
{
    public class DeleteStockRow
    {
        public int StockMaterialId { get; set; }
        public int SrNoFrom { get; set; }
        public int SrNoTo { get; set; }
        public int Quantity { get; set; }
    }
}
