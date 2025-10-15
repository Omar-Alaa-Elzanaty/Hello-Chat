using System.ComponentModel.Design;

namespace Hello.Domain.Models
{
    public class Chat : BaseEntity
    {
        public string SenderId { get; set; }
        public virtual User Sender { get; set; }
        public string ReceiverId { get; set; }
        public virtual User Receiver { get; set; }
        public string? Message { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageId { get; set; }
        public DateTime? ReadAt { get; set; }
    }
}
