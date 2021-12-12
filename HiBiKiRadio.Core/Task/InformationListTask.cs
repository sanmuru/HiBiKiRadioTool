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
    public class InformationListTask : ApiTaskBase
    {
        public InformationListTask() : base() { }

        public virtual async Task<InformationInfo[]> Run() =>
            (await this.FetchAs<information[]>(new Uri(ApiBase, "informations")))
                .Select(ip => new InformationInfo(ip)).ToArray();

        public sealed override async Task<object?> Run(params object[] taskParameters) => await this.Run();
    }
}
