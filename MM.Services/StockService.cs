
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Pspcl.Core.Domain;
using Pspcl.DBConnect;
using Pspcl.Services.Interfaces;
using Pspcl.Services.Models;
using Pspcl.Services.Options;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Security.Policy;
using static System.Collections.Specialized.BitVector32;

namespace Pspcl.Services
{

    public class StockService : IStockService
    {
        
        
        private readonly ApplicationDbContext _dbcontext;
  
        public StockService(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
        }
        public List<MaterialGroup> GetAllMaterialGroups(bool? onlyActive = false)
        {
            if (!onlyActive.HasValue)
            {
                return _dbcontext.MaterialGroup.ToList();
            }
            return _dbcontext.MaterialGroup.Where(x => (onlyActive.Value && x.IsActive) || (!onlyActive.Value)).ToList();
        }
        public List<MaterialType> GetAllMaterialTypes(int materialGroupId, bool? onlyActive = false)
        {
            if (!onlyActive.HasValue)
            {
                return _dbcontext.MaterialType.Where(x => x.MaterialGroupId == materialGroupId).ToList();
            }
            var materialTypes = _dbcontext.MaterialType.Where(x => (onlyActive.Value && x.IsActive) || (!onlyActive.Value)).ToList();
            return materialTypes.Where(x => x.MaterialGroupId == materialGroupId).ToList();

        }
        public List<Tuple<int, string>> GetAllMaterialRatings(int materialTypeId)
        {
            return _dbcontext.Rating
            .Join(_dbcontext.RatingMaterialTypeMapping, rating => rating.Id, mapping => mapping.RatingId, (rating, mapping) => new { rating, mapping })
            .Where(joinResult => joinResult.mapping.MaterialTypeId == materialTypeId)
           .Select(joinResult => Tuple.Create(joinResult.rating.Id, joinResult.rating.Name))
            .ToList();
        }

        public List<Material> GetAllMaterialCodes(int materialTypeId, bool? onlyActive = false)
        {
            if (!onlyActive.HasValue)
            {
                return _dbcontext.Material.Where(x => x.MaterialTypeId == materialTypeId).ToList();
            }
            var materialCodes = _dbcontext.Material.Where(x => (onlyActive.Value && x.IsActive) || (!onlyActive.Value)).ToList();
            return materialCodes.Where(x => x.MaterialTypeId == materialTypeId).ToList();
        }

        public List<SubDivision> GetAllSubDivisions(bool? onlyActive = false)
        {
            if (!onlyActive.HasValue)
            {
                return _dbcontext.SubDivision.ToList();
            }
            return _dbcontext.SubDivision.Where(x => (onlyActive.Value && x.IsActive) || (!onlyActive.Value)).ToList();
        }
        public List<string> GetCircleAndDivisionAndLocationCode(int selectedSubDivId, bool? onlyActive = false)
        {
            if (onlyActive.HasValue)
            {
                SubDivision subDivision = _dbcontext.SubDivision.FirstOrDefault(x => x.Id == selectedSubDivId);
                if (subDivision != null)
                {
                    int divId = subDivision.DivisionId;
                    string divisionId = divId.ToString();

                    Division Division = _dbcontext.Division.FirstOrDefault(x => x.Id == divId);
                    string divisionName = Division.Name.ToString();
                    string locationCode = Division.LocationCode.ToString();

                    int circleDiv = Division.CircleId;
                    string circleId = circleDiv.ToString();
                    Circle Circle = _dbcontext.Circle.FirstOrDefault(x => x.Id == circleDiv);
                    string circleName = Circle.Name.ToString();

                    List<string> DivisionCircle = new List<string>();
                    DivisionCircle.Add(divisionName);
                    DivisionCircle.Add(circleName);
                    DivisionCircle.Add(divisionId);
                    DivisionCircle.Add(circleId);
                    DivisionCircle.Add(locationCode);

                    return DivisionCircle;
                }
            }
            return new List<string>();
        }
        
        public int AddStock(Stock stock)
        {
            _dbcontext.Set<Stock>().Add(stock);
            _dbcontext.SaveChanges();
            return stock.Id;
        }
        public int AddStockMaterial(StockMaterial stockMaterial)
        {
            _dbcontext.Set<StockMaterial>().Add(stockMaterial);
            _dbcontext.SaveChanges();
            return stockMaterial.Id;
        }
        public void AddStockMaterialSeries(StockMaterialSeries stockMaterialSeries)
        {
            _dbcontext.Set<StockMaterialSeries>().AddRange(stockMaterialSeries);
            _dbcontext.SaveChanges();
        }
        public List<StockInModel> GetStockInModels()
        {
            var stockInModels = _dbcontext.Stock
               .Select(s => new StockInModel
               {
                   Stock = s,
                   MaterialGroup = _dbcontext.MaterialGroup.Where(mg => mg.Id == s.MaterialGroupId).Select(mg => mg.Name).FirstOrDefault(),
                   MaterialName = _dbcontext.MaterialType.Where(mt => mt.Id == s.MaterialTypeId).Select(mt => mt.Name).FirstOrDefault(),
                   MaterialCode = _dbcontext.Material.Where(mt => mt.Id == s.MaterialId).Select(mt => mt.Code).FirstOrDefault(),
                   Quantity = _dbcontext.StockMaterial.Where(sm => sm.StockId == s.Id).Sum(sm => sm.Quantity)
               })
               .ToList();
            stockInModels.RemoveAll(s => s.Quantity == 0);

            return stockInModels;
        }
        public List<AvailableStockModel> GetAvailableStock()
        {
            var availableStocks = _dbcontext.StockMaterial
                .Select(sm => new AvailableStockModel
                {
                    StockMaterial = sm,


                    StockMaterialId = sm.Id,


                    MaterialGroup = _dbcontext.MaterialGroup.Where(mg => mg.Id == _dbcontext.Stock.Where(s => s.Id == sm.StockId).Select(s => s.MaterialGroupId).FirstOrDefault()).Select(mg => mg.Name)
                    .FirstOrDefault(),
                    MaterialName = _dbcontext.MaterialType.Where(mt => mt.Id == _dbcontext.Stock.Where(s => s.Id == sm.StockId).Select(s => s.MaterialTypeId).FirstOrDefault()).Select(mt => mt.Name)
                    .FirstOrDefault(),
                    MaterialCode = _dbcontext.Material.Where(mt => mt.Id == _dbcontext.Stock.Where(s => s.Id == sm.StockId).Select(s => s.MaterialId).FirstOrDefault()).Select(mt => mt.Code)
                    .FirstOrDefault(),

                    grnNo = _dbcontext.Stock.Where(s => s.Id == sm.StockId).Select(s => s.GrnNumber).FirstOrDefault(),
                    grnDate = (DateTime)_dbcontext.Stock.Where(s => s.Id == sm.StockId).Select(s => s.GrnDate).FirstOrDefault(),
                    Rate = (float)_dbcontext.Stock.Where(s => s.Id == sm.StockId).Select(s => s.Rate).FirstOrDefault(),
                    Make = _dbcontext.Stock.Where(s => s.Id == sm.StockId).Select(s => s.Make).FirstOrDefault(),
                    AvailableQuantity = _dbcontext.StockMaterialSeries.Count(sms => sms.StockMaterialId == sm.Id && sms.IsIssued == false && sms.IsDeleted == false),
                    SrNoTo = sm.SerialNumberTo,
                    SrNoFrom = sm.SerialNumberTo - _dbcontext.StockMaterialSeries.Count(sms => sms.StockMaterialId == sm.Id && sms.IsIssued == false && sms.IsDeleted == false) + 1,


                })
                .ToList();
            availableStocks.RemoveAll(sm => sm.AvailableQuantity == 0);
            foreach (var stock in availableStocks)
            {
                stock.Value = stock.Rate * stock.AvailableQuantity;
            }


            return availableStocks;
        }


        public List<StockOutModel> GetStockOutModels()
        {
            var stockIssueBookModels = _dbcontext.StockIssueBook
                .Join(_dbcontext.StockBookMaterial,
                    sib => sib.Id,
                    sbm => sbm.StockIssueBookId,
                    (sib, sbm) => new StockOutModel
                    {

                        TransactionId = sbm.Id,
                        CurrentDate = sib.CurrentDate,
                        SrNoDate = sib.SrNoDate,
                        SerialNumber = sib.SerialNumber,
                        DivisionName = _dbcontext.Division.Where(d => d.Id == sib.DivisionId).Select(d => d.Name).FirstOrDefault(),
                        LocationID = _dbcontext.Division.Where(d => d.Id == sib.DivisionId).Select(d => d.LocationCode).FirstOrDefault(),
                        SubDivisionName = _dbcontext.SubDivision.Where(sd => sd.Id == sib.SubDivisionId).Select(sd => sd.Name).FirstOrDefault(),
                        JuniorEngineerName = sib.JuniorEngineerName,
                        MaterialName = _dbcontext.Material.Where(m => m.Id == sbm.MaterialId).Select(m => m.Name).FirstOrDefault(),
                        MaterialCode = _dbcontext.Material.Where(m => m.Id == sbm.MaterialId).Select(m => m.Code).FirstOrDefault(),
                        MaterialId = _dbcontext.Material.Where(m => m.Id == sbm.MaterialId).Select(m => m.Id).FirstOrDefault(),
                        Rate = _dbcontext.Stock.Where(s => s.MaterialId == sbm.MaterialId).Select(s => s.Rate).FirstOrDefault(),
                        Quantity = sbm.Quantity,
                       
                        SrControlNumber=sib.SrControlNumber,
                        Make = sbm.Make,
                        ImageName = sib.Image
                    })
                .ToList();

            foreach (var stock in stockIssueBookModels)
            {
                stock.Cost = GetCost(stock.MaterialId, stock.Quantity);
            }

            return stockIssueBookModels;
        }
        public string GetMaterialGroupById(int? materialGroupId)
        {
            var response = _dbcontext.MaterialGroup.Where(x => x.Id == materialGroupId).Select(x => x.Name).FirstOrDefault();
            string materialGroupName = response.ToString();
            return materialGroupName;
        }
        public string GetMaterialTypeById(int? materialTypeId)
        {
            var response = _dbcontext.MaterialType.Where(x => x.Id == materialTypeId).Select(x => x.Name).FirstOrDefault();
            string materialTypeName = response.ToString();
            return materialTypeName;
        }
        public string GetMaterialCodeById(int? materialIdByCode)
        {
            var response = _dbcontext.Material.Where(x => x.Id == materialIdByCode).Select(x => x.Code).FirstOrDefault();
            if (response == null)
            {
                return "None";
            }
            string materialCodeName = response.ToString();
            return materialCodeName;
        }
        public string GetRatingNameById(int? ratingId)
        {
            var response = _dbcontext.Rating.Where(x => x.Id == ratingId).Select(x => x.Name).FirstOrDefault();
            if (response == null)
            {
                return "None";
            }
            return response;
        }

        public void UpdateStockMaterialSeries(List<List<int>> requiredIssueData)
        {
            foreach (var Item in requiredIssueData)
            {
                // Get the StockMaterialSeries records that meet the specified conditions
                var recordsToUpdate = _dbcontext.StockMaterialSeries.Where(x => x.StockMaterialId == Item[0] && x.SerialNumber >= Item[1] && x.SerialNumber <= Item[2]);

                // Loop through each record and update the abc column value to 1
                foreach (var record in recordsToUpdate)
                {
                    record.ModifiedOn = DateTime.Now;
                    record.IsIssued = true;
                }
                _dbcontext.SaveChanges();
            }
        }
        public int IssueStock(StockIssueBook stockIssueBook)
        {
            _dbcontext.Set<StockIssueBook>().Add(stockIssueBook);

            _dbcontext.SaveChanges();
            return stockIssueBook.Id;
        }
        public void StockBookMaterial(StockBookMaterial stockBookMaterial)
        {
            _dbcontext.Set<StockBookMaterial>().Add(stockBookMaterial);
            _dbcontext.SaveChanges();
            return;
        }
        public Dictionary<String, int> AllMakesAndQuantities(int materialGroupId, int materialTypeId, int materialId)
        {
            List<Stock> stocks = _dbcontext.Stock.Where(x => x.MaterialGroupId == materialGroupId && x.MaterialTypeId == materialTypeId && x.MaterialId == materialId).ToList();
            List<string> distinctMakes = stocks.Select(x => x.Make).Distinct().ToList();

            Dictionary<string, List<int>> makeWithStockIds = new Dictionary<string, List<int>>();
            List<int> stockId = new List<int>();
            foreach (string make in distinctMakes)
            {
                stocks = _dbcontext.Stock.Where(x => x.MaterialGroupId == materialGroupId && x.MaterialTypeId == materialTypeId && x.MaterialId == materialId && x.Make == make).ToList();
                stockId = stocks.Select(x => x.Id).ToList();

                makeWithStockIds.Add(make, stockId);
            }

            Dictionary<String, int> makesAndQuantities = new Dictionary<String, int>();

            foreach (KeyValuePair<string, List<int>> keyValuePair in makeWithStockIds)
            {
                string Make = keyValuePair.Key;
                List<int> stockIdList = keyValuePair.Value.ToList();

                var query = _dbcontext.StockMaterial.Where(x => stockIdList.Contains(x.StockId)).Select(x => x.Id);
                List<int> stockMaterialIdsList = query.ToList();

                List<StockMaterialSeries> Materials = _dbcontext.StockMaterialSeries.Where(x => stockMaterialIdsList.Contains(x.StockMaterialId) && !x.IsIssued && !x.IsDeleted).ToList();
                List<int> idList = Materials.Select(x => x.Id).ToList();
                int QuantityAgainstMake = idList.Count();

                makesAndQuantities.Add(Make, QuantityAgainstMake);
            }
            foreach (KeyValuePair<string, int> makeAndQuantity in makesAndQuantities)
            {
                if (makeAndQuantity.Value == 0)
                {
                    makesAndQuantities.Remove(makeAndQuantity.Key);
                }
            }
            return makesAndQuantities;
        }

        public Dictionary<string, List<List<int>>> GetAvailableMakesAndRows(int materialGroupId, int materialTypeId, int materialId)
        {
            Dictionary<string, List<List<int>>> availableMakesAndRows = new Dictionary<string, List<List<int>>>();

            List<Stock> stocks = _dbcontext.Stock.Where(x => x.MaterialGroupId == materialGroupId && x.MaterialTypeId == materialTypeId && x.MaterialId == materialId).ToList();
            List<string> distinctMakes = stocks.Select(x => x.Make).Distinct().ToList();

            Dictionary<string, List<int>> makeWithStockIds = new Dictionary<string, List<int>>();
            List<int> stockId = new List<int>();
            foreach (string make in distinctMakes)
            {
                stocks = _dbcontext.Stock.Where(x => x.MaterialGroupId == materialGroupId && x.MaterialTypeId == materialTypeId && x.MaterialId == materialId && x.Make == make).ToList();
                stockId = stocks.Select(x => x.Id).ToList();

                makeWithStockIds.Add(make, stockId);
            }
            foreach (KeyValuePair<string, List<int>> keyValuePair in makeWithStockIds)
            {
                string Make = keyValuePair.Key;
                List<int> stockIdList = keyValuePair.Value.ToList();

                var query = _dbcontext.StockMaterial.Where(x => stockIdList.Contains(x.StockId)).Select(x => x.Id);
                List<int> stockMaterialIdsList = query.ToList();

                List<StockMaterialSeries> Materials = _dbcontext.StockMaterialSeries.Where(x => stockMaterialIdsList.Contains(x.StockMaterialId) && !x.IsIssued).ToList();
                List<int> idList = Materials.Select(x => x.Id).ToList();


                var materialRanges = Materials.GroupBy(ms => ms.StockMaterialId).Select(g => new
                {
                    StockMaterialId = g.Key,
                    SrNoFrom = g.OrderBy(ms => ms.SerialNumber).First().SerialNumber,
                    SrNoTo = g.OrderBy(ms => ms.SerialNumber).Last().SerialNumber
                }).ToList();

                List<List<int>> ranges = materialRanges.Select(x => new List<int> { x.StockMaterialId, x.SrNoFrom, x.SrNoTo, (x.SrNoTo - x.SrNoFrom + 1) }).ToList();

                availableMakesAndRows.Add(Make, ranges);

                List<string> keysToRemove = new List<string>();

                foreach (KeyValuePair<string, List<List<int>>> MakeAndRows in availableMakesAndRows)
                {
                    if (MakeAndRows.Value.Count == 0)
                    {
                        keysToRemove.Add(MakeAndRows.Key);
                    }
                }

                foreach (string key in keysToRemove)
                {
                    availableMakesAndRows.Remove(key);
                }

            }
            return availableMakesAndRows;
        }
        public string GetCorrespondingMakeValue(string invoiceNumber)
        {
            List<Stock> stocks = _dbcontext.Stock.Where(x => x.InvoiceNumber == invoiceNumber).ToList();
            if (stocks.Count == 0)
            {
                return "Enter Make";
            }

            else 
            {
                string Make = stocks.Select(x => x.Make).FirstOrDefault().ToString();
                List<int> stockIdList = stocks.Select(x => x.Id).Distinct().ToList();

                var query = _dbcontext.StockMaterial.Where(x => stockIdList.Contains(x.StockId)).Select(x => x.Id).Distinct();
                List<int> stockMaterialIdsList = query.ToList();

                List<StockMaterialSeries> Materials = _dbcontext.StockMaterialSeries.Where(x => stockMaterialIdsList.Contains(x.StockMaterialId) && !x.IsDeleted).ToList();
                List<int> idList = Materials.Select(x => x.Id).ToList();

                if (idList.Count > 0)
                {
                    return Make;
                }

                else
                {
                    return "Enter Make";
                }
                
            }
            
        }

        public bool isGrnNumberExist(string GrnNumber)
        {
            //check if there are any stock records with same GRN number.
            List<Stock> stocks = _dbcontext.Stock.Where(x => x.GrnNumber == GrnNumber).ToList();


            if (stocks.Count == 0)
            {
                return false;
            }
            else
            {
                List<int> stockIdList = stocks.Select(x => x.Id).Distinct().ToList();

                var query = _dbcontext.StockMaterial.Where(x => stockIdList.Contains(x.StockId)).Select(x => x.Id).Distinct();
                List<int> stockMaterialIdsList = query.ToList();

                // if yes, check if their IsDeleted is 0, if yes, this means there are records, present with same GRN, and those are not deleted.

                List<StockMaterialSeries> Materials = _dbcontext.StockMaterialSeries.Where(x => stockMaterialIdsList.Contains(x.StockMaterialId) && !x.IsDeleted).ToList();
                List<int> idList = Materials.Select(x => x.Id).ToList();


                if (idList.Count > 0)
                {
                    return true;
                }
                return false;

            }
        }

        public bool srNoValidationInDatabase(List<int> serialNumbers, int materialGroupId, int materialTypeId, int materialId, string make)
        {

            List<Stock> stocks = _dbcontext.Stock.Where(x => x.MaterialGroupId == materialGroupId && x.MaterialTypeId == materialTypeId && x.MaterialId == materialId && x.Make == make).ToList();
            List<int> stockId = stocks.Select(x => x.Id).ToList();

            List<StockMaterial> stockMaterial = _dbcontext.StockMaterial.Where(x => stockId.Contains(x.StockId)).ToList();
            List<int> stockMaterialId = stockMaterial.Select(x => x.Id).ToList();


            List<StockMaterialSeries> stockMaterialSeries = _dbcontext.StockMaterialSeries.Where(x => stockMaterialId.Contains(x.StockMaterialId)).ToList();
            List<int> SerialNumberList = stockMaterialSeries.Select(x => x.SerialNumber).ToList();

            bool isContained = SerialNumberList.Any(x => serialNumbers.Contains(x));

            if (isContained)
            {

                return true;
            }
            return false;
        }

        public float GetCost(int materialId, int noOfUnits)
        {
            List<Material> material = _dbcontext.Material.Where(x => x.Id == materialId).ToList();
            float testingCharges = material.Select(x => x.TestingCharges).First();

            List<Stock> stocks = _dbcontext.Stock.Where(x => x.MaterialId == materialId).ToList();
            float rate = (float)(stocks.Select(x => x.Rate).First());

            float totalCost = (rate + (3 * rate / 100) + testingCharges) * noOfUnits;
            totalCost = (float)Math.Round(totalCost * 100f) / 100f;

            return totalCost;
        }

        public void UpdateIsDeletedColumn(List<List<int>> selectedRowsToDelete)
        {
            foreach (var Item in selectedRowsToDelete)
            {
                var recordsToUpdate = _dbcontext.StockMaterialSeries.Where(x => x.StockMaterialId == Item[0] && x.SerialNumber >= Item[1] && x.SerialNumber <= Item[2]);
                foreach (var record in recordsToUpdate)
                {
                    record.IsDeleted = true;
                    record.ModifiedOn = DateTime.Now;
                }
                _dbcontext.SaveChanges();

            }
            
        }

        public void UpdateStockMaterial(List<List<int>> selectedRowsToDelete)
        {
            foreach (var Item in selectedRowsToDelete)
            {
                var recordsToUpdate = _dbcontext.StockMaterial.Where(x => x.Id == Item[0] && x.SerialNumberTo == Item[2]);
                foreach (var record in recordsToUpdate)
                {
                    record.ModifiedOn = DateTime.Now;
                    if (record.SerialNumberFrom == Item[1])
                    {
                        _dbcontext.StockMaterial.Remove(record);

                    }
                    else
                    {
                        record.SerialNumberTo = Item[1] - 1;
                        record.Quantity = record.SerialNumberTo - record.SerialNumberFrom + 1;
                    }
                    record.ModifiedOn = DateTime.Now;

                }

            }
            _dbcontext.SaveChanges();
            
        }   
        
        public string getSubDivisionNameById(int SubDivisionId)
        {
            return _dbcontext.SubDivision.Where(sd => sd.Id == SubDivisionId).Select(sd => sd.Name).FirstOrDefault(); 
        }
        public string getDivisionNameById(int DivisionId)
        {
            return _dbcontext.Division.Where(sd => sd.Id == DivisionId).Select(sd => sd.Name).FirstOrDefault(); 
        }
        public string getCircleNameById(int CircleId)
        {
            return _dbcontext.Circle.Where(sd => sd.Id == CircleId).Select(sd => sd.Name).FirstOrDefault(); 
        }
        public int getLocationCode(int DivisionId)
        {
            return _dbcontext.Division.Where(sd => sd.Id == DivisionId).Select(sd => sd.LocationCode).FirstOrDefault(); 
        }

    }


}

