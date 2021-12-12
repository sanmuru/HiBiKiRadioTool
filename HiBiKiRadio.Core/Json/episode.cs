namespace SamLu.Utility.HiBiKiRadio.Json
{
    public record class episode
    {
        public int id { get; init; }
        public string name { get; init; }
        public int media_type { get; init; }
        public video video { get; init; }
        public video additional_video { get; init; }
        public string html_description { get; init; }
        public string link_url { get; init; }
        public string updated_at { get; init; }
        public episode_part[] episode_parts { get; init; }
        public chapter[] chapters { get; init; }
    }
}