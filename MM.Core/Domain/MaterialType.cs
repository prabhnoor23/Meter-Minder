using System.ComponentModel.DataAnnotations;

namespace Pspcl.Core.Domain
{
    public class MaterialType
    {
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }

		public bool IsActive { get; set; }

		public bool IsDeleted { get; set; }

		public DateTime? CreatedOn { get; set; }

		public DateTime? ModifiedOn { get; set; }
		//public string? Rating { get; set; }
		public int MaterialGroupId { get; set; }

	}
}
