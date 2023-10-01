using Pspcl.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Pspcl.Services.Models
{
    public class StockInModel
    {
        public Stock Stock { get; set; }
        public string MaterialGroup { get; set; }
        public string MaterialName { get; set; }
        public string MaterialCode { get; set; }
        public int Quantity { get; set; }
    }
}
