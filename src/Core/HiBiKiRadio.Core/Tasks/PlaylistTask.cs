using SamLu.Utility.HiBiKiRadio.M3U8;
using System.Diagnostics;
using System.Text;

namespace SamLu.Utility.HiBiKiRadio.Tasks;

public class PlaylistTask : ApiTaskBase
{
    public sealed class DownloadSettings
    {
        public string OutputPath { get; set; }
        public string? TempPath { get; set; }


        /// <inheritdoc cref="DownloadSettings(string, string?)"/>
        /// <summary>
        /// 使用指定的输出目录初始化 <see cref="DownloadSettings"/> 的实例。
        /// </summary>
        public DownloadSettings(string outPath) : this(outPath, null) { }

        /// <summary>
        /// 使用指定的输出目录和临时文件目录初始化 <see cref="DownloadSettings"/> 的实例。
        /// </summary>
        /// <param name="outPath">下载的文件的输出目录。</param>
        /// <param name="tempPath">存放下载操作的临时文件目录。</param>
        public DownloadSettings(string outPath, string? tempPath)
        {
            this.OutputPath = outPath ?? throw new ArgumentNullException(nameof(outPath));
            this.TempPath = tempPath;
        }
    }

    private readonly IDownloadClient _client =
#if NET35
		new WebClient();
#else
        new HttpClient();
#endif
    public override IDownloadClient Client => this._client;

    private bool disposedValue;
    protected override void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                this._client.Dispose();
            }

            disposedValue = true;
        }
    }

    public PlaylistTask() : base() { }

    protected virtual IEnumerable<Stream> DownloadCore(Uri hls, DownloadSettings settings, CancellationToken cancellationToken)
    {
        Debug.Assert(hls is not null);
        Debug.Assert(settings is not null);

        this.DownloadM3U8(hls, settings, cancellationToken, out var m3u8Uri, out var m3u8Document);

        foreach (var clip in m3u8Document.MediaClips)
        {
            this.DownloadClip(m3u8Uri, clip, cancellationToken, out var clipStream);
            yield return clipStream;
        }
    }

    public Task<IEnumerable<Stream>> DownloadAsync(Uri hls, DownloadSettings settings, CancellationToken cancellationToken = default) =>
        Task.Factory.StartNew(() => this.DownloadCore(hls, settings, cancellationToken), cancellationToken);

    protected virtual void DownloadM3U8(Uri hls, DownloadSettings settings, CancellationToken cancellationToken,
        out Uri m3u8Uri, out M3U8Document m3u8Document)
    {
        Debug.Assert(hls is not null);
        Debug.Assert(settings is not null);

        var m3u8 = Encoding.UTF8.GetString(this.FetchDataAsync(hls, cancellationToken).Result);
        m3u8Uri = new(m3u8.Split('\n').First(line => !line.StartsWith("#")), UriKind.Absolute);
        var m3u8Content = Encoding.UTF8.GetString(this.FetchDataAsync(m3u8Uri, cancellationToken).Result);
        m3u8Document = new();
        m3u8Document.Load(m3u8Content);
    }

    /// <summary>
    /// 下载M3U8中的音频切片。
    /// </summary>
    /// <param name="m3u8Uri">M3U8文件的地址。</param>
    /// <param name="clip">M3U8中的音频切片。</param>
    /// <param name="cancellationToken">操作取消令牌。</param>
    /// <param name="clipStream">一个流，包含M3U8中的音频切片的全部数据。</param>
    protected virtual void DownloadClip(Uri m3u8Uri, UriInsection clip, CancellationToken cancellationToken, out Stream clipStream)
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
                m3u8Uri.Scheme + m3u8Uri.Authority + string.Concat(segments)
            );
            file = new Uri(host, clip.Uri); // 组合获得文件地址的绝对路径。
        }

        var content = this.FetchDataAsync(file, cancellationToken).Result;

        MemoryStream ms = new(content, false);
        try
        {
            if (clip.Key is null)
                clipStream = ms;
            else
                clipStream = clip.Key.EncryptStream(ms, kUri => this.FetchDataAsync(kUri, cancellationToken).Result, cancellationToken);
        }
        catch (Exception)
        {
            ms.Dispose();
            throw;
        }

        if (!object.ReferenceEquals(clipStream, ms)) // 释放已失去作用的流对象。
            ms.Dispose();
    }
}
