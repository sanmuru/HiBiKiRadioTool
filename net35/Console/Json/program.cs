using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiBikiRadioTool.Json
{
    public class program
    {
        public string access_id;
        public int id;
        public string name;
        public string name_kana;
        public int day_of_week;
        public string description;
        public string pc_image_url;
        public image_info pc_image_info;
        public string sp_image_url;
        public image_info sp_image_info;
        public string onair_information;
        public string message_from_url;
        public string email;
        public bool new_program_flg;
        public string copyright;
        public int priority;
        public string meta_title;
        public string meta_keyword;
        public string meta_description;
        public string hash_tag;
        public string share_text;
        public string share_url;
        public string cast;
        public string publish_start_at;
        public string publish_end_at;
        public string updated_at;
        public int? latest_episode_id;
        public string latest_episode_name;
        public string episode_updated_at;
        public bool update_flg;
        public episode episode;
        public bool chapter_flg;
        public bool additional_video_flg;
        public int segment_count;
        public int program_information_count;
        public int product_information_count;
        public bool user_favorite_flag;
        public program_link[] program_links;
        public cast[] casts;
        public segment[] segments;
    }
}
