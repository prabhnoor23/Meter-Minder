using System.ComponentModel.DataAnnotations;

namespace Pspcl.Core.Domain
{
    public class Material 
    {
        [Key]
        public int Id { get; set; } // primary key
        public string Name { get; set; }

        public string? Code { get; set; }
        public int TestingCharges { get; set; }

        public MaterialType MaterialType { get; set; }

        public int MaterialTypeId { get; set; } // Foreign key to MaterialType

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

    }
}
