using Pspcl.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pspcl.Services.Models
{
    public class StockOutModel
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public DateTime CurrentDate { get; set; }
        public DateTime SrNoDate { get; set; }
        public string SerialNumber { get; set; }
        public string SrControlNumber { get; set; }
        public string DivisionName { get; set; }
        public int LocationID { get; set; }
        public string SubDivisionName { get; set; }
        public string JuniorEngineerName { get; set; }
        public string MaterialName { get; set; }
        public string MaterialCode { get; set; }
        public int MaterialId { get; set; }
        public decimal Rate { get; set; }   
        public int Quantity { get; set; }
        public string Make{ get; set; }
        public float Cost{ get; set; }
        public int TestingCharge { get; set; }
        public string ImageName { get; set; }
    }
}
