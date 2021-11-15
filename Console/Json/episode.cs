using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiBikiRadioTool.Json
{
    public class episode
    {
        public int id;
        public string name;
        public string media_type;
        public video video;
        public video additional_video;
        public string html_description;
        public string link_url;
        public string updated_at;
        public episode_part[] episode_parts;
        public chapter[] chapters;
    }
}