using MessagePack;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pspcl.Core.Domain;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pspcl.Web.Models
{
    public class IssueStockModel
    {
        public IssueStockModel() 
        {
            SubDivisionList = new List<SelectListItem>();

			AvailableMaterialGroups = new List<SelectListItem>();
			AvailableMaterialTypes = new List<SelectListItem>();
			AvailableMaterialCodes = new List<SelectListItem>();
			AvailableMakes = new List<string>();			
		}
		public string TransactionId { get; set; }
		public DateTime CurrentDate { get; set; }
		public string SerialNumber { get; set; }
		public DateTime SrNoDate { get; set; }
        public int SubDivisionId { get; set; }
		public string? SubDivisionName { get; set; }        
        public int DivisionId { get; set; }
        public string? Division { get; set; }
        public int? LocationCode { get; set; }
        public int CircleId { get; set; }
		public string? Circle { get; set; }
		public string JuniorEngineerName { get; set; }
		public int MaterialGroupId { get; set; }
		public string? MaterialGroupName { get; set; }
        public int MaterialTypeId { get; set; }
		public string? MaterialTypeName { get; set; }
		public int? MaterialId { get; set; }
		public string? MaterialCode { get; set; }
		public int Quantity { get; set; }
		public string Make { get; set; }
        public string? Cost { get; set; }
        public IList<SelectListItem> SubDivisionList { get; set; }
		public List<List<int>> QuantityRanges { get; set; }
		public IList<SelectListItem>? AvailableMaterialGroups { get; set; }
		public IList<SelectListItem>? AvailableMaterialTypes { get; set; }
		public IList<SelectListItem>? AvailableMaterialCodes { get; set; }
		public List<string> AvailableMakes { get; set; }
        public Dictionary<string, List<List<int>>> IssuedStockRanges { get; set; }   
		public string Image { get; set; }
		public string SrControlNumber { get; set; }       
		public List<Dictionary<string,string>> StockItems { get; set; }

		[DisplayName("Upload Image")]
		public string FileDetails { get; set; }
		public IFormFile File { get; set; }

    }
}
