// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Qtyi.HiBiKiRadio.Json;

#pragma warning disable CS1591, CS8618
public class video
{
    public int id{ get; set; }
    public double duration{ get; set; }
    public bool live_flg{ get; set; }
    public string? delivery_start_at{ get; set; }
    public string? delivery_end_at{ get; set; }
    public bool dvr_flg{ get; set; }
    public bool replay_flg{ get; set; }
    public int media_type{ get; set; }
}
