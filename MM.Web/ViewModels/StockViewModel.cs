using Microsoft.AspNetCore.Mvc.Rendering;
using Pspcl.Core.Domain;
using System.ComponentModel.DataAnnotations;


namespace Pspcl.Web.ViewModels
{
	public class StockViewModel
	{
		public StockViewModel()
		{
			AvailableMaterialGroups = new List<SelectListItem>();
			AvailableMaterialTypes = new List<SelectListItem>();
			AvailableRatings = new List<SelectListItem>();
			AvailableMaterialCodes = new List<SelectListItem>();
		}
		///[DataMember]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? GrnDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }


        //[DataMember]
        public string? GrnNumber { get; set; }
		//[DataMember]
		public String? TestReportReference { get; set; }
		//[DataMember]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? InvoiceDate { get; set; }
		//[DataMember]
		public string? InvoiceNumber { get; set; }
		public IList<SelectListItem>? AvailableMaterialCodes { get; set; }
		//[DataMember]
		public int? MaterialIdByCode { get; set; }
		//[DataMember]
		public IList<SelectListItem>? AvailableMaterialGroups { get; set; }
		public int? MaterialGroupId { get; set; }
		public string SelectedMaterialGroupName { get; set; }

		public IList<SelectListItem>? AvailableMaterialTypes { get; set; }
		public int? MaterialTypeId { get; set; }
		public string SelectedMaterialTypeName { get; set; }

		public IList<SelectListItem>? AvailableRatings { get; set; }
		public int? Rating { get; set; }
		//[DataMember]
		public decimal? Rate { get; set; }
		//[DataMember]
		public string? PrefixNumber { get; set; }
		public string Make { get; set; }
		public List<StockMaterial> stockMaterialList { get; set; }
		public string SelectedMaterialCodeName { get; set; }
		public string SelectedRatingName { get; set; }
		public string TransactionId { get; set; }
	}
}