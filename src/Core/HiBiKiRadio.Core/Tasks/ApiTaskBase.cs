using System.Diagnostics;

namespace SamLu.Utility.HiBiKiRadio.Tasks;

/// <summary>
/// 通过HiBiKi官方API完成的任务的基类，提供基础获取数据的方法。
/// </summary>
public abstract partial class ApiTaskBase : IDisposable
{
	/// <summary>
	/// HiBiKi官方API的主机地址。
	/// </summary>
	public static readonly Uri ApiHost = new("https://vcms-api.hibiki-radio.jp", UriKind.Absolute);
	/// <summary>
	/// HiBiKi官方服务状态API的地址。
	/// </summary>
	public static readonly Uri ServiceStatusAPI = new("https://s3-ap-northeast-1.amazonaws.com/hibiki-status/service_status.json");
	/// <summary>
	/// HiBiKi官方API的根路径。
	/// </summary>
	public static readonly Uri ApiBase = new(ApiHost, "/api/v1/");

	protected internal static readonly IDownloadClient GlobalClient =
#if NET35
		new WebClient();
#else
        new HttpClient();
#endif

	public virtual IDownloadClient Client => GlobalClient;

    protected ApiTaskBase() { }

    #region IDisposable
	protected virtual void Dispose(bool disposing) { }

    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
	#endregion

	protected virtual Task<byte[]> FetchDataAsync(Uri requestUri, CancellationToken cancellationToken)
	{
		Debug.Assert(requestUri is not null);

		return this.Client.GetDataAsync(requestUri, cancellationToken);
	}

	protected virtual Task<T> FetchAsAsync<T>(Uri requestUri, CancellationToken cancellationToken) where T : notnull
	{
		Debug.Assert(requestUri is not null);

		return Task.Factory.StartNew(() =>
		{
			var json = this.Client.GetStringAsync(requestUri, cancellationToken).Result;
			T? result;
#if NET35 || NETSTANDARD1_3
			result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
#else
			result = System.Text.Json.JsonSerializer.Deserialize<T>(json);
#endif

			return result ?? throw new InvalidOperationException("无法解析JSON。");
		}, cancellationToken);
	}
}
