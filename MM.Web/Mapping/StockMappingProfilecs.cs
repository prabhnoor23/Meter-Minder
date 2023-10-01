using Pspcl.Web.ViewModels;
using System.Collections;
using AutoMapper;
using System.Collections.Generic;
using Pspcl.Core.Domain;
using Pspcl.Web.Models;
using Pspcl.Services.Models;

namespace Pspcl.Web.Mapping
{
    public class StockMappingProfilecs:Profile
    {
        public StockMappingProfilecs()
        {
            CreateMap <StockViewModel, Stock>();        
            CreateMap <StockIssueBook, IssueStockModel>();
            CreateMap <Stock, StockViewModel>();
            CreateMap <AddUserModel,User>();
        }
    }
}

