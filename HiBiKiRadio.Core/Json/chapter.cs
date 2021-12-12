namespace SamLu.Utility.HiBiKiRadio.Json
{
    public record class chapter
    {
        public int id { get; init; }
        public double start_time { get; init; }
        public string pc_image_url { get; init; }
        public image_info pc_image_info { get; init; }
        public string sp_image_url { get; init; }
        public image_info sp_image_info { get; init; }
        public string name { get; init; }
        public string description { get; init; }
    }
}
