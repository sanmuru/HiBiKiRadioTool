// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Qtyi.HiBiKiRadio.Json;

#pragma warning disable CS1591, CS8618
internal class information
{
    public int id { get; set; }
    public string? day { get; set; }
    public string? name { get; set; }
    public int kind { get; set; }
    public string? kind_name { get; set; }
    public int priority { get; set; }
    public string? link_url { get; set; }
    public string? pc_image_url { get; set; }
    public image_info? pc_image_info { get; set; }
    public string? sp_image_url { get; set; }
    public image_info? sp_image_info { get; set; }
    public information_part[]? information_parts { get; set; }
    public string? html_description { get; set; }
    public string? publish_start_at { get; set; }
    public string? publish_end_at { get; set; }
    public string? updated_at { get; set; }
}
