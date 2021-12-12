namespace SamLu.Utility.HiBiKiRadio.Json
{
    public record class segment_part
    {
        public int id{ get; init; }
        public int? sort_order{ get; init; }
        public string description{ get; init; }
        public string pc_image_url{ get; init; }
        public image_info pc_image_info{ get; init; }
        public string sp_image_url{ get; init; }
        public image_info sp_image_info{ get; init; }
        public string updated_at{ get; init; }
    }
}
