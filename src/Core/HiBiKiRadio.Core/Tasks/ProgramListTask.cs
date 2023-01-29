using SamLu.Utility.HiBiKiRadio.Info;
using SamLu.Utility.HiBiKiRadio.Json;

namespace SamLu.Utility.HiBiKiRadio.Tasks;

public class ProgramListTask : ApiTaskBase
{
    public ProgramListTask() : base() { }

    protected virtual ProgramInfo[] FetchCore(CancellationToken cancellationToken)
    {
        var programs = this.FetchAsAsync<program[]>(new Uri(ApiBase, "programs"), cancellationToken).Result;
        var length = programs.Length;
        var infos = new ProgramInfo[length];
        for (int i = 0; i < length; i++)
            infos[i] = new(programs[i]);

        return infos;
    }

    public Task<ProgramInfo[]> FetchAsync(CancellationToken cancellationToken = default) =>
        Task.Factory.StartNew(() => this.FetchCore(cancellationToken));
}
