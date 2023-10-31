namespace AZ.STORAGE;

public enum ContainerEnum
{
    pictures,
    pdf,
    logs
}
public interface IBlobStorage
{
    public string BlobUrl { get; }
    Task UploadAsync(Stream filStream, string name, ContainerEnum containerNameEnum);
    Task<Stream> DownloadAsync(string filenName,ContainerEnum containerNameEnum);
    Task DeleteAsync(string fileName,ContainerEnum containerNameEnum);
    Task SetLogAsync(string text, string fileName);
    Task<List<string>> GetLogAsync(string fileName);
    List<string> GetNames(ContainerEnum containerNameEnum);
}