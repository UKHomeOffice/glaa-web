using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GLAA.Services
{
    public interface IVirusScanService
    {
        Task<VirusScanResult> ScanFileAsync(IFormFile file);
    }
}
