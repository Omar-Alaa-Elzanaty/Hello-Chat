namespace Hello.Domain.Dtos.Response
{
    public class ReceivePrivateMessageDto
    {
        public string Name { get; set; }
        public int MessageId { get; set; }
        public string? Message { get; set; }
        public string? ImageUrl { get; set; }
    }
}
