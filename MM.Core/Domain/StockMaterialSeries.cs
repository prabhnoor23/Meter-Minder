using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Pspcl.Core.Domain
{
    public class StockMaterialSeries
    {
        [Key]
        public int Id { get; set; }
        public StockMaterial StockMaterial { get; set; }
        public int StockMaterialId { get; set; } // Foreign key
        public int SerialNumber { get; set; }
        public bool IsIssued { get; set; }
        public bool IsDeleted{ get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}