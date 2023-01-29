using SamLu.Utility.HiBiKiRadio.Info;
using SamLu.Utility.HiBiKiRadio.Json;
using System.Diagnostics;

namespace SamLu.Utility.HiBiKiRadio.Tasks;

public class ProgramDetailTask : ApiTaskBase
{
    public ProgramDetailTask() : base() { }

    protected virtual ProgramInfo FetchCore(string id, CancellationToken cancellationToken)
    {
        Debug.Assert(id is not null);

        return new(this.FetchAsAsync<program>(new Uri(ApiBase, $"programs/{id}"), cancellationToken).Result);
    }

    public Task<ProgramInfo> FetchAsync(string id, CancellationToken cancellationToken = default) =>
        Task.Factory.StartNew(() => this.FetchCore(id, cancellationToken));
}
