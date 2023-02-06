// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Qtyi.HiBiKiRadio.Info;
using Qtyi.HiBiKiRadio.Json;
using System.Diagnostics;

namespace Qtyi.HiBiKiRadio.Tasks;

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
