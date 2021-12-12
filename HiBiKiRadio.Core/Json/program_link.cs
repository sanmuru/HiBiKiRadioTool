namespace SamLu.Utility.HiBiKiRadio.Json
{
    public record class program_link
    {
        public int id{ get; init; }
        public string name{ get; init; }
        public string pc_image_url{ get; init; }
        public image_info pc_image_info{ get; init; }
        public string sp_image_url{ get; init; }
        public image_info sp_image_info{ get; init; }
        public string link_url{ get; init; }
    }
}
