using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Utility.HiBiKiRadio.Info
{
    public abstract class JsonObjectInfo<T>
    {
        protected readonly T jObject;

        protected JsonObjectInfo(T jObject) => this.jObject = jObject ?? throw new ArgumentNullException(nameof(jObject));
    }
}
