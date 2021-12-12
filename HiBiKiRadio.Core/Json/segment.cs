namespace SamLu.Utility.HiBiKiRadio.Json
{
    public record class segment
    {
        public int id{ get; init; }
        public string name{ get; init; }
        public segment_part[] segment_parts{ get; init; }
        public string html_description{ get; init; }
        public string publish_start_at{ get; init; }
        public string publish_end_at{ get; init; }
        public string updated_at{ get; init; }
    }
}
