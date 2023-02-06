// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Qtyi.HiBiKiRadio.Json;

#pragma warning disable CS1591, CS8618
public class segment
{
    public int id{ get; set; }
    public string? name{ get; set; }
    public segment_part[] segment_parts{ get; set; }
    public string html_description{ get; set; }
    public string? publish_start_at{ get; set; }
    public string? publish_end_at{ get; set; }
    public string? updated_at{ get; set; }
}
