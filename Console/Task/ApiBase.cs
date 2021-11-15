using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace HiBikiRadioTool.Task
{
    public abstract class ApiTaskBase
    {
        public static readonly Uri ApiHost = new Uri("https://vcms-api.hibiki-radio.jp", UriKind.Absolute);
        public static readonly Uri ServiceStatusAPI = new Uri("https://s3-ap-northeast-1.amazonaws.com/hibiki-status/service_status.json");
        public static readonly Uri ApiBase = new Uri(ApiHost, "/api/v1/");

        protected ApiTaskBase() { }

        public abstract object Run(params object[] taskParameters);

		protected virtual byte[] FetchData(Uri requestUri, Action<HttpWebRequest> requestCustomizer = null)
		{
			byte[] data;
			
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri.AbsoluteUri);
			httpWebRequest.Method = "GET";
			httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36";
			httpWebRequest.Headers["X-Requested-With"] = "XMLHttpRequest";

			requestCustomizer?.Invoke(httpWebRequest);

			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			using (Stream stream = httpWebResponse.GetResponseStream())
			{
				long contentLength = httpWebResponse.ContentLength;
				byte[] buffer = new byte[byte.MaxValue];
				using (MemoryStream ms = new MemoryStream())
				{
					while (true)
					{
						int read = stream.Read(buffer, 0, buffer.Length);
						if (read <= 0)
						{
							data = ms.ToArray();
							break;
						}
						ms.Write(buffer, 0, read);
					}
				}
			}

			return data;
		}

		protected virtual string FetchAsJson(Uri requestUri, Action<HttpWebRequest> requestCustomizer = null)
        {
			byte[] data = this.FetchData(requestUri,
				request =>
				{
					requestCustomizer?.Invoke(request);
					
					request.Accept = "application/json, text/plain, */*";
				});
			string json = Encoding.UTF8.GetString(data);
            return json;
		}

        public virtual T FetchAsObject<T>(Uri requestUri, Action<HttpWebRequest> requestCustomizer = null)
        {
            string json = this.FetchAsJson(requestUri, requestCustomizer);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
