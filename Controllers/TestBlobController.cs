using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/test-blob")]
public class TestBlobController : ControllerBase
{
    private readonly IConfiguration _config;

    public TestBlobController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        var connectionString = _config.GetConnectionString("AzureStorage");
        var containerName = "ticket-attachments";

        var blobServiceClient = new BlobServiceClient(connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        var blobClient = containerClient.GetBlobClient(file.FileName);

        await using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: true);

        return Ok(new
        {
            message = "Uploaded successfully",
            blobUrl = blobClient.Uri.ToString(),
            fileName = file.FileName
        });
    }
}