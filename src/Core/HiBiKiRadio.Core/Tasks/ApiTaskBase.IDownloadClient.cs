namespace SamLu.Utility.HiBiKiRadio.Tasks;

partial class ApiTaskBase
{
    public interface IDownloadClient : IDisposable
    {
        Task<byte[]> GetDataAsync(Uri requestUri, CancellationToken cancellationToken = default);
        Task<string> GetStringAsync(Uri requestUri, CancellationToken cancellationToken = default);
        Task GetFileAsync(Uri requestUri, string fileName, CancellationToken cancellationToken = default);
    }
}
