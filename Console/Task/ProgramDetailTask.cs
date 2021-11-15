using HiBikiRadioTool.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiBikiRadioTool.Task
{
    public class ProgramDetailTask : ApiTaskBase
    {
        public ProgramDetailTask() : base() { }

        public virtual ProgramInfo Run(string id) => new ProgramInfo(this.FetchAsObject<program>(new Uri(ApiBase, $"programs/{id ?? throw new ArgumentNullException(nameof(id))}")));

        public override object Run(params object[] taskParameters)
        {
            if (taskParameters is null) throw new ArgumentNullException(nameof(taskParameters));
            if (taskParameters.Length < 1 || taskParameters[0] is string) throw new ArgumentException("第一个参数应为字符串。", nameof(taskParameters));

            return this.Run((string)taskParameters[0]);
        }
    }
}
