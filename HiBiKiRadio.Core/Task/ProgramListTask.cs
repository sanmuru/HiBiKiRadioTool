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
    public class ProgramListTask : ApiTaskBase
    {
        public ProgramListTask() : base() { }

        public virtual async Task<ProgramInfo[]> Run() =>
            (await this.FetchAs<program[]>(new Uri(ApiBase, "programs")))
                .Select(p => new ProgramInfo(p)).ToArray();

        public sealed override async Task<object?> Run(params object[] taskParameters) => await this.Run();
    }
}
