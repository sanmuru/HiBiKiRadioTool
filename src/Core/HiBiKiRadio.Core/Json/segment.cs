namespace SamLu.Utility.HiBiKiRadio.Json
{
    public class segment
    {
        public int id{ get; set; }
        public string name{ get; set; }
        public segment_part[] segment_parts{ get; set; }
        public string html_description{ get; set; }
        public string publish_start_at{ get; set; }
        public string publish_end_at{ get; set; }
        public string updated_at{ get; set; }
    }
}
