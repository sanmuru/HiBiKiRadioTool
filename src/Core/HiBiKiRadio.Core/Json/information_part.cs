﻿// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Qtyi.HiBiKiRadio.Json;

#pragma warning disable CS1591, CS8618
internal class information_part
{
    public int sort_order { get; set; }
    public string? description { get; set; }
    public string? pc_image_url { get; set; }
    public image_info? pc_image_info { get; set; }
    public string? sp_image_url { get; set; }
    public image_info? sp_image_info { get; set; }
}
