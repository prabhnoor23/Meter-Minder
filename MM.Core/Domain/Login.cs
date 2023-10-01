using System.ComponentModel.DataAnnotations;

namespace Pspcl.Core.Domain
{
    public class Login
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime? LoginDate { get; set; }
        public string IpAddress { get; set; }
    }
}
