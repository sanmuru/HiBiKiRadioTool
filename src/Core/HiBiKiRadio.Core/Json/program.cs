// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Qtyi.HiBiKiRadio.Json;

#pragma warning disable CS1591, CS8618
public class program
{
    public string? access_id { get; set; }
    public int id { get; set; }
    public string? name { get; set; }
    public string? name_kana { get; set; }
    public int day_of_week { get; set; }
    public string? description { get; set; }
    public string? pc_image_url { get; set; }
    public image_info? pc_image_info { get; set; }
    public string? sp_image_url { get; set; }
    public image_info? sp_image_info { get; set; }
    public string? onair_information { get; set; }
    public string? message_from_url { get; set; }
    public string? email { get; set; }
    public bool new_program_flg { get; set; }
    public string? copyright { get; set; }
    public int priority { get; set; }
    public string? meta_title { get; set; }
    public string? meta_keyword { get; set; }
    public string? meta_description { get; set; }
    public string? hash_tag { get; set; }
    public string? share_text { get; set; }
    public string? share_url { get; set; }
    public string? cast { get; set; }
    public string? publish_start_at { get; set; }
    public string? publish_end_at { get; set; }
    public string? updated_at { get; set; }
    public int? latest_episode_id { get; set; }
    public string? latest_episode_name { get; set; }
    public string? episode_updated_at { get; set; }
    public bool update_flg { get; set; }
    public episode? episode { get; set; }
    public bool chapter_flg { get; set; }
    public bool additional_video_flg { get; set; }
    public int segment_count { get; set; }
    public int program_information_count { get; set; }
    public int product_information_count { get; set; }
    public bool user_favorite_flag { get; set; }
    public program_link[]? program_links { get; set; }
    public cast[]? casts { get; set; }
    public segment[]? segments { get; set; }
}
