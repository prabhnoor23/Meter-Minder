using Pspcl.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pspcl.Services.Models
{
    public class AvailableStockModel
    {
            
        public StockMaterial StockMaterial { get; set; }
        public int StockMaterialId { get; set; }
        public String grnNo { get; set; }
        public DateTime grnDate { get; set; }
        public String MaterialGroup { get; set; }
        public String MaterialName { get; set; }
        public String MaterialCode { get; set; }
        public String Make { get; set; }
        public int AvailableQuantity { get; set; }
        public int SrNoFrom { get; set; }
        public int SrNoTo { get; set; }
        public float Rate { get; set; }
        public float Value { get; set; }
    }
}
