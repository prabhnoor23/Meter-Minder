using System.ComponentModel.DataAnnotations;

namespace Pspcl.Core.Domain
{
    public class SubDivision
    {
        [Key]
        public int Id { get; set; } // primary key
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
        public int DivisionId { get; set; }
    }
}
