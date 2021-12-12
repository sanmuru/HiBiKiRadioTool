using SamLu.Utility.HiBiKiRadio.Info;
using SamLu.Utility.HiBiKiRadio.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SamLu.Utility.HiBiKiRadio.Task
{
    public class PlayCheckTask : ApiTaskBase
    {
        public virtual async Task<PlaylistInfo> Run(int id) =>
            new PlaylistInfo(await this.FetchAs<playlist>(new Uri(ApiBase, $"videos/play_check?video_id={id}")));

        public sealed override async Task<object?> Run(params object[] taskParameters)
        {
            ArgumentNullException.ThrowIfNull(taskParameters);
            if (taskParameters.Length < 1 || taskParameters[0] is int) throw new ArgumentException("第一个参数应为正整数。", nameof(taskParameters));

            return await this.Run((int)taskParameters[0]);
        }
    }
}
