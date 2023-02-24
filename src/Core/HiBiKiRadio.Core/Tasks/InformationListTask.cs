// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Qtyi.HiBiKiRadio.Info;
using Qtyi.HiBiKiRadio.Json;

namespace Qtyi.HiBiKiRadio.Tasks;

public class InformationListTask : ApiTaskBase
{
    public InformationListTask() : base() { }

    protected virtual InformationInfo[] FetchCore(CancellationToken cancellationToken)
    {
        var informations = this.FetchAsAsync<information[]>(new Uri(ApiBase, "informations"), cancellationToken).Result;
        var length = informations.Length;
        var infos = new InformationInfo[informations.Length];
        for (var i = 0; i < length; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            infos[i] = new(informations[i]);
        }

        return infos;
    }

    public Task<InformationInfo[]> FetchAsync(CancellationToken cancellationToken = default) =>
        Task.Factory.StartNew(() => this.FetchCore(cancellationToken), cancellationToken);
}
