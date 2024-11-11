using System.Text.Json;
using System.Threading.Tasks;
using BlazorSchedule;
using Microsoft.JSInterop;

namespace BlazorSchedule
{
    public class FileDownloadService : IFileDownloadService
    {
        private readonly IJSRuntime _jsRuntime;

        public FileDownloadService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task DownloadFile(string fileName, byte[] content, string contentType)
        {
            await _jsRuntime.InvokeVoidAsync("BlazorDownloadFile.saveAsFile",
                fileName,
                Convert.ToBase64String(content),
                contentType);
        }
    }
}
