// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;

namespace Qtyi.HiBiKiRadio.M3U8;

public class M3U8Document
{
    protected List<Insection> insections = new List<Insection>();

    protected List<M3U8Key> keys = new List<M3U8Key>();

    public int Version
    {
        get
        {
            var versionInsection = this.insections.OfType<TagInsection>().FirstOrDefault(i => i.Tag == M3U8Tag.EXT_X_VERSION);
            if (versionInsection is null) return 1;
            else return int.Parse(versionInsection.Value);
        }
    }

    public bool AllowCache
    {
        get
        {
            var allowCacheInsection = this.insections.OfType<TagInsection>().FirstOrDefault(i => i.Tag == M3U8Tag.EXT_X_ALLOW_CACHE);
            if (allowCacheInsection is null) return false;
            else return allowCacheInsection.Value == "yes";
        }
    }

    public IEnumerable<UriInsection> MediaClips
    {
        get
        {
            foreach (var insection in this.insections)
            {
                if (insection is UriInsection)
                    yield return (UriInsection)insection;
            }
        }
    }

    public void Load(string content)
    {
        if (content is null) throw new ArgumentNullException(nameof(content));

        this.Load(new StringReader(content));
    }

    public void Load(Stream stream, Encoding encoding = null)
    {
        if (stream is null) throw new ArgumentNullException(nameof(stream));

        this.Load(new StreamReader(stream, encoding ?? Encoding.UTF8));
    }

    public void LoadFromFile(string path)
    {
        if (path is null) throw new ArgumentNullException(nameof(path));

        this.Load(File.OpenRead(path), Encoding.UTF8);
    }

    private void Load(TextReader reader)
    {
        if (reader.ReadLine() != "#EXTM3U") throw new M3U8FormatException("M3U8格式错误：缺少‘EXTM3U’标签。");
        this.insections.Add(Insection.CreateTag(M3U8Tag.EXTM3U));

        string line = reader.ReadLine();
        while (!string.IsNullOrEmpty(line))
        {
            if (line.StartsWith("#"))
            {
                if (line.StartsWith("#EXT"))
                {
                    M3U8Tag tag = default;
                    string value = default;
                    if (line.Contains(":"))
                    {
                        tag = (M3U8Tag)Enum.Parse(typeof(M3U8Tag), line.Substring(1, line.IndexOf(':') - 1).Replace('-', '_'));
                        value = line.Substring(line.IndexOf(':') + 1);
                    }
                    else
                    {
                        tag = (M3U8Tag)Enum.Parse(typeof(M3U8Tag), line.Substring(1).Replace('-', '_'));
                    }

                    switch (tag)
                    {
                        case M3U8Tag.EXT_X_KEY:
                            this.keys.Add(new M3U8Key(value));
                            goto default;
                        default:
                            this.insections.Add(Insection.CreateTag(M3U8Tag.EXT_X_KEY, value));
                            break;
                    }
                }
                else
                    this.insections.Add(Insection.CreateComment(line));
            }
            else
                this.insections.Add(Insection.CreateUri(new Uri(line, UriKind.RelativeOrAbsolute), this.keys.Last()));

            line = reader.ReadLine();
        }
    }
}
