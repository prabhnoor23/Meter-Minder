using JasperFx.Core;
using Pspcl.Core.Domain;
using Pspcl.DBConnect;
using Pspcl.Services;


namespace Pspcl.Web.MapperService
{
    public class StockRepository:IStockRepository
    {
        private readonly ApplicationDbContext _context;
        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
                        
        }

        public IEnumerable<Stock> GetAll()
        {
            return _context.Set<Stock>().ToList();
        }
    }
}
