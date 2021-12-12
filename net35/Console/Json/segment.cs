using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiBikiRadioTool.Json
{
    public class segment
    {
        public int id;
        public string name;
        public segment_part[] segment_parts;
        public string html_description;
        public string publish_start_at;
        public string publish_end_at;
        public string updated_at;
    }
}
