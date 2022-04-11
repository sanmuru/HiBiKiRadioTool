using SamLu.Utility.HiBiKiRadio.M3U8;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
#if !NET35
using System.Threading;
using System.Threading.Tasks;
#endif

namespace SamLu.Utility.HiBiKiRadio.Tasks
{
    public class PlaylistTask : ApiTaskBase
    {
        public sealed class DownloadSettings
        {
            public string OutputPath { get; set; }
            public string TempPath { get; set; }

            /// <summary>
            /// 使用指定的输出目录初始化 <see cref="DownloadSettings"/> 的实例。
            /// </summary>
            /// <param name="path">下载的文件的输出目录。</param>
            public DownloadSettings(string path) => this.OutputPath = path ?? throw new ArgumentNullException(nameof(path));
        }

        #region Download
#if NET5_0_OR_GREATER
        protected virtual IEnumerable<Stream> DownloadCore(Uri hls, DownloadSettings settings) =>
            this.DownloadCore(hls, settings, default);
#endif

        protected virtual IEnumerable<Stream> DownloadCore(Uri hls, DownloadSettings settings
#if NET5_0_OR_GREATER
            , CancellationToken cancellationToken
#endif
            )
        {
            Debug.Assert(hls is not null);
            Debug.Assert(settings is not null);

#if !NET5_0_OR_GREATER
            var pair = this.DownloadM3U8(hls, settings);
#else
            var pair = this.DownloadM3U8(hls, settings, cancellationToken);
#endif
            var m3u8Uri = pair.Key;
            var m3u8Document = pair.Value;

            foreach (var clip in m3u8Document.MediaClips)
            {
#if !NET5_0_OR_GREATER
                yield return this.DownloadClip(m3u8Uri, clip);
#else
                yield return this.DownloadClip(m3u8Uri, clip, cancellationToken);
#endif
            }
        }

        public IEnumerable<Stream> Download(Uri hls, DownloadSettings settings)
        {
            if (hls is null) throw new ArgumentNullException(nameof(hls));
            if (settings is null) throw new ArgumentNullException(nameof(settings));

            return this.DownloadCore(hls, settings);
        }

        public IAsyncResult BeginDownload(AsyncCallback callback, object state)
        {
            Debug.Assert(state is FetchAsyncState);
            var fetchAsyncState = (FetchAsyncState)state;

            var arguments = fetchAsyncState.Arguments;
            Debug.Assert(arguments.Length >= 2);
            Debug.Assert(arguments[0] is Uri);
            Debug.Assert(arguments[1] is DownloadSettings);
#if NET35
            Func<Uri, DownloadSettings, IEnumerable<Stream>> handler = this.DownloadCore;
            IAsyncResult asyncResult = handler.BeginInvoke(
                (Uri)arguments[0],
                (DownloadSettings)arguments[1],
                callback,
                state);
            return FetchAsyncResult.Create(handler, asyncResult);
#else
            return Task<IEnumerable<Stream>>.Factory.ContinueWhenAll(
                new[]
                {
                    Task.Run(()=>
                        this.DownloadAsync((Uri)arguments[0], (DownloadSettings)arguments[1]
#if NET5_0_OR_GREATER
                            , fetchAsyncState.CancellationToken
#endif
                        ).Synchronize(
#if NET5_0_OR_GREATER
                            fetchAsyncState.CancellationToken
#endif
                        )
                    )
                },
                tasks =>
                {
                    var task = tasks[0];
                    callback?.Invoke(task);
                    return task.Result;
                });
#endif
        }

        public IEnumerable<Stream> EndDownload(IAsyncResult asyncResult)
        {
            if (asyncResult is null) throw new ArgumentNullException(nameof(asyncResult));
#if NET35
            var fetchAsyncResult = (FetchAsyncResult<Func<Uri, DownloadSettings, IEnumerable<Stream>>>)asyncResult;
            return fetchAsyncResult.Handler.EndInvoke(fetchAsyncResult.AsyncResult);
#else
            Debug.Assert(asyncResult is Task<IEnumerable<Stream>>);
            var task = (Task<IEnumerable<Stream>>)asyncResult;
            task.Wait();
            return task.Result;
#endif
        }

#if NET5_0_OR_GREATER
        public IAsyncEnumerable<Stream> DownloadAsync(Uri hls, DownloadSettings settings) =>
            this.DownloadAsync(hls, settings, CancellationToken.None);
#endif

#if !NET35
        public async IAsyncEnumerable<Stream> DownloadAsync(Uri hls, DownloadSettings settings
#if NET5_0_OR_GREATER
            , CancellationToken cancellationToken
#endif
            )
        {
            if (hls is null) throw new ArgumentNullException(nameof(hls));
            if (settings is null) throw new ArgumentNullException(nameof(settings));

#if !NET5_0_OR_GREATER
            var pair = await this.DownloadM3U8Async(hls, settings);
#else
            var pair = await this.DownloadM3U8Async(hls, settings, cancellationToken);
#endif
            var m3u8Uri = pair.Key;
            var m3u8Document = pair.Value;

            foreach (var clip in m3u8Document.MediaClips)
            {
#if !NET5_0_OR_GREATER
                yield return await this.DownloadClipAsync(m3u8Uri, clip);
#else
                yield return await this.DownloadClipAsync(m3u8Uri, clip, cancellationToken);
#endif
            }
        }
#endif
        #endregion

        #region DownloadM3U8
#if NET5_0_OR_GREATER
        protected virtual KeyValuePair<Uri, M3U8Document> DownloadM3U8(Uri hls, DownloadSettings settings) =>
            this.DownloadM3U8(hls, settings, CancellationToken.None);
#endif

        protected virtual KeyValuePair<Uri, M3U8Document> DownloadM3U8(Uri hls, DownloadSettings settings
#if NET5_0_OR_GREATER
            , CancellationToken cancellationToken
#endif
            )
        {
            Debug.Assert(hls is not null);
            Debug.Assert(settings is not null);

            var m3u8 = Encoding.UTF8.GetString(
#if !NET5_0_OR_GREATER
                this.FetchData(hls!)
#else
                this.FetchData(hls, cancellationToken)
#endif
            );
            Uri uri_m3u8 = new(m3u8.Split('\n').First(line => !line.StartsWith("#")), UriKind.Absolute);
            var content_m3u8 = Encoding.UTF8.GetString(
#if !NET5_0_OR_GREATER
                this.FetchData(uri_m3u8)
#else
                this.FetchData(uri_m3u8, cancellationToken)
#endif
            );
            M3U8Document m3u8Document = new();
            m3u8Document.Load(content_m3u8);

            return new KeyValuePair<Uri, M3U8Document>(uri_m3u8, m3u8Document);
        }

#if !NET35
        protected async Task<KeyValuePair<Uri, M3U8Document>> DownloadM3U8Async(Uri hls, DownloadSettings settings) =>
            await Task.Run(() => this.DownloadM3U8(hls, settings));

#if NET5_0_OR_GREATER
        protected async Task<KeyValuePair<Uri, M3U8Document>> DownloadM3U8Async(Uri hls, DownloadSettings settings, CancellationToken cancellationToken) =>
            await Task.Run(() => this.DownloadM3U8(hls, settings, cancellationToken));
#endif
#endif
        #endregion

        #region DownloadClip
#if NET5_0_OR_GREATER
        /// <inheritdoc cref="DownloadClip(Uri, UriInsection, CancellationToken)"/>
        protected virtual Stream DownloadClip(Uri m3u8Uri, UriInsection clip) =>
            this.DownloadClip(m3u8Uri, clip, CancellationToken.None);
#endif

#if !NET5_0_OR_GREATER
        /// <summary>
        /// 下载M3U8中的音频切片。
        /// </summary>
        /// <param name="m3u8Uri">M3U8文件的地址。</param>
        /// <param name="clip">M3U8中的音频切片。</param>
        /// <returns>一个流，包含M3U8中的音频切片的全部数据。</returns>
#else
        /// <summary>
        /// 下载M3U8中的音频切片。
        /// </summary>
        /// <param name="m3u8Uri">M3U8文件的地址。</param>
        /// <param name="clip">M3U8中的音频切片。</param>
        /// <param name="cancellationToken">操作取消令牌。</param>
        /// <returns>一个流，包含M3U8中的音频切片的全部数据。</returns>
#endif
        protected virtual Stream DownloadClip(Uri m3u8Uri, UriInsection clip
#if NET5_0_OR_GREATER
            , CancellationToken cancellationToken
#endif
        )
        {
            Debug.Assert(m3u8Uri is not null);
            Debug.Assert(clip is not null);

            Uri file;
            if (clip.Uri.IsAbsoluteUri) // 音频切片中的文件地址为绝对路径。
                file = clip.Uri;
            else // 音频切片中的文件地址为相对路径。
            {
                int segmentsLength = m3u8Uri.Segments.Length;
                if (string.IsNullOrEmpty(m3u8Uri.Segments[segmentsLength - 1])) segmentsLength -= 1;
                var segments = new string[segmentsLength];
                Array.Copy(m3u8Uri.Segments, segments, segmentsLength);
                Uri host = new(
                    m3u8Uri.GetLeftPart(UriPartial.Authority) + string.Concat(segments)
                );
                file = new Uri(host, clip.Uri); // 组合获得文件地址的绝对路径。
            }

#if !NET5_0_OR_GREATER
            var content = this.FetchData(file);
#else
            var content = this.FetchData(file, cancellationToken);
#endif

            MemoryStream ms = new(content, false);
            Stream clipStream;
            try
            {
                clipStream = clip.Key.EncryptStream(ms, kUri =>
#if !NET5_0_OR_GREATER
                    this.FetchData(kUri)
#else
                    this.FetchData(kUri, cancellationToken)
#endif
                );
            }
            catch (Exception)
            {
                ms.Dispose();
                throw;
            }

            if (!object.ReferenceEquals(clipStream, ms)) // 释放已失去作用的流对象。
                ms.Dispose();

            return clipStream;
        }

#if !NET35
        protected async Task<Stream> DownloadClipAsync(Uri m3u8Uri, UriInsection clip) =>
            await Task.Run(() => this.DownloadClip(m3u8Uri, clip));

#if NET5_0_OR_GREATER
        protected async Task<Stream> DownloadClipAsync(Uri m3u8Uri, UriInsection clip, CancellationToken cancellationToken) =>
            await Task.Run(() => this.DownloadClip(m3u8Uri, clip, cancellationToken));
#endif
#endif
        #endregion
    }
}
