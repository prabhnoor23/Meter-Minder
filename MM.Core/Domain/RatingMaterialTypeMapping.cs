
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Pspcl.Core.Domain
{
    public class RatingMaterialTypeMapping
    {
        [Key]
        public int Id { get; set; }
        public int MaterialTypeId { get; set; }
        public int RatingId { get; set; }
    }
}
