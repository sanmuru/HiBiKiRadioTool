namespace SamLu.Utility.HiBiKiRadio.Json
{
    public class video
    {
        public int id{ get; set; }
        public double duration{ get; set; }
        public bool live_flg{ get; set; }
        public string delivery_start_at{ get; set; }
        public string delivery_end_at{ get; set; }
        public bool dvr_flg{ get; set; }
        public bool replay_flg{ get; set; }
        public int media_type{ get; set; }
    }
}
