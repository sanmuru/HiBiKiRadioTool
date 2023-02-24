// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Qtyi.HiBiKiRadio.Json.Converters;

public partial class DateTimeJsonConverter
{
    private const string DateTimeFormat = "yyyy/MM/dd HH:mm:ss";

    public DateTime? ConvertFrom(string? value) => this.ConvertFromCore(value);

    public string? ConvertTo(DateTime? value) => this.ConvertToCore(value);

    protected virtual DateTime? ConvertFromCore(string? value, params object[] args)
    {
        if (
#if NETFRAMEWORK && !NET40_OR_GREATER
            string.IsNullOrEmpty(value) || value.All(char.IsWhiteSpace)
#else
            string.IsNullOrWhiteSpace(value)
#endif
            ) return null;
        return DateTime.ParseExact(value, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None);
    }

    [return: NotNullIfNotNull(nameof(value))]
    protected virtual string? ConvertToCore(DateTime? value, params object[] args)
    {
        if (!value.HasValue) return null;
        return value.Value.ToString(DateTimeFormat);
    }

    public DateTimeJsonConverter WithTimezone(double utc, bool adjustConvertFrom = true, bool adjustConvertTo = true) => new DateTimeWithTimezoneJsonConverter(utc, adjustConvertFrom: adjustConvertFrom, adjustConvertTo: adjustConvertTo);
}

partial class DateTimeJsonConverter
{
    internal sealed class DateTimeWithTimezoneJsonConverter : DateTimeJsonConverter
    {
        private readonly double _utc;
        private readonly bool _adjustConvertFrom;
        private readonly bool _adjustConvertTo;

        public DateTimeWithTimezoneJsonConverter(double utc, bool adjustConvertFrom, bool adjustConvertTo)
        {
            this._utc = utc;
            this._adjustConvertFrom = adjustConvertFrom;
            this._adjustConvertTo = adjustConvertTo;
        }

        protected override DateTime? ConvertFromCore(string? value, params object[] args)
        {
            var result = base.ConvertFromCore(value, args);
            if (this._adjustConvertFrom && result.HasValue)
                result = result.Value.AddHours(-this._utc);
            return result;
        }

        [return: NotNullIfNotNull("value")]
        protected override string? ConvertToCore(DateTime? value, params object[] args)
        {
            if (this._adjustConvertTo && value.HasValue)
                value = value.Value.AddHours(this._utc);
            return base.ConvertToCore(value, args);
        }
    }
}
