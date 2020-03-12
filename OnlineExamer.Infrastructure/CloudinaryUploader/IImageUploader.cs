namespace OnlineExamer.Infrastructure.CloudinaryUploader
{
    using Microsoft.AspNetCore.Http;

    public interface IImageUploader
    {
        string UploadImage(IFormFile fileform, string articleTitle);
    }
}
