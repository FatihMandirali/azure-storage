using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;

namespace AZ.STORAGE.Services;

public class BlobStorageService:IBlobStorage
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageService()
    {
        var connectionString = "DefaultEndpointsProtocol=https;AccountName=teststorageaccountfm;AccountKey=LSZswUwYWXX9fbiG4IUE+A00LQUC6oKqIazJGcy1spibhbv8tjyHDwQINmQMwRTlsfRchpQFWWCP+AStcrz2Vw==;EndpointSuffix=core.windows.net";
        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public string BlobUrl => "https://teststorageaccountfm.blob.core.windows.net";
    public async Task UploadAsync(Stream fileStream, string fileName, ContainerEnum containerNameEnum)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerNameEnum.ToString());

        await containerClient.CreateIfNotExistsAsync();

        await containerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);

        var blobClient = containerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(fileStream,true);
    }

    public async Task<Stream> DownloadAsync(string fileName, ContainerEnum containerNameEnum)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerNameEnum.ToString());

        var blobClient = containerClient.GetBlobClient(fileName);

        var info = await blobClient.DownloadAsync();

        return info.Value.Content;
    }

    public async Task DeleteAsync(string fileName, ContainerEnum containerNameEnum)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerNameEnum.ToString());

        var blobClient = containerClient.GetBlobClient(fileName);

        await blobClient.DeleteAsync();
    }

    public async Task SetLogAsync(string text, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerEnum.logs.ToString());

        var appendBlobClient = containerClient.GetAppendBlobClient(fileName);

        await appendBlobClient.CreateIfNotExistsAsync();

        using (MemoryStream ms = new MemoryStream())
        {
            using (StreamWriter sw = new StreamWriter(ms))
            {
                sw.Write($"{DateTime.Now}: {text}\n");

                sw.Flush();
                ms.Position = 0;

                await appendBlobClient.AppendBlockAsync(ms);
            }
        }
    }

    public async Task<List<string>> GetLogAsync(string fileName)
    {
        List<string> logs = new List<string>();
        var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerEnum.logs.ToString());

        await containerClient.CreateIfNotExistsAsync();

        var appendBlobClient = containerClient.GetAppendBlobClient(fileName);

        await appendBlobClient.CreateIfNotExistsAsync();

        var info = await appendBlobClient.DownloadAsync();

        using (StreamReader sr = new StreamReader(info.Value.Content))
        {
            string line = string.Empty;

            while ((line = sr.ReadLine()) != null)
            {
                logs.Add(line);
            }
        }
        return logs;
    }

    public List<string> GetNames(ContainerEnum containerNameEnum)
    {
        List<string> blobNames = new List<string>();

        var containerClient = _blobServiceClient.GetBlobContainerClient(containerNameEnum.ToString());

        var blobs = containerClient.GetBlobs();

        blobs.ToList().ForEach(x =>
        {
            blobNames.Add(x.Name);
        });

        return blobNames;
    }
}