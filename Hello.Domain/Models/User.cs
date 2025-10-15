using Microsoft.AspNetCore.Identity;

namespace Hello.Domain.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime LastSeen { get; set; }
        public virtual ICollection<GroupMember> Groups { get; set; }
    }
}
