// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Qtyi.HiBiKiRadio.Generators;

using System.Collections.Immutable;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Model;

[Generator]
public sealed class JsonTypeGenerator : ISourceGenerator
{
    private const string JsonXml = "Json.xml";

    private static readonly DiagnosticDescriptor s_MissingJsonXml = new DiagnosticDescriptor(
        "HBKRG1001",
        title: $"缺失“{JsonXml}”",
        messageFormat: $"“{JsonXml}”未被包含于项目中，因此将不会生成源。",
        category: "JsonTypeGenerator",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor s_UnableToReadJsonXml = new DiagnosticDescriptor(
        "HBKRG1002",
        title: $"无法读取“{JsonXml}”",
        messageFormat: $"无法读取“{JsonXml}”，可能是文件不存在或被占用。",
        category: "JsonTypeGenerator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor s_JsonXmlError = new DiagnosticDescriptor(
        "HBKRG1003",
        title: $"“{JsonXml}”中存在语法错误",
        messageFormat: "{0}",
        category: "JsonTypeGenerator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public void Execute(GeneratorExecutionContext context)
    {
        if (!this.TryGetRelevantInput(context, out var inputFile, out var inputText)) return;

        if (!this.TryGetResponse(context, inputFile, inputText, out var response)) return;

        var stringBuilder = new StringBuilder();
        using TextWriter textWriter = new StringWriter(stringBuilder);
        JsonTypeSourceWriter.WriteSource(textWriter, response, context.CancellationToken);
        var sourceText = SourceText.From(new StringBuilderReader(stringBuilder), stringBuilder.Length, encoding: Encoding.UTF8);
        context.AddSource("JsonTypes.cs", sourceText);
    }

    private bool TryGetRelevantInput(GeneratorExecutionContext context,
        [NotNullWhen(true)] out AdditionalText? inputFile,
        [NotNullWhen(true)] out SourceText? inputText)
    {
        inputFile = context.AdditionalFiles.SingleOrDefault(at => string.Equals(Path.GetFileName(at.Path), JsonXml, StringComparison.OrdinalIgnoreCase));
        if (inputFile is null)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                s_MissingJsonXml,
                location: null));
            inputText = null;
            return false;
        }

        inputText = inputFile.GetText(context.CancellationToken);
        if (inputText is null)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                s_UnableToReadJsonXml,
                location: null));
            return false;
        }

        return true;
    }

    private bool TryGetResponse(GeneratorExecutionContext context, AdditionalText inputFile, SourceText inputText,
        [NotNullWhen(true)] out Response? response)
    {
        var reader = XmlReader.Create(new SourceTextReader(inputText), new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit });
        try
        {
            var serializer = new XmlSerializer(typeof(Response));
            response = (Response)serializer.Deserialize(reader);
        }
        catch (InvalidOperationException ex) when (ex.InnerException is XmlException exception)
        {
            var xmlException = exception;

            var line = inputText.Lines[xmlException.LineNumber - 1]; // LineNumber is one-based.
            var offset = xmlException.LinePosition - 1; // LinePosition is one-based
            var position = line.Start + offset;
            var span = new TextSpan(position, 0);
            var lineSpan = inputText.Lines.GetLinePositionSpan(span);

            response = null;
            context.ReportDiagnostic(Diagnostic.Create(
                s_JsonXmlError,
                location: Location.Create(inputFile.Path, span, lineSpan),
                xmlException.Message));

            return false;
        }

        return true;
    }

    void ISourceGenerator.Initialize(GeneratorInitializationContext context) { }
}
