using Hello.Domain.Dtos.Response;

namespace Hello.Domain.Interfaces
{
    public interface IMediaServices
    {
        Task<UploadMediaResponseDto> UploadMediaAsync(byte[] mediaData);
        Task<bool> DeleteMediaAsync(string fileId);
    }
}
