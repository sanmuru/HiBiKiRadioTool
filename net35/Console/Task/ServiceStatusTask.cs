using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiBikiRadioTool.Task
{
    public class ServiceStatusTask : ApiTaskBase
    {
        public ServiceStatusTask() : base() { }

        public override object Run(params object[] taskParameters) => this.FetchAsJson(ServiceStatusAPI);
    }
}
