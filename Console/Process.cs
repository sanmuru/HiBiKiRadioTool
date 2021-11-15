using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiBikiRadioTool
{
    public class Process
    {
        public static readonly Uri SiteDomain = new Uri("https://hibiki-radio.jp/", UriKind.Absolute);
        public static readonly Uri ProgramDescription = new Uri(SiteDomain, "description");

        public static string GetPageHtml(string programTitle, string subpage = "detail")
        {
            Uri uri = new Uri(ProgramDescription, programTitle);
            if (!(subpage is null)) uri = new Uri(uri, subpage);

           return SamLu.Web.HTML.GetSource(uri.AbsoluteUri);
        }
    }
}
