﻿namespace SamLu.Utility.HiBiKiRadio.Json;

#pragma warning disable CS1591, CS8618
public class chapter
{
    public int id { get; set; }
    public string? name { get; set; }
    public string? description { get; set; }
    public double start_time { get; set; }
    public string? pc_image_url { get; set; }
    public image_info? pc_image_info { get; set; }
    public string? sp_image_url { get; set; }
    public image_info? sp_image_info { get; set; }
}
