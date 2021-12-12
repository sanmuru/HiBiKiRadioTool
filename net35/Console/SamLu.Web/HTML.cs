//HTML.cs

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SamLu.Web
{
    public static class HTML
    {
        public static readonly Dictionary<string, char> EscapeCharactersDictionary = new Dictionary<string, char>() {
            { "&quot;", '"' },
            { "&amp;", '&' },
            { "&lt;", '<' },
            { "&gt;", '>' },
            { "&nbsp;", ' ' },
            { "&iexcl;", '¡' },
            { "&Aacute;", 'Á' },
            { "&aacute;", 'á' },
            { "&cent;", '¢' },
            { "&circ;", 'Â' },
            { "&pound;", '£' },
            { "&Atilde;", 'Ã' },
            { "&atilde;", 'ã' },
            { "&curren;", '¤' },
            { "&auml;", 'ä' },
            { "&yen;", '¥' },
            { "&ring;", 'Å' },
            { "&aring;", 'å' },
            { "&brvbar;", '¦' },
            { "&AElig;", 'Æ' },
            { "&aelig;", 'æ' },
            { "&sect;", '§' },
            { "&Ccedil;", 'Ç' },
            { "&ccedil;", 'ç' },
            { "&uml;", '¨' },
            { "&Egrave;", 'È' },
            { "&egrave;", 'è' },
            { "&copy;", '©' },
            { "&Eacute;", 'É' },
            { "&eacute;", 'é' },
            { "&ordf;", 'ª' },
            { "&Ecirc;", 'Ê' },
            { "&ecirc;", 'ê' },
            { "&laquo;", '«' },
            { "&Euml;", 'Ë' },
            { "&euml;", 'ë' },
            { "&not;", '¬' },
            { "&Igrave;", 'Ì' },
            { "&igrave;", 'ì' },
            { "&shy;", '­' },
            { "&Iacute;", 'Í' },
            { "&iacute;", 'í' },
            { "&reg;", '®' },
            { "&Icirc;", 'Î' },
            { "&icirc;", 'î' },
            { "&macr;", '¯' },
            { "&Iuml;", 'Ï' },
            { "&iuml;", 'ï' },
            { "&deg;", '°' },
            { "&ETH;", 'Ð' },
            { "&ieth;", 'ð' },
            { "&plusmn;", '±' },
            { "&Ntilde;", 'Ñ' },
            { "&ntilde;", 'ñ' },
            { "&sup2;", '²' },
            { "&Ograve;", 'Ò' },
            { "&ograve;", 'ò' },
            { "&sup3;", '³' },
            { "&Oacute;", 'Ó' },
            { "&oacute;", 'ó' },
            { "&acute;", '´' },
            { "&Ocirc;", 'Ô' },
            { "&ocirc;", 'ô' },
            { "&micro;", 'µ' },
            { "&Otilde;", 'Õ' },
            { "&otilde;", 'õ' },
            { "&para;", '¶' },
            { "&Ouml;", 'Ö' },
            { "&ouml;", 'ö' },
            { "&middot;", '·' },
            { "&times;", '×' },
            { "&divide;", '÷' },
            { "&cedil;", '¸' },
            { "&Oslash;", 'Ø' },
            { "&oslash;", 'ø' },
            { "&sup1;", '¹' },
            { "&Ugrave;", 'Ù' },
            { "&ugrave;", 'ù' },
            { "&ordm;", 'º' },
            { "&Uacute;", 'Ú' },
            { "&uacute;", 'ú' },
            { "&raquo;", '»' },
            { "&Ucirc;", 'Û' },
            { "&ucirc;", 'û' },
            { "&frac14;", '¼' },
            { "&Uuml;", 'Ü' },
            { "&uuml;", 'ü' },
            { "&frac12;", '½' },
            { "&Yacute;", 'Ý' },
            { "&yacute;", 'ý' },
            { "&frac34;", '¾' },
            { "&THORN;", 'Þ' },
            { "&thorn;", 'þ' },
            { "&iquest;", '¿' },
            { "&szlig;", 'ß' },
            { "&yuml;", 'ÿ' },
            { "&Agrave;", 'À' },
            { "&agrave;", 'à' }
        };

        public static string InterpretEscapeCharacters(string str)
        {
            if (str is null) throw new ArgumentNullException("str");

            foreach (Match m in Regex.Matches(str, "((&\\w+?;)|(&#\\d+;))"))
            {
                string matchStr = m.Value;
                if (Regex.IsMatch(matchStr, "&\\w+?;") && HTML.EscapeCharactersDictionary.ContainsKey(matchStr))
                    str = str.Replace(matchStr, HTML.EscapeCharactersDictionary[matchStr].ToString());
                else if (Regex.IsMatch(matchStr, "&#\\d+;"))
                    str = str.Replace(matchStr, ((char)ushort.Parse(matchStr.Substring(2, matchStr.Length - 3))).ToString());
            }

            return str;
        }

#if UPDATE
        
        public static string GetSource(string url) {
            if (url is null) throw new ArgumentNullException("url");
            if (url.Trim() == string.Empty) throw new ArgumentOutOfRangeException("url", url, string.Empty);
            
            return HTML.GetSource(url, null);
        }
        
        public static string GetSource(string url, Encoding encoding) {
            if (url is null) throw new ArgumentNullException("url");
            if (url.Trim() == string.Empty) throw new ArgumentOutOfRangeException("url", url, string.Empty);
            
            //generate http request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //use GET method to get source
            request.Method = "GET";
            //use request to get response
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            
            //get source from response
            string source = null;
            encoding = encoding ?? Encoding.GetEncoding(response.ContentEncoding);
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), encoding))
            {
                source = sr.ReadToEnd();
            }
            
            return source;
        }
        
#endif

#if !UPDATE

        public static string GetSource(string url)
        {
            if (url is null) throw new ArgumentNullException("url");
            if (url.Trim() == string.Empty) throw new ArgumentOutOfRangeException("url", url, string.Empty);

            return HTML.GetSource(url, Encoding.UTF8);
        }

        public static string GetSource(string url, Encoding encoding)
        {
            if (url is null) throw new ArgumentNullException("url");
            if (encoding is null) throw new ArgumentNullException("encoding");
            if (url.Trim() == string.Empty) throw new ArgumentOutOfRangeException("url", url, string.Empty);

            string source = null;

            WebRequest request = WebRequest.Create(url);
            request.Timeout = 30000;
            request.Headers.Set("Pragma", "no-cache");
            WebResponse response = request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(streamReceive, encoding);
            source = streamReader.ReadToEnd();
            streamReader.Close();

            if ((source is null) || (source.Trim() == string.Empty))
                source = HTML.GetSourceCompulsively(url);

            return source;
        }

#endif

        internal static string GetSourceCompulsively(string url)
        {
            const string getterPath = "..\\HTMLSourceGetter.exe";

            if (!File.Exists(getterPath)) throw new NotSupportedException("HTMLSourceGetter not found.");

            if (url is null) throw new ArgumentNullException("url");
            if (url.Trim() == string.Empty) throw new ArgumentOutOfRangeException("url", url, string.Empty);

            string source = null;

            string fileName = string.Format("_getter_{0}[{1}].html", DateTime.Now.ToString("yyyyMMddHHmmssfff"), new Random().Next(0, byte.MaxValue + 1));
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.Arguments = string.Format("\"{0}\" {1}", url, fileName);
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = getterPath;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            string strRst = process.StandardOutput.ReadToEnd();
            process.Close();

            Exception ex = null;

            try
            {

                if (!Regex.IsMatch(strRst, "\tCOMPLETED: \\d+, ERROR: \\d+?\r\n\t((COMPLETED LIST:\r\n\t\t\"(\\s|\\S)*?\")|(ERROR LIST:\r\n\t\t\"(\\s|\\S)*?\"))\r\n")) throw new InvalidOperationException();
                string[] lines = strRst.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                if (lines[1].Trim().StartsWith("COMPLETED"))
                    return source = File.ReadAllText(lines[2].Trim().Trim('"'));
                else
                    throw new InvalidOperationException(string.Format("Unknown error raised while processing \"{0}\"", lines[2].Trim()));

            }
            catch (Exception e)
            {
                ex = ex ?? e;
            }
            finally
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);
            }

            if (!(ex is null)) throw ex;

            return source;
        }

        public static void DownloadFile(string url, string filePath)
        {
            if (url is null) throw new ArgumentNullException("url");
            if (filePath is null) throw new ArgumentNullException("filePath");
            if (url.Trim() == string.Empty) throw new ArgumentOutOfRangeException("url", url, string.Empty);
            if (filePath.Trim() == string.Empty) throw new ArgumentOutOfRangeException("filePath", filePath, string.Empty);

            using (WebClient mywebclient = new WebClient())
            {
                mywebclient.DownloadFile(url, filePath);
            }
        }

        //static void Main() {
        //    string source = GetSource("http://lknovel.lightnovel.cn/main/vollist/83.html");
        //    Console.WriteLine(source);
        //}
    }
}