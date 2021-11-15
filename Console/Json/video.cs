using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiBikiRadioTool.Json
{
    public class video
    {
        public int id;
        public double duration;
        public bool live_flg;
        public string delivery_start_at;
        public string delivery_end_at;
        public bool dvr_flg;
        public bool replay_flg;
        public int media_type;
    }
}
