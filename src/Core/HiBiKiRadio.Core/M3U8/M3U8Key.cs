// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Security.Cryptography;

namespace Qtyi.HiBiKiRadio.M3U8;

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
                    var iv = new byte[value.Length / 2];
                    for (var i = 0; i < iv.Length; i++)
                    {
                        iv[i] = Convert.ToByte(value.Substring(i * 2, 2), 16);
                    }
                    this.IV = iv;
                    break;
            }
        }

        Debug.Assert(this.Method is not null);
        Debug.Assert(this.Uri is not null);
        Debug.Assert(this.IV is not null);
    }

    protected ICryptoTransform? transform;

    public virtual Stream EncryptStream(Stream stream, Func<Uri, byte[]> keyProvider, CancellationToken cancellationToken = default)
    {
        if (keyProvider is null) throw new ArgumentNullException(nameof(keyProvider));

        if (this.Method == "NONE") return stream;
        else
        {
            if (this.transform is null)
            {
                var key = keyProvider(this.Uri);
                this.transform = Aes.Create().CreateDecryptor(key, this.IV);
            }

            MemoryStream ms = new();
            using (CryptoStream cs = new(stream, this.transform, CryptoStreamMode.Read))
            {
                var buffer = new byte[byte.MaxValue];
                int count;
                do
                {
                    cancellationToken.ThrowIfCancellationRequested();
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
                this.transform?.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
