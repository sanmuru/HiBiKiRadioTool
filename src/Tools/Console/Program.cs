using SamLu.Utility.HiBiKiRadio.Info;
using SamLu.Utility.HiBiKiRadio.Tasks;

namespace HiBiKiRadioTool.Console;

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

        var task_revuestarlight = Work(revuestarlight);
        var task_siegfeld = Work(siegfeld);

        Task.WaitAll(new[] { task_revuestarlight, task_siegfeld });
    }

    private static
#if !NET35
        async
#endif
        Task Work(Task<ProgramInfo> task)
#if NET35
        => Task.Factory.StartNew(() =>
#endif
    {
        ProgramInfo program;
#if NET35
        program = task.Result;
#else
        program = await task;
#endif

        // 创建广播对应文件夹。
        DirectoryInfo radioDir = new(@"F:\少女☆歌劇 レヴュースタァライト\广播");
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
#if NET35
                    writer.Write(text);
#else
                    await writer.WriteAsync(text);
#endif
                }
            }
            System.Diagnostics.Process.Start("notepad", descriptionFile.FullName); // 打开介绍文本文件以修改。
        }

        // 下载介绍配图。
        DownloadImageTask downloadImageTask = new();
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
#if NET35
                downloadImageTask.DownloadAsync(photoUri, photoFile.FullName).Wait();
#else
                await downloadImageTask.DownloadAsync(photoUri, photoFile.FullName);
#endif
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

        List<Task> tasks = new(2);
        PlayCheckTask playCheckTask = new();
        DirectoryInfo tempRoot = episodeDir.CreateSubdirectory("temp");
        if (!mainRecordFileExists && !(program.Episode.Video == null))
        {
            tasks.Add(
                playCheckTask.CheckAsync(program.Episode.Video.ID).ContinueWith(
                    t => DownloadVideo(t.Result, new(Path.Combine(episodeDir.FullName, $"{program.Name} {program.Episode.Name}.aac"), tempRoot.FullName)).Wait(),
                    TaskContinuationOptions.OnlyOnRanToCompletion
                )
            );
        }
        if (!additionalRecordFileExists && !(program.Episode.AdditionalVideo == null))
        {
            tasks.Add(
                playCheckTask.CheckAsync(program.Episode.AdditionalVideo.ID).ContinueWith(
                    t => DownloadVideo(t.Result, new(Path.Combine(episodeDir.FullName, $"{program.Name} {program.Episode.Name} 楽屋裏.aac"), tempRoot.FullName)).Wait(),
                    TaskContinuationOptions.OnlyOnRanToCompletion
                )
            );
        }
        Task.WaitAll(tasks.ToArray());
        tempRoot.Delete(true);

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
#if NET35
    );
#endif

    private static
#if !NET35
        async
#endif
        Task DownloadVideo(PlaylistInfo video, PlaylistTask.DownloadSettings settings)
#if NET35
        => Task.Factory.StartNew(() =>
#endif
    {
        PlaylistTask playlistTask = new();
        DirectoryInfo outDir = new(settings.OutputPath);

        var hls = video.PlaylistUri;

        DirectoryInfo tempRoot = settings.TempPath is null ? new FileInfo(settings.OutputPath).Directory!.CreateSubdirectory("temp") : new DirectoryInfo(settings.TempPath);
        if (!tempRoot.Exists) tempRoot.Create();
        DirectoryInfo temp = tempRoot.CreateSubdirectory(Path.GetRandomFileName());
        tempRoot.Attributes |= FileAttributes.Hidden;

        FileInfo playlist = new(Path.Combine(temp.FullName, "playlist"));
        FileInfo outputMp4 = new(Path.Combine(temp.FullName, "output.mp4"));
        FileInfo outputAac = new(Path.Combine(temp.FullName, "output.aac"));
        using (StreamWriter writer = playlist.CreateText())
        {
            int index = 0;
            foreach (var clipStream in
#if NET35
                playlistTask.DownloadAsync(hls, settings).Result
#else
                await playlistTask.DownloadAsync(hls, settings)
#endif
            )
            {
                index++;
                FileInfo clipFile = new(Path.Combine(temp.FullName, $"{index:D3}.ts"));
                using (var fs = clipFile.OpenWrite())
                {
                    System.Console.WriteLine("正在下载\"{0}\"的第{1}部分。", outDir.Name, index);

                    const int bufferSize = 81920;
#if NET35
                    clipStream.CopyTo(fs, bufferSize);
#else
                    await clipStream.CopyToAsync(fs);
#endif
                }

                writer.WriteLine("file {0:D3}.ts", index);
            }
        }


        var process = new System.Diagnostics.Process();
        process.StartInfo.FileName = "ffmpeg";
        process.StartInfo.Arguments = string.Join(" ", new[]
        {
            "-f", "concat",
            "-i", playlist.FullName,
            "-bsf:a", "aac_adtstoasc",
            "-c", "copy",
            outputMp4.FullName
        }.Select(span => span.Contains(' ') ? '"' + span + '"' : span).ToArray());
        process.Start();
        process.WaitForExit();

        process.StartInfo.Arguments = string.Join(" ", new[]
        {
            "-i", outputMp4.FullName,
            "-vn",
            "-acodec", "copy",
            outputAac.FullName
        }.Select(span => span.Contains(' ') ? '"' + span + '"' : span).ToArray());
        process.Start();
        process.WaitForExit();

        outputAac.MoveTo(settings.OutputPath);
    }
#if NET35
    );
#endif
}
