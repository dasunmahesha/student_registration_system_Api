using Amazon.S3.Model;
using Amazon.S3;
using Amazon;

namespace StudentRegApi.Services { 
public class S3Service
{
    private readonly IConfiguration _configuration;
    private readonly IAmazonS3 _s3Client;

    public S3Service(IConfiguration configuration)
    {
        _configuration = configuration;
        _s3Client = new AmazonS3Client(
            _configuration["AWS:AccessKey"],
            _configuration["AWS:SecretKey"],
            RegionEndpoint.GetBySystemName(_configuration["AWS:Region"])
        );
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file), "The file cannot be null.");
            }
            var uploadRequest = new PutObjectRequest
        {
            InputStream = file.OpenReadStream(),
            BucketName = _configuration["AWS:BucketName"],
            Key = "studentmanagementimages/" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName),
            ContentType = file.ContentType
        };

        var response = await _s3Client.PutObjectAsync(uploadRequest);
        return $"https://{_configuration["AWS:BucketName"]}.s3.{_configuration["AWS:Region"]}.amazonaws.com/{uploadRequest.Key}";
    }
}

}