using System;

namespace SamLu.Utility.HiBiKiRadio.Info
{
    public abstract class JsonObjectInfo<T> where T : class
    {
        protected readonly T jObject;

        protected JsonObjectInfo(T jObject) => this.jObject = jObject ?? throw new ArgumentNullException(nameof(jObject));
    }
}
