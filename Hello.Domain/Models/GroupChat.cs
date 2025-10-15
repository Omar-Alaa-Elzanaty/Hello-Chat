namespace Hello.Domain.Models
{
    public class GroupChat : BaseEntity
    {
        public string SenderId { get; set; }
        public virtual User Sender { get; set; }
        public string? Message { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageId { get; set; }
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}
