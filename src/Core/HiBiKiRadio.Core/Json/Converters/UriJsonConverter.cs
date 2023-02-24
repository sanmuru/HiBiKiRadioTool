// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace Qtyi.HiBiKiRadio.Json.Converters;

public partial class UriJsonConverter
{
    public Uri? ConvertFrom(string? value) => this.ConvertFromCore(value);

    public string? ConvertTo(Uri? value) => this.ConvertToCore(value);

    protected virtual Uri? ConvertFromCore(string? value, params object[] args)
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
    protected virtual string? ConvertToCore(Uri? value, params object[] args) => value?.AbsoluteUri;
}
