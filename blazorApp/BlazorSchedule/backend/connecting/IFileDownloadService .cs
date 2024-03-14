namespace BlazorSchedule
{
    public interface IFileDownloadService
    {
        Task DownloadFile(string fileName, byte[] content, string contentType);
    }
}
