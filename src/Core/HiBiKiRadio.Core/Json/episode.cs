// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Qtyi.HiBiKiRadio.Json;

#pragma warning disable CS1591, CS8618
internal class episode
{
    public int id { get; set; }
    public string name { get; set; }
    public int? media_type { get; set; }
    public video? video { get; set; }
    public video? additional_video { get; set; }
    public string? html_description { get; set; }
    public string? link_url { get; set; }
    public string? updated_at { get; set; }
    public episode_part[] episode_parts { get; set; }
    public chapter[]? chapters { get; set; }
}
