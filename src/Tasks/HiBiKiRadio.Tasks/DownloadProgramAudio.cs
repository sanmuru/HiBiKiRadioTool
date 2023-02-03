using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using SamLu.Utility.HiBiKiRadio.Tasks;

namespace SamLu.Utility.HiBiKiRadio.Build.Tasks;

public class DownloadProgramAudio : Task
{
    [Required]
    public ITaskItem ProgramId { get; set; }

    [Output]
    public ITaskItem ProgramName { get; protected set; }

    [Output]
    public ITaskItem EpisodeName { get; protected set; }

    public override bool Execute()
    {
        ProgramDetailTask programDetailTask = new();
        var program = programDetailTask.FetchAsync(this.ProgramId.ItemSpec).Result;

        this.ProgramName = new TaskItem(program.Name);
        this.EpisodeName = new TaskItem(program.Episode.Name);


        throw new System.NotImplementedException();
    }
}