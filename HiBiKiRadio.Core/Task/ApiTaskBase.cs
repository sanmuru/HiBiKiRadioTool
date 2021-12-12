using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SamLu.Utility.HiBiKiRadio.Task
{
	public abstract class ApiTaskBase
	{
		public static readonly Uri ApiHost = new("https://vcms-api.hibiki-radio.jp", UriKind.Absolute);
		public static readonly Uri ServiceStatusAPI = new("https://s3-ap-northeast-1.amazonaws.com/hibiki-status/service_status.json");
		public static readonly Uri ApiBase = new(ApiHost, "/api/v1/");

		private static readonly object _lock = new();
		protected static readonly HttpClient httpClient;
		static ApiTaskBase()
        {
			lock (_lock)
			{
                httpClient ??= new HttpClient(new HttpClientHandler() { UseCookies = true });
                httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
			}
        }

		protected ApiTaskBase() { }

		public abstract Task<object?> Run(params object[] taskParameters);

        public virtual async Task<byte[]> FetchData(Uri requestUri) => await ApiTaskBase.httpClient.GetByteArrayAsync(requestUri);

        public virtual async Task<T> FetchAs<T>(Uri requestUri) => await ApiTaskBase.httpClient.GetFromJsonAsync<T>(requestUri) ?? throw new JsonException("无法包装JSON。");
    }
}
