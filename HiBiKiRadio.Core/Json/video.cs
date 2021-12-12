namespace SamLu.Utility.HiBiKiRadio.Json
{
    public record class video
    {
        public int id{ get; init; }
        public double duration{ get; init; }
        public bool live_flg{ get; init; }
        public string delivery_start_at{ get; init; }
        public string delivery_end_at{ get; init; }
        public bool dvr_flg{ get; init; }
        public bool replay_flg{ get; init; }
        public int media_type{ get; init; }
    }
}
