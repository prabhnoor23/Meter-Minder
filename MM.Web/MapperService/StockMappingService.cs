using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pspcl.Core.Domain;
using Pspcl.Web.MapperService;
using Pspcl.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;


namespace Pspcl.Services
{
    public class StockMappingservice 
    {
        private readonly IMapper _mapper;
        private readonly IStockRepository _stockRepository;




        public StockMappingservice(IMapper mapper, IStockRepository stockRepository)
        {

            _mapper = mapper;
            _stockRepository = stockRepository;


        }

      

        public IEnumerable<StockViewModel> GetAll()
        {
            var entries = _stockRepository.GetAll();
            var models=_mapper.Map<IEnumerable<StockViewModel>>(entries);

            return models;
        }

    }
}
    
    

