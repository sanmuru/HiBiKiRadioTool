using HiBikiRadioTool.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiBikiRadioTool.Task
{
    public class InformationListTask : ApiTaskBase
    {
        public InformationListTask() : base() { }

        public virtual InformationInfo[] Run() => this.FetchAsObject<information[]>(new Uri(ApiBase, "informations")).Select(ip => new InformationInfo(ip)).ToArray();

        public override object Run(params object[] taskParameters) => this.Run();
    }
}
