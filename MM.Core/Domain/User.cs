using Microsoft.AspNetCore.Identity;

namespace Pspcl.Core.Domain
{
    public class User : IdentityUser<int>
    {
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public bool IsDeleted { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public virtual ICollection<UserRole>? UserRoles { get; set; }

    }
}
