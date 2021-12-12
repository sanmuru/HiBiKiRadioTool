namespace SamLu.Utility.HiBiKiRadio.Json
{
    public record class information
    {
        public int id { get; init; }
        public string day { get; init; }
        public string name { get; init; }
        public int kind { get; init; }
        public string kind_name { get; init; }
        public int priority { get; init; }
        public string link_url { get; init; }
        public string pc_image_url { get; init; }
        public image_info pc_image_info { get; init; }
        public string sp_image_url { get; init; }
        public image_info sp_image_info { get; init; }
        public information_part[] information_parts { get; init; }
        public string html_description { get; init; }
        public string publish_start_at { get; init; }
        public string publish_end_at { get; init; }
        public string updated_at { get; init; }
    }
}
