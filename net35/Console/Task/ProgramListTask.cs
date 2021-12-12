using HiBikiRadioTool.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiBikiRadioTool.Task
{
    public class ProgramListTask : ApiTaskBase
    {
        public ProgramListTask() : base() { }

        public virtual ProgramInfo[] Run() => this.FetchAsObject<program[]>(new Uri(ApiBase, "programs")).Select(p => new ProgramInfo(p)).ToArray();

        public override object Run(params object[] taskParameters) => this.Run();
    }
}
