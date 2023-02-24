// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Qtyi.HiBiKiRadio.Info;
using Qtyi.HiBiKiRadio.Json;

namespace Qtyi.HiBiKiRadio.Tasks;

public class ProgramListTask : ApiTaskBase
{
    public ProgramListTask() : base() { }

    protected virtual ProgramInfo[] FetchCore(CancellationToken cancellationToken)
    {
        var programs = this.FetchAsAsync<program[]>(new Uri(ApiBase, "programs"), cancellationToken).Result;
        var length = programs.Length;
        var infos = new ProgramInfo[length];
        for (var i = 0; i < length; i++)
            infos[i] = new(programs[i]);

        return infos;
    }

    public Task<ProgramInfo[]> FetchAsync(CancellationToken cancellationToken = default) =>
        Task.Factory.StartNew(() => this.FetchCore(cancellationToken));
}
