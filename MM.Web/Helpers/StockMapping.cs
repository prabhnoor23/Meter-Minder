using AutoMapper;
using Pspcl.Core.Domain;
using Pspcl.Web.Models;
using System.Runtime.InteropServices;

namespace Pspcl.Web.Helpers
{
    public class StockMapping :Profile
    {
        public StockMapping()
        {
            CreateMap<Entity,StockViewModel>(); 
        }
    }
}
