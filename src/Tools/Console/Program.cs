using SamLu.Utility.HiBiKiRadio.Info;
using SamLu.Utility.HiBiKiRadio.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HiBiKiRadioTool.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //ProgramListTask programList= new ProgramListTask();
            //var programs = programList.Run();

            const int id_revuestarlight = 199;
            const string access_id_revuestarlight = "revuestarlight";
            const int id_siegfeld = 230;
            const string access_id_siegfeld = "siegfeld";

            //string serviceStatus = new ServiceStatusTask().Run() as string;
            //string informations = new InformationListTask().Run() as string;

            //InformationListTask informationListTask = new InformationListTask();
            //var informations = informationListTask.Run();

            ProgramDetailTask programDetail = new();
            var revuestarlight = programDetail.FetchAsync(access_id_revuestarlight);
            var siegfeld = programDetail.FetchAsync(access_id_siegfeld);

            Work(revuestarlight).Wait();
            Work(siegfeld).Wait();

            //Task.WaitAll(new[] { task_revuestarlight, task_siegfeld });
        }

        private static async Task Work(Task<ProgramInfo> task)
        {
            var program = await task;

            // 创建广播对应文件夹。
            DirectoryInfo radioDir = new DirectoryInfo(@"F:\少女☆歌劇 レヴュースタァライト\广播");
            DirectoryInfo programDir = radioDir.CreateSubdirectory(program.Name);
            DirectoryInfo episodeDir = programDir.CreateSubdirectory(Regex.Match(program.Episode.Name, @"第(?<num>\d+)回|(?<num>.+)").Groups["num"].Value);
            episodeDir.Create();

            // 创建介绍文本文件。
            FileInfo descriptionFile = new(Path.Combine(episodeDir.FullName, "description.txt"));
            if (!descriptionFile.Exists)
            {
                using (FileStream fs = descriptionFile.Create())
                using (StreamWriter writer = new(fs, Encoding.UTF8))
                {
                    foreach (var text in program.Episode.EpisodeParts.Select(ep => ep.Description.Trim()))
                    {
                        await writer.WriteAsync(text);
                    }
                }
                System.Diagnostics.Process.Start("notepad", descriptionFile.FullName); // 打开介绍文本文件以修改。
            }

            // 下载介绍配图。
            DownloadImageTask downloadImageTask = new DownloadImageTask();
            var photoUris = program.Episode.EpisodeParts.Where(ep => ep.PCImageUri is not null).Select(ep => ep.PCImageUri!).ToList();
            DirectoryInfo photoDir = episodeDir;
            int count = photoUris.Count;
            if (count > 1)
                photoDir = episodeDir.CreateSubdirectory("photos");
            for (int index = 0; index < count; index++)
            {
                var photoUri = photoUris[index];
                FileInfo photoFile = new(
                    Path.Combine(photoDir.FullName, (count == 1 ? "photo" : (index + 1).ToString($"D{count.ToString().Length}")))
                        + Path.GetExtension(photoUri.AbsoluteUri)
                );

                if (!photoFile.Exists)
                {
                    byte[] imageContent = await downloadImageTask.DownloadAsync(photoUri);
                    using (FileStream fs = photoFile.Create())
                    {
                        await fs.WriteAsync(imageContent);
                    }
                }
            }

            string[] avaliableExtensions = { ".wav", ".flac", ".mp3", ".aac", ".wma" };
            // 重命名录音文件。
            var recordFiles =
                from fi in episodeDir.GetFiles("*.*", SearchOption.TopDirectoryOnly)
                let fileName = Path.GetFileNameWithoutExtension(fi.FullName)
                let fileExtension = fi.Extension
                where avaliableExtensions.Contains(fileExtension)
                where fileName == "gky" || fileName == Regex.Match(program.Episode.Name, @"第(?<num>\d+)回|(?<num>.+)").Groups["num"].Value
                select fi;
            bool mainRecordFileExists = File.Exists(Path.Combine(episodeDir.FullName, $"{program.Name} {program.Episode.Name}.aac"));
            bool additionalRecordFileExists = File.Exists(Path.Combine(episodeDir.FullName, $"{program.Name} {program.Episode.Name} 楽屋裏.aac"));
            foreach (var recordFile in recordFiles)
            {
                bool isAdditionalRecordFile = Path.GetFileNameWithoutExtension(recordFile.Name) == "gky";
                string newPath = Path.Combine(recordFile.Directory!.FullName, $"{program.Name} {program.Episode.Name}{(isAdditionalRecordFile ? " 楽屋裏" : string.Empty)}{recordFile.Extension}");
                if (!File.Exists(newPath))
                {
                    recordFile.MoveTo(newPath);
                    if (isAdditionalRecordFile) additionalRecordFileExists = true; else mainRecordFileExists = true;
                }
            }
            if (!mainRecordFileExists && !(program.Episode.Video == null))
            {
                PlayCheckTask playCheckTask = new();
                var video = await playCheckTask.CheckAsync(program.Episode.Video.ID);
                //try
                //{
                await DownloadVideo(video, Path.Combine(episodeDir.FullName, $"{program.Name} {program.Episode.Name}.aac"));
                //}
                //catch (Exception) { }
            }
            if (!additionalRecordFileExists && !(program.Episode.AdditionalVideo == null))
            {
                PlayCheckTask playCheckTask = new();
                var video = await playCheckTask.CheckAsync(program.Episode.AdditionalVideo.ID);
                //try
                //{
                await DownloadVideo(video, Path.Combine(episodeDir.FullName, $"{program.Name} {program.Episode.Name} 楽屋裏.aac"));
                //}
                //catch (Exception) { }
            }

            // 重命名视频文件。
            var videoFiles =
                from file in Directory.GetFiles(@"F:\", "*.mp4", SearchOption.TopDirectoryOnly)
                let fileName = Path.GetFileNameWithoutExtension(file)
                where Regex.IsMatch(fileName, $@"^{Regex.Match(program.Episode.Name, @"第(?<num>\d+)回|(?<num>.+)").Groups["num"].Value}_bilibili( \(\d+\))?")
                select new FileInfo(file);
            foreach (var videoFile in videoFiles)
            {
                string newPath = Path.Combine(episodeDir.FullName, $"【生肉】[{program.Episode.UpdatedTime!.Value:yyyy.MM.dd}] {program.Name} {program.Episode.Name}{(Regex.IsMatch(Path.GetFileNameWithoutExtension(videoFile.FullName).Replace("_bilibili", string.Empty), @"\bgky\b", RegexOptions.IgnoreCase) || videoFile.Length < 50 * 1024 * 1024/* 50MB */ ? " 楽屋裏" : string.Empty)}.mp4");
                if (!File.Exists(newPath))
                    try
                    {
                        videoFile.MoveTo(newPath);
                    }
                    catch (IOException e)
                    {
                        var color = System.Console.ForegroundColor;
                        System.Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.WriteLine("移动文件 {0} 到 {1} 失败：{2}", videoFile.FullName, newPath, e.Message);
                        System.Console.ForegroundColor = color;
                    }
            }
        }

        private static async Task DownloadVideo(PlaylistInfo video, string path)
        {
            PlaylistTask playlistTask = new();
            DirectoryInfo outDir = new DirectoryInfo(path);

            var hls = video.PlaylistUri;
            var settings = new PlaylistTask.DownloadSettings(path);

            DirectoryInfo tempRoot = new FileInfo(settings.OutputPath).Directory!.CreateSubdirectory("temp");
            DirectoryInfo temp = tempRoot.CreateSubdirectory(Path.GetRandomFileName());
            tempRoot.Attributes |= FileAttributes.Hidden;

            Environment.CurrentDirectory = temp.FullName;
            FileInfo playlist = new(Path.Combine(temp.FullName, "playlist"));
            FileInfo outputMp4 = new(Path.Combine(temp.FullName, "output.mp4"));
            FileInfo outputAac = new(Path.Combine(temp.FullName, "output.aac"));
            using (StreamWriter writer = playlist.CreateText())
            {
                int index = 0;
                await foreach (var clipStream in playlistTask.DownloadAsync(hls, settings))
                {
                    index++;
                    FileInfo clipFile = new(Path.Combine(temp.FullName, $"{index:D3}.ts"));
                    using (var fs = clipFile.OpenWrite())
                    {
                        System.Console.WriteLine("正在下载\"{0}\"的第{1}部分。", outDir.Name, index);
                        await clipStream.CopyToAsync(fs);
                    }

                    writer.WriteLine("file {0:D3}.ts", index);
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
            }.Select(span => span.Contains(' ') ? '"' + span + '"' : span).ToArray());
            process.Start();
            process.WaitForExit();

            process.StartInfo.Arguments = string.Join(" ", new[]
            {
                "-i", outputMp4.Name,
                "-vn",
                "-acodec", "copy",
                outputAac.Name
            }.Select(span => span.Contains(' ') ? '"' + span + '"' : span).ToArray());
            process.Start();
            process.WaitForExit();

            outputAac.MoveTo(settings.OutputPath);
            Environment.CurrentDirectory = tempRoot.Parent!.FullName; // 切换到上层目录防止占用目录，导致无法删除目录。
            tempRoot.Delete(true);
        }
    }
}
