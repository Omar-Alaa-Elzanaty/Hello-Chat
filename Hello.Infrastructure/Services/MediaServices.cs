using Hello.Domain.Dtos.Response;
using Hello.Domain.Interfaces;
using Imagekit.Sdk;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Hello.Infrastructure.Services
{
    public class MediaServices : IMediaServices
    {
        private readonly ImagekitClient _imageKit;

        public MediaServices(IConfiguration configuration)
        {
            _imageKit = new ImagekitClient(
               configuration["ImageKit:PublicKey"],
               configuration["ImageKit:PrivateKey"],
               configuration["ImageKit:UrlEndpoint"]
               );
        }

        public async Task<UploadMediaResponseDto> UploadMediaAsync(byte[] mediaData)
        {

            FileCreateRequest ob = new FileCreateRequest
            {
                file = mediaData,
                fileName = Guid.NewGuid().ToString() + ".jpg",
            };

            Result response = await _imageKit.UploadAsync(ob);

            return new UploadMediaResponseDto()
            {
                FileId = response.fileId,
                Url = response.url
            };
        }

        public async Task<bool> DeleteMediaAsync(string fileId)
        {
            var response = await _imageKit.DeleteFileAsync(fileId);
            return response.HttpStatusCode == (int)HttpStatusCode.OK;
        }
    }
}
