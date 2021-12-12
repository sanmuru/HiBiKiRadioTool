using HiBikiRadioTool.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiBikiRadioTool.Task
{
    public class PlayCheckTask : ApiTaskBase
    {
        public virtual PlaylistInfo Run(int id) => new PlaylistInfo(this.FetchAsObject<playlist>(new Uri(ApiBase, $"videos/play_check?video_id={id}")));

        public override object Run(params object[] taskParameters)
        {
            if (taskParameters is null) throw new ArgumentNullException(nameof(taskParameters));
            if (taskParameters.Length < 1 || taskParameters[0] is int) throw new ArgumentException("第一个参数应为正整数。", nameof(taskParameters));

            return this.Run((int)taskParameters[0]);
        }
    }
}
