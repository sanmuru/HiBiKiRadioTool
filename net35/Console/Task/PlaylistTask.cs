using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using HiBikiRadioTool.M3U8;

namespace HiBikiRadioTool.Task
{
    public class PlaylistTask : ApiTaskBase
    {
        internal virtual void Download(Uri hls, string path)
        {
            CookieContainer cookieContainer = new CookieContainer();
            var m3u8 = Encoding.UTF8.GetString(
                this.FetchData(hls,
                    request =>
                    {
                        request.Accept = "*/*";
                        request.CookieContainer = cookieContainer;
                    }
                )
            );
            var cookies = cookieContainer.GetCookies(hls);
            var logica = cookies["_logica-vms_session"];

            Uri uri_MediaM3U8 = new Uri(m3u8.Split('\n').First(line => !line.StartsWith("#")), UriKind.Absolute);
            var content_MediaM3U8 = Encoding.UTF8.GetString(this.FetchData(uri_MediaM3U8));
            var mediaM3U8 = new HiBikiRadioTool.M3U8.M3U8();
            mediaM3U8.Load(content_MediaM3U8);
            var clips = mediaM3U8.MediaClips;

            DirectoryInfo tempRoot = new FileInfo(path).Directory.CreateSubdirectory("temp");
            DirectoryInfo temp = tempRoot .CreateSubdirectory(Path.GetRandomFileName());
            tempRoot.Attributes |= FileAttributes.Hidden;

            foreach (var clip in clips)
            {
                var file = clip.Uri.IsAbsoluteUri ? clip.Uri : new Uri(new Uri(uri_MediaM3U8.GetLeftPart(UriPartial.Authority) + string.Concat(uri_MediaM3U8.Segments.Take(uri_MediaM3U8.Segments.Length - 1).ToArray())), clip.Uri);
                var content = this.FetchData(file,
                    request =>
                    {
                        request.Accept = "*/*";
                    }
                );

                FileInfo tsClip = new FileInfo(Path.Combine(temp.FullName, file.Segments.Last()));
                using (FileStream fs = tsClip.Create())
                using (MemoryStream ms = new MemoryStream(content, false))
                using (var cs = clip.Key.EncryptStream(ms, kUri =>
                {
                    byte[] kData = this.FetchData(kUri,
                        request =>
                        {
                            request.Accept = "*/*";
                            request.CookieContainer = new CookieContainer();
                            request.CookieContainer.Add(logica);
                        });
                    return kData;
                }))
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

            outputAac.MoveTo(path);
            Environment.CurrentDirectory = tempRoot.Parent.FullName; // 切换到上层目录防止占用目录，导致无法删除目录。
            tempRoot.Delete(true);
        }

        public override object Run(params object[] taskParameters)
        {
            if (taskParameters is null) throw new ArgumentNullException(nameof(taskParameters));

            this.Download(taskParameters[0] as Uri, taskParameters[1] as string);
            return null;
        }
    }
}
