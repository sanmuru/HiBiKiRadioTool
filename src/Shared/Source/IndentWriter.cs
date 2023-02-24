// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Qtyi.HiBiKiRadio.Generators;

internal class IndentWriter
{
    protected readonly TextWriter _writer;

    protected readonly int _indentSize;
    private int _indentLevel;
    private bool _needIndent = true;

    public IndentWriter(TextWriter writer, CancellationToken cancellationToken) : this(writer, 4, cancellationToken) { }

    public IndentWriter(TextWriter writer, int indentSize, CancellationToken cancellationToken)
    {
        this._writer = writer;
        this._indentSize = indentSize;
        this.CancellationToken = cancellationToken;
    }

    protected CancellationToken CancellationToken { get; }

    #region Output helpers

    protected void Indent()
    {
        this._indentLevel++;
    }

    protected void Unindent()
    {
        if (this._indentLevel <= 0)
        {
            throw new InvalidOperationException("Cannot unindent from base level");
        }
        this._indentLevel--;
    }

    public void Write(string msg)
    {
        this.CancellationToken.ThrowIfCancellationRequested();

        this.WriteIndentIfNeeded();
        this._writer.Write(msg);
    }

    public void Write(string format, params object[] args) => this.Write(string.Format(format, args));

    public void WriteLine() => this.WriteLine("");

    public void WriteLine(string msg)
    {
        this.CancellationToken.ThrowIfCancellationRequested();

        if (msg != "")
        {
            this.WriteIndentIfNeeded();
        }

        this._writer.WriteLine(msg);
        this._needIndent = true; //need an indent after each line break
    }

    public void WriteLine(string format, params object[] args) => this.WriteLine(string.Format(format, args));

    public void WriteLineWithoutIndent(string msg)
    {
        this.CancellationToken.ThrowIfCancellationRequested();

        this._writer.WriteLine(msg);
        this._needIndent = true; //need an indent after each line break
    }

    public void WriteLineWithoutIndent(string format, params object[] args) => this.WriteLineWithoutIndent(string.Format(format, args));

    private void WriteIndentIfNeeded()
    {
        if (this._needIndent)
        {
            this._writer.Write(new string(' ', this._indentLevel * this._indentSize));
            this._needIndent = false;
        }
    }

    /// <summary>
    /// Joins all the values together in <paramref name="values"/> into one string with each
    /// value separated by a comma.  Values can be either <see cref="string"/>s or <see
    /// cref="IEnumerable{T}"/>s of <see cref="string"/>.  All of these are flattened into a
    /// single sequence that is joined. Empty strings are ignored.
    /// </summary>
    public string CommaJoin(params object[] values)
        => Join(", ", values);

    public static string Join(string separator, params object[] values)
        => string.Join(separator, values.SelectMany(v => (v switch
        {
            string s => new[] { s },
            IEnumerable<string> ss => ss,
            _ => throw new InvalidOperationException("Join must be passed strings or collections of strings")
        }).Where(s => s != "")));

    public virtual void OpenBlock()
    {
        this.WriteLine("{");
        this.Indent();
    }

    public virtual void CloseBlock(string extra = "")
    {
        this.Unindent();
        this.WriteLine("}" + extra);
    }

    #endregion Output helpers
}
