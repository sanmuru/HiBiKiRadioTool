using SamLu.Utility.HiBiKiRadio.Info;
using SamLu.Utility.HiBiKiRadio.Json;
using SamLu.Utility.HiBiKiRadio.M3U8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SamLu.Utility.HiBiKiRadio.Task
{
    public class PlaylistTask : ApiTaskBase
    {
        public sealed class DownloadSettings
        {
            public string OutputPath { get; init; }
            public string? TempPath { get; set; }
            
            /// <summary>
            /// 使用指定的输出目录初始化 <see cref="DownloadSettings"/> 的实例。
            /// </summary>
            /// <param name="path">下载的文件的输出目录。</param>
            public DownloadSettings(string path) => this.OutputPath = path;
        }

        public virtual async System.Threading.Tasks.Task Download(Uri hls, DownloadSettings settings)
        {
            var m3u8 = Encoding.UTF8.GetString(await this.FetchData(hls));
            Uri uri_MediaM3U8 = new Uri(m3u8.Split('\n').First(line => !line.StartsWith("#")), UriKind.Absolute);
            var content_MediaM3U8 = Encoding.UTF8.GetString(await this.FetchData(uri_MediaM3U8));
            var mediaM3U8 = new M3U8Document();
            mediaM3U8.Load(content_MediaM3U8);
            var clips = mediaM3U8.MediaClips;

            DirectoryInfo tempRoot = settings.TempPath is null ? new FileInfo(settings.OutputPath).Directory!.CreateSubdirectory("temp") : new DirectoryInfo(settings.TempPath);
            DirectoryInfo temp = tempRoot.CreateSubdirectory(Path.GetRandomFileName());
            tempRoot.Attributes |= FileAttributes.Hidden;

            //List<System.Threading.Tasks.Task> tasks = new();
            foreach (var clip in clips)
            {
                var task = this.DownloadClip(uri_MediaM3U8, temp, clip);
                task.Wait(); // HiBiKi网站不允许同时多个请求。
                //tasks.Add(task);
            }

            Environment.CurrentDirectory = temp.FullName;
            FileInfo playlist = new FileInfo(Path.Combine(temp.FullName, "playlist"));
            FileInfo outputMp4 = new FileInfo(Path.Combine(temp.FullName, "output.mp4"));
            FileInfo outputAac = new FileInfo(Path.Combine(temp.FullName, "output.aac"));
            using (StreamWriter writer = playlist.CreateText())
            {
                foreach (var clip in clips)
                {
                    var file = clip.Uri.IsAbsoluteUri ? clip.Uri : new Uri(new Uri(uri_MediaM3U8.GetLeftPart(UriPartial.Authority) + string.Concat(uri_MediaM3U8.Segments.Take(uri_MediaM3U8.Segments.Length - 1).ToArray())), clip.Uri);
                    writer.WriteLine("file {0}", file.Segments.Last());
                }
            }

            //System.Threading.Tasks.Task.WaitAll(tasks.ToArray());

            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "ffmpeg";
            process.StartInfo.Arguments = string.Join(" ", new[]
            {
                "-f", "concat",
                "-i", playlist.Name,
                "-bsf:a", "aac_adtstoasc",
                "-c", "copy",
                outputMp4.Name
            }.Select(span => span.Contains(" ")? '"' + span + '"' : span).ToArray());
            process.Start();
            process.WaitForExit();

            process.StartInfo.Arguments = string.Join(" ", new[]
            {
                "-i", outputMp4.Name,
                "-vn",
                "-acodec", "copy",
                outputAac.Name
            }.Select(span => span.Contains(" ") ? '"' + span + '"' : span).ToArray());
            process.Start();
            process.WaitForExit();

            outputAac.MoveTo(settings.OutputPath);
            Environment.CurrentDirectory = tempRoot.Parent!.FullName; // 切换到上层目录防止占用目录，导致无法删除目录。
            tempRoot.Delete(true);
        }

        private System.Threading.Tasks.Task DownloadClip(Uri uri, DirectoryInfo temp, UriInsection clip)
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                var file = clip.Uri.IsAbsoluteUri ? clip.Uri : new Uri(new Uri(uri.GetLeftPart(UriPartial.Authority) + string.Concat(uri.Segments.Take(uri.Segments.Length - 1).ToArray())), clip.Uri);
                var content = this.FetchData(file).Result;

                FileInfo tsClip = new FileInfo(Path.Combine(temp.FullName, file.Segments.Last()));
                using (FileStream fs = tsClip.Create())
                using (MemoryStream ms = new MemoryStream(content, false))
                using (var cs = clip.Key.EncryptStream(ms, kUri => this.FetchData(kUri).Result))
                {
                    byte[] buffer = new byte[byte.MaxValue];
                    int count;
                    do
                    {
                        count = cs.Read(buffer, 0, buffer.Length);
                        fs.Write(buffer, 0, count);
                    }
                    while (count > 0);
                }
            });
        }

        public sealed override async Task<object?> Run(params object[] taskParameters)
        {
            ArgumentNullException.ThrowIfNull(taskParameters);

            await this.Download((Uri)taskParameters[0], (DownloadSettings)taskParameters[1]);
            return null;
        }
    }
}
