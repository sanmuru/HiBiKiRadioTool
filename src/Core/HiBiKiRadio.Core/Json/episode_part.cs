namespace SamLu.Utility.HiBiKiRadio.Json;

#pragma warning disable CS1591
public class episode_part
{
    public int id { get; set; }
    public int? sort_order { get; set; }
    public string? description { get; set; }
    public string? pc_image_url { get; set; }
    public image_info? pc_image_info { get; set; }
    public string? sp_image_url { get; set; }
    public image_info? sp_image_info { get; set; }
    public string? updated_at { get; set; }
}
