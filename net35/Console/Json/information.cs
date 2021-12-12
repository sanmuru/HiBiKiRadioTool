using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiBikiRadioTool.Json
{
    public class information
    {
        public int id;
        public string day;
        public string name;
        public int kind;
        public string kind_name;
        public int priority;
        public string link_url;
        public string pc_image_url;
        public image_info pc_image_info;
        public string sp_image_url;
        public image_info sp_image_info;
        public information_part[] information_parts;
        public string html_description;
        public string publish_start_at;
        public string publish_end_at;
        public string updated_at;
    }
}
