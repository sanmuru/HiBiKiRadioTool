// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Qtyi.HiBiKiRadio.Info;
using Qtyi.HiBiKiRadio.Json;

namespace Qtyi.HiBiKiRadio.Tasks;

public class PlayCheckTask : ApiTaskBase
{
    public PlayCheckTask() : base() { }

    public PlaylistInfo CheckCore(int id, CancellationToken cancellationToken) =>
        new(this.FetchAsAsync<playlist>(new Uri(ApiBase, $"videos/play_check?video_id={id}"), cancellationToken).Result);

    public Task<PlaylistInfo> CheckAsync(int id, CancellationToken cancellationToken = default) =>
        Task.Factory.StartNew(() => this.CheckCore(id, cancellationToken), cancellationToken);
}
