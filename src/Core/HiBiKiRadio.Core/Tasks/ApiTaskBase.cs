using System;
using System.Diagnostics;
#if NET35
using System.Net;
using Newtonsoft.Json;
#else
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace SamLu.Utility.HiBiKiRadio.Tasks
{
	/// <summary>
	/// 通过HiBiKi官方API完成的任务的基类，提供基础获取数据的方法。
	/// </summary>
	public abstract partial class ApiTaskBase
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

		/// <summary>
		/// 内部执行下载数据的客户端对象。
		/// </summary>
#if NET35
		protected static readonly WebClient client;
#else
		protected static readonly HttpClient client;
#endif

		static ApiTaskBase()
		{
#if NET35
			client ??= new();
			client.Headers.Add("X-Requested-With", "XMLHttpRequest");
#else
			client ??= new(new HttpClientHandler() { UseCookies = true });
			client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
#endif
		}

		protected ApiTaskBase() { }

		#region FetchData
		protected virtual byte[] FetchData(Uri requestUri)
		{
			Debug.Assert(requestUri is not null);

#if NET35
			return client.DownloadData(requestUri);
#else
			return client.GetByteArrayAsync(requestUri).Result;
#endif
		}

#if NET5_0_OR_GREATER
		protected virtual byte[] FetchData(Uri requestUri, CancellationToken cancellationToken)
		{
			Debug.Assert(requestUri is not null);

			return client.GetByteArrayAsync(requestUri, cancellationToken).Result;
		}
#endif

		public IAsyncResult BeginFetchData(AsyncCallback? callback, object state)
		{
			Debug.Assert(state is FetchAsyncState);
			var fetchAsyncState = (FetchAsyncState)state;

			var arguments = fetchAsyncState.Arguments;
			Debug.Assert(arguments.Length >= 1 && arguments[0] is Uri);
#if NET35
			Func<Uri, byte[]> handler = this.FetchData;
			IAsyncResult asyncResult = handler.BeginInvoke(
				(Uri)arguments[0],
				callback,
				state);
			return FetchAsyncResult.Create(handler, asyncResult);
#else
			return Task<byte[]>.Factory.ContinueWhenAll(
				new[] {
					this.FetchDataAsync((Uri)arguments[0]
#if NET5_0_OR_GREATER
						, fetchAsyncState.CancellationToken
#endif
					)
				},
				tasks =>
				{
					var task = tasks[0];
					callback?.Invoke(task);
					return task.Result;
				}
			);
#endif
		}

		public byte[] EndFetchData(IAsyncResult asyncResult)
		{
			if (asyncResult is null) throw new ArgumentNullException(nameof(asyncResult));
#if NET35
			var fetchAsyncResult = (FetchAsyncResult<Func<Uri, byte[]>>)asyncResult;
			return fetchAsyncResult.Handler.EndInvoke(fetchAsyncResult.AsyncResult);
#else
			Debug.Assert(asyncResult is Task<byte[]>);
			var task = (Task<byte[]>)asyncResult;
			task.Wait();
			return task.Result;
#endif
		}

#if !NET35
        public async Task<byte[]> FetchDataAsync(Uri requestUri)
        {
            if (requestUri is null) throw new ArgumentNullException(nameof(requestUri));

            return await Task.Run(() => this.FetchData(requestUri));
        }

#if NET5_0_OR_GREATER
        public async Task<byte[]> FetchDataAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            if (requestUri is null) throw new ArgumentNullException(nameof(requestUri));

            return await Task.Run(() => this.FetchData(requestUri, cancellationToken));
        }
#endif
#endif
        #endregion

        #region FetchAs
        protected virtual T FetchAs<T>(Uri requestUri)
		{
			Debug.Assert(requestUri is not null);

#if NET35
			var json = client.DownloadString(requestUri);
			var obj = JsonConvert.DeserializeObject<T>(json);
#else
			var obj = client.GetFromJsonAsync<T>(requestUri).Result;
#endif
			if (obj is null) throw new JsonException("无法包装JSON。");
			return obj;
		}

#if NET5_0_OR_GREATER
		protected virtual T FetchAs<T>(Uri requestUri, CancellationToken cancellationToken)
		{
			Debug.Assert(requestUri is not null);

			return client.GetFromJsonAsync<T>(requestUri, cancellationToken).Result ??
				throw new JsonException("无法解析JSON。");
		}
#endif

		public IAsyncResult BeginFetchAs<T>(AsyncCallback? callback, object state)
		{
			Debug.Assert(state is FetchAsyncState);
			var fetchAsyncState = (FetchAsyncState)state;

			var arguments = fetchAsyncState.Arguments;
			Debug.Assert(arguments.Length >= 1 && arguments[0] is Uri);
#if NET35
			Func<Uri, T> handler = this.FetchAs<T>;
			IAsyncResult asyncResult = handler.BeginInvoke(
				(Uri)arguments[0],
				callback,
				state);
			return FetchAsyncResult.Create(handler, asyncResult);
#else
			return Task<T>.Factory.ContinueWhenAll(
				new[] {
					this.FetchAsAsync<T>((Uri)arguments[0]
#if NET5_0_OR_GREATER
						, fetchAsyncState.CancellationToken
#endif
					)
				},
				tasks =>
				{
					var task = tasks[0];
					callback?.Invoke(task);
					return task.Result;
				}
			);
#endif
		}

		public T EndFetchAs<T>(IAsyncResult asyncResult)
		{
			if (asyncResult is null) throw new ArgumentNullException(nameof(asyncResult));
#if NET35
			var fetchAsyncResult = (FetchAsyncResult<Func<Uri, T>>)asyncResult;
			return fetchAsyncResult.Handler.EndInvoke(fetchAsyncResult.AsyncResult);
#else
			Debug.Assert(asyncResult is Task<T>);
			var task = (Task<T>)asyncResult;
			task.Wait();
			return task.Result;
#endif
		}

#if !NET35
		public async Task<T> FetchAsAsync<T>(Uri requestUri)
        {
            if (requestUri is null) throw new ArgumentNullException(nameof(requestUri));

            return await Task.Run(() => FetchAs<T>(requestUri));
        }

#if NET5_0_OR_GREATER
        public async Task<T> FetchAsAsync<T>(Uri requestUri, CancellationToken cancellationToken)
        {
            if (requestUri is null) throw new ArgumentNullException(nameof(requestUri));

            return await Task.Run(() => FetchAs<T>(requestUri, cancellationToken));
        }
#endif
#endif
        #endregion
    }
}
