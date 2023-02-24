// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CodeAnalysis.Text;

namespace Qtyi.HiBiKiRadio.Generators;

/// <summary>
/// 表示可以读取源代码文本的读取器。
/// </summary>
internal sealed class SourceTextReader : TextReader
{
    private readonly SourceText _sourceText;
    private int _position;

    /// <summary>
    /// 使用指定的源代码文本初始化<see cref="SourceTextReader"/>的新实例。
    /// </summary>
    /// <param name="sourceText">要读取的源代码文本。</param>
    public SourceTextReader(SourceText sourceText)
    {
        this._sourceText = sourceText;
        this._position = 0;
    }

    public override int Peek()
    {
        if (this._position == this._sourceText.Length)
            return -1;

        return this._sourceText[this._position];
    }

    public override int Read()
    {
        if (this._position == this._sourceText.Length)
        {
            return -1;
        }

        return this._sourceText[this._position++];
    }

    public override int Read(char[] buffer, int index, int count)
    {
        var charsToCopy = Math.Min(count, this._sourceText.Length - this._position);
        this._sourceText.CopyTo(this._position, buffer, index, charsToCopy);
        this._position += charsToCopy;
        return charsToCopy;
    }
}
