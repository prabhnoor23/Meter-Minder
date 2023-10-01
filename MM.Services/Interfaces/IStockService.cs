using Microsoft.AspNetCore.Http;
using Pspcl.Core.Domain;
using Pspcl.Services.Models;

namespace Pspcl.Services.Interfaces
{
    public interface IStockService
    {
        
        List<MaterialGroup> GetAllMaterialGroups(bool? onlyActive = false);
        List<MaterialType> GetAllMaterialTypes(int materialGroupId, bool? onlyActive = false);
        List<Tuple<int, string>> GetAllMaterialRatings(int materialTypeId);
        List<Material> GetAllMaterialCodes(int materialTypeId, bool? onlyActive = false);
		List<SubDivision> GetAllSubDivisions(bool? onlyActive = false);
		List<string> GetCircleAndDivisionAndLocationCode(int selectedSubDivId, bool? onlyActive = false);
		//List<List<int>> GetAvailableQuantity(List<int> Ids);

        void UpdateStockMaterialSeries(List<List<int>> requiredIssueData);
		int IssueStock(StockIssueBook stockIssueBook);
		void StockBookMaterial(StockBookMaterial stockBookMaterial);


		int AddStock(Stock stock);
		int AddStockMaterial(StockMaterial stockMaterial);
        void AddStockMaterialSeries(StockMaterialSeries stockMaterialSeries);
        List<StockInModel> GetStockInModels();
        List<AvailableStockModel> GetAvailableStock();
        List<StockOutModel> GetStockOutModels();
        string GetMaterialGroupById(int? materialGroupId);
        string GetMaterialTypeById(int? materialTypeId);
        string GetMaterialCodeById(int? materialCodeId);
        string GetRatingNameById(int? materialTypeId);
        public string GetCorrespondingMakeValue(string invoiceNumber);

        //public int GetCorrespondingRateValue(int materialId);
        public bool isGrnNumberExist(string GrnNumber);
        public float GetCost(int materialId,int noOfUnits);

        public bool srNoValidationInDatabase(List<int> serialNumbers, int materialGroupId,int materialTypeId, int materialId, string make);
		public Dictionary<String, int> AllMakesAndQuantities(int materialGroupId, int materialTypeId, int materialId);
        public Dictionary<string, List<List<int>>> GetAvailableMakesAndRows(int materialGroupId, int materialTypeId, int materialId);
        public void UpdateIsDeletedColumn(List<List<int>> selectedRowsToDelete);
        public void UpdateStockMaterial(List<List<int>> selectedRowsToDelete);
        public string getCircleNameById(int CircleId);
        public string getDivisionNameById(int DivisionId);
        public string getSubDivisionNameById(int SubDivisionId);
        public int getLocationCode(int DivisionId);


    }
}
