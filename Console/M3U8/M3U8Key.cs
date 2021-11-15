using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace HiBikiRadioTool.M3U8
{
    public class M3U8Key : IDisposable
    {
        public string Method { get; }
        public Uri Uri { get; }
        public byte[] IV { get; }

        public M3U8Key(string attributes)
        {
            foreach (var pair in attributes.Split(','))
            {
                var key = pair.Substring(0, pair.IndexOf('='));
                var value = pair.Substring(pair.IndexOf('=') + 1);

                switch (key)
                {
                    case "METHOD":
                        this.Method = value;
                        break;
                    case "URI":
                        this.Uri = new Uri(value.Replace("\"", string.Empty), UriKind.Absolute);
                        break;
                    case "IV":
                        value = value.Substring(2);
                        byte[] iv = new byte[value.Length / 2];
                        for (int i = 0; i < iv.Length; i++)
                        {
                            iv[i] = Convert.ToByte(value.Substring(i * 2, 2), 16);
                        }
                        this.IV = iv;
                        break;
                }
            }
        }

        protected ICryptoTransform transform;

        public virtual Stream EncryptStream(Stream stream, Func<Uri, byte[]> keyProvider)
        {
            if (keyProvider is null) throw new ArgumentNullException(nameof(keyProvider));

            if (this.Method == "NONE") return stream;
            else
            {
                if (this.transform is null)
                {
                    byte[] key = keyProvider(this.Uri);
                    this.transform = new AesManaged().CreateDecryptor(key, this.IV);
                }

                MemoryStream ms = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(stream, this.transform, CryptoStreamMode.Read))
                {
                    byte[] buffer = new byte[byte.MaxValue];
                    int count;
                    do
                    {
                        count = cs.Read(buffer, 0, buffer.Length);
                        ms.Write(buffer, 0, count);
                    }
                    while (count > 0);
                }

                ms.Seek(0, SeekOrigin.Begin); // 复位到流的头部。
                return ms;
            }
        }

        #region IDisposable
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                this.transform.Dispose();

                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~M3U8Key()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
