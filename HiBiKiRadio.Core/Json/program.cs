namespace SamLu.Utility.HiBiKiRadio.Json
{
    public record class program
    {
        public string access_id { get; init; }
        public int id { get; init; }
        public string name { get; init; }
        public string name_kana { get; init; }
        public int day_of_week { get; init; }
        public string description { get; init; }
        public string pc_image_url { get; init; }
        public image_info pc_image_info { get; init; }
        public string sp_image_url { get; init; }
        public image_info sp_image_info { get; init; }
        public string onair_information { get; init; }
        public string message_from_url { get; init; }
        public string email { get; init; }
        public bool new_program_flg { get; init; }
        public string copyright { get; init; }
        public int priority { get; init; }
        public string meta_title { get; init; }
        public string meta_keyword { get; init; }
        public string meta_description { get; init; }
        public string hash_tag { get; init; }
        public string share_text { get; init; }
        public string share_url { get; init; }
        public string cast { get; init; }
        public string publish_start_at { get; init; }
        public string publish_end_at { get; init; }
        public string updated_at { get; init; }
        public int? latest_episode_id { get; init; }
        public string latest_episode_name { get; init; }
        public string episode_updated_at { get; init; }
        public bool update_flg { get; init; }
        public episode episode { get; init; }
        public bool chapter_flg { get; init; }
        public bool additional_video_flg { get; init; }
        public int segment_count { get; init; }
        public int program_information_count { get; init; }
        public int product_information_count { get; init; }
        public bool user_favorite_flag { get; init; }
        public program_link[] program_links { get; init; }
        public cast[] casts { get; init; }
        public segment[] segments { get; init; }
    }
}
