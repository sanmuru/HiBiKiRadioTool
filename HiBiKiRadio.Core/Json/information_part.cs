namespace SamLu.Utility.HiBiKiRadio.Json
{
    public record class information_part
    {
        public int sort_order { get; init; }
        public string description { get; init; }
        public string pc_image_url { get; init; }
        public image_info pc_image_info { get; init; }
        public string sp_image_url { get; init; }
        public image_info sp_image_info { get; init; }
    }
}
