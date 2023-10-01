using Microsoft.AspNetCore.Identity;

namespace Pspcl.Core.Domain
{
    public class Role : IdentityRole<int>
    {
        public virtual ICollection<UserRole>? UserRoles { get; set; }
    }
}
