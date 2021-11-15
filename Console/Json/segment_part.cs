using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiBikiRadioTool.Json
{
    public class segment_part
    {
        public int id;
        public int? sort_order;
        public string description;
        public string pc_image_url;
        public image_info pc_image_info;
        public string sp_image_url;
        public image_info sp_image_info;
        public string updated_at;
    }
}
