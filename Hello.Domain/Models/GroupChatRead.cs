namespace Hello.Domain.Models
{
    public class GroupChatRead
    {
        public int GroupChatId { get; set; }
        public virtual GroupChat GroupChat { get; set; }
        public string MemberId { get; set; }
        public virtual User Member { get; set; }
        public DateTime ReadAt { get; set; }
    }
}
