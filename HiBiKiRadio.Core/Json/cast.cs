namespace SamLu.Utility.HiBiKiRadio.Json
{
    public record class cast
    {
        public int id { get; init; }
        public string name { get; init; }
        public string roll_name { get; init; }
        public string? pc_image_url { get; init; }
        public image_info? pc_image_info { get; init; }
        public string? sp_image_url { get; init; }
        public image_info? sp_image_info { get; init; }
        public string publish_start_at { get; init; }
        public string publish_end_at { get; init; }
        public string updated_at { get; init; }
    }
}
