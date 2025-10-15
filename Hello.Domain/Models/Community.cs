namespace Hello.Domain.Models
{
    public class Community : BaseEntity
    {
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public string CreatedBy { get; set; }
        public virtual ICollection<CommunityChat> Chats { get; set; }
    }
}
