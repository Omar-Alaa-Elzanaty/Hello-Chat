namespace Hello.Domain.Models
{
    public class GroupMember
    {
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        public string MemberId { get; set; }
        public virtual User Member { get; set; }
        public bool IsAdmin { get; set; }
    }
}