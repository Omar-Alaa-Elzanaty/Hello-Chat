namespace Hello.Domain.Models
{
    public class Group : BaseEntity
    {
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public string CreatedBy { get; set; }
        public virtual ICollection<GroupChat> Chats { get; set; }
        public virtual ICollection<GroupMember> Members { get; set; }
    }
}
