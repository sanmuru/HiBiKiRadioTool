// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace Qtyi.HiBiKiRadio.Json.Converters;

public partial class SizeJsonConverter
{
    [return: NotNullIfNotNull(nameof(value))]
    internal Size? ConvertFrom(image_info? value)
    {
        if (value is null) return null;
        return this.ConvertFromCore(value.width, value.height);
    }

    [return: NotNullIfNotNull(nameof(value))]
    public string? ConvertTo(Size? value) => this.ConvertToCore(value);

    protected virtual Size ConvertFromCore(int width, int height, params object[] args)
    {
        if (
#if NETFRAMEWORK && !NET40_OR_GREATER
            string.IsNullOrEmpty(value) || value.All(char.IsWhiteSpace)
#else
            string.IsNullOrWhiteSpace(value)
#endif
            ) return null;
        return new Uri(value, UriKind.RelativeOrAbsolute);
    }

    [return: NotNullIfNotNull(nameof(value))]
    protected virtual string? ConvertToCore(Size? value, params object[] args)
    {
        if (!value.HasValue) return null;
        return new image_info() { width = value.Value.Width, height = value.Value.Height }.ToString();
    }
}
