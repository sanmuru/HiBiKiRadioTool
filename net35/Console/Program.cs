using HiBikiRadioTool;
using HiBikiRadioTool.Task;
using SamLu.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

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

            ProgramDetailTask programDetail = new ProgramDetailTask();
            var revuestarlight = programDetail.Run(access_id_revuestarlight);
            var siegfeld = programDetail.Run(access_id_siegfeld);

            Work(revuestarlight);
            Work(siegfeld);
        }

        private static void Work(ProgramInfo program)
        {
            // 创建广播对应文件夹。
            DirectoryInfo radioDir = new DirectoryInfo(@"F:\少女☆歌劇 レヴュースタァライト\广播");
            DirectoryInfo programDir = radioDir.CreateSubdirectory(program.Name);
            DirectoryInfo episodeDir = programDir.CreateSubdirectory(Regex.Match(program.Episode.Name, @"第(?<num>\d+)回|(?<num>.+)").Groups["num"].Value);
            episodeDir.Create();

            // 创建介绍文本文件。
            FileInfo descriptionFile = new FileInfo(Path.Combine(episodeDir.FullName, "description.txt"));
            if (!descriptionFile.Exists)
            {
                using (FileStream fs = descriptionFile.Create())
                using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
                {
                    foreach (var text in program.Episode.EpisodeParts.Select(ep => ep.Description))
                    {
                        writer.Write(text);
                    }
                }
                System.Diagnostics.Process.Start(descriptionFile.FullName); // 打开介绍文本文件以修改。
            }

            // 下载介绍配图。
            var photoUris = program.Episode.EpisodeParts.Where(ep => !(ep.PCImageUri is null)).Select(ep => ep.PCImageUri).ToList();
            DirectoryInfo photoDir = episodeDir;
            int count = photoUris.Count;
            if (count > 1)
                photoDir = episodeDir.CreateSubdirectory("photos");
            for (int index = 0; index < count; index++)
            {
                var photoUri = photoUris[index];
                FileInfo photoFile = new FileInfo(
                    Path.Combine(photoDir.FullName, (count == 1 ? "photo" : (index + 1).ToString($"D{count.ToString().Length}")))
                        + Path.GetExtension(photoUri.AbsoluteUri)
                );

                if (!photoFile.Exists)
                {
                    WebRequest imgRequest = WebRequest.Create(photoUri);
                    HttpWebResponse res;
                    try
                    {
                        res = (HttpWebResponse)imgRequest.GetResponse();
                    }
                    catch (WebException ex)
                    {
                        res = (HttpWebResponse)ex.Response;
                    }

                    if (res.StatusCode.ToString() == "OK")
                    {
                        System.Drawing.Image downImage = System.Drawing.Image.FromStream(imgRequest.GetResponse().GetResponseStream());

                        downImage.Save(photoFile.FullName);
                        downImage.Dispose();
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
                string newPath = Path.Combine(recordFile.Directory.FullName, $"{program.Name} {program.Episode.Name}{(isAdditionalRecordFile ? " 楽屋裏" : string.Empty)}{recordFile.Extension}");
                if (!File.Exists(newPath))
                {
                    recordFile.MoveTo(newPath);
                    if (isAdditionalRecordFile) additionalRecordFileExists = true; else mainRecordFileExists = true;
                }
            }
            if (!mainRecordFileExists && !(program.Episode.Video is null))
            {
                PlayCheckTask playCheckTask = new PlayCheckTask();
                var video = playCheckTask.Run(program.Episode.Video.ID);
                //try
                //{
                    DownloadVideo(video, Path.Combine(episodeDir.FullName, $"{program.Name} {program.Episode.Name}.aac"));
                //}
                //catch (Exception) { }
            }
            if (!additionalRecordFileExists && !(program.Episode.AdditionalVideo is null))
            {
                PlayCheckTask playCheckTask = new PlayCheckTask();
                var video = playCheckTask.Run(program.Episode.AdditionalVideo.ID);
                //try
                //{
                    DownloadVideo(video, Path.Combine(episodeDir.FullName, $"{program.Name} {program.Episode.Name} 楽屋裏.aac"));
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
                string newPath = Path.Combine(episodeDir.FullName, $"【生肉】[{program.Episode.UpdatedTime.Value:yyyy.MM.dd}] {program.Name} {program.Episode.Name}{(Regex.IsMatch(Path.GetFileNameWithoutExtension(videoFile.FullName).Replace("_bilibili", string.Empty), @"\bgky\b", RegexOptions.IgnoreCase) || videoFile.Length < 50 * 1024 * 1024/* 50MB */ ? " 楽屋裏" : string.Empty)}.mp4");
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

        private static void DownloadVideo(PlaylistInfo video, string path)
        {
            PlaylistTask playlistTask = new PlaylistTask();
            playlistTask.Download(video.PlaylistUri, path);
        }
    }
}
