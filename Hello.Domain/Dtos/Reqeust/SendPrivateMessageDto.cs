namespace Hello.Domain.Dtos.Reqeust
{
    public class SendPrivateMessageDto
    {
        public string? Message { get; set; }
        public string? ImageBase64 { get; set; }
        public string? UserId { get; set; }
    }
}
