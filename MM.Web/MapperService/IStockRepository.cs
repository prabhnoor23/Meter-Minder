using Pspcl.Core.Domain;
using Pspcl.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pspcl.Services
{
    public interface IStockRepository
    {
        IEnumerable<Stock> GetAll();
    }
}
