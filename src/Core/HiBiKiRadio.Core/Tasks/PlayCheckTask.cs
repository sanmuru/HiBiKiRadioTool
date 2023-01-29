using SamLu.Utility.HiBiKiRadio.Info;
using SamLu.Utility.HiBiKiRadio.Json;

namespace SamLu.Utility.HiBiKiRadio.Tasks;

public class PlayCheckTask : ApiTaskBase
{
    public PlayCheckTask() : base() { }

    public PlaylistInfo CheckCore(int id, CancellationToken cancellationToken) =>
        new(this.FetchAsAsync<playlist>(new Uri(ApiBase, $"videos/play_check?video_id={id}"), cancellationToken).Result);

    public Task<PlaylistInfo> CheckAsync(int id, CancellationToken cancellationToken = default) =>
        Task.Factory.StartNew(() => this.CheckCore(id, cancellationToken), cancellationToken);
}
