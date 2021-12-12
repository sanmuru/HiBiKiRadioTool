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
    public class ProgramDetailTask : ApiTaskBase
    {
        public ProgramDetailTask() : base() { }

        public virtual async Task<ProgramInfo> Run(string id) => new ProgramInfo(await this.FetchAs<program>(new Uri(ApiBase, $"programs/{id}")));

        public sealed override async Task<object?> Run(params object[] taskParameters)
        {
            ArgumentNullException.ThrowIfNull(taskParameters);
            if (taskParameters.Length < 1 || taskParameters[0] is string) throw new ArgumentException("第一个参数应为字符串。", nameof(taskParameters));

            return await this.Run((string)taskParameters[0]);
        }
    }
}
