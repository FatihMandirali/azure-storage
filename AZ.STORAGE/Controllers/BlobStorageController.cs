using AZ.STORAGE.Model;
using Microsoft.AspNetCore.Mvc;

namespace AZ.STORAGE.Controllers;
[ApiController]
[Route("[controller]")]
public class BlobStorageController:ControllerBase
{
    private readonly IBlobStorage _blobStorage;

    public BlobStorageController(IBlobStorage blobStorage)
    {
        _blobStorage = blobStorage;
    }
    [HttpGet]
    public async Task<List<FileBlob>> Index()
    {
        var names = _blobStorage.GetNames(ContainerEnum.pictures);
        string blobUrl = $"{_blobStorage.BlobUrl}/{ContainerEnum.pictures.ToString()}";
        var s = names.Select(x => new FileBlob { Name = x, Url = $"{blobUrl}/{x}" }).ToList();

        var log = await _blobStorage.GetLogAsync("controller.txt");
        return s;
    }
    
    [HttpPost]
    public async Task Upload(IFormFile picture)
    {
        await _blobStorage.SetLogAsync("Upload methoduna giriş yapıldı", "controller.txt");

        var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(picture.FileName);

        await _blobStorage.UploadAsync(picture.OpenReadStream(), newFileName, ContainerEnum.pictures);

        await _blobStorage.SetLogAsync("Upload methodundan çıkış yapıldı", "controller.txt");
    }
    
    [HttpGet("download")]
    public async Task<IActionResult> Download(string fileName)
    {
        var stream = await _blobStorage.DownloadAsync(fileName, ContainerEnum.pictures);

        return File(stream, "application/octet-stream", fileName);
    }

    [HttpGet("delete")]
    public async Task Delete(string fileName)
    {
        await _blobStorage.DeleteAsync(fileName, ContainerEnum.pictures);
    }
}