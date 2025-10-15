namespace Hello.Domain.Models
{
    public class CommunityChat : BaseEntity
    {
        public string? Message { get; set; }
        public string? ImageUrl { get; set; }
        public string SenderId { get; set; }
        public virtual User Sender { get; set; }
        public int CommunityId { get; set; }
        public virtual Community Community { get; set; }
    }
}
