// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Qtyi.HiBiKiRadio.Tasks;

namespace Qtyi.HiBiKiRadio.UnitTests;

public class JsonValuesTests
{
    private static readonly JToken? s_Program = JsonConvert.DeserializeObject(new ProgramListTask().Client.GetStringAsync(new Uri(ApiTaskBase.ApiBase, "programs")).Result) as JToken;
    [MemberNotNull(nameof(s_Program))]
    internal static JArray Programs
    {
        get
        {
            Debug.Assert(s_Program is not null && s_Program.Type == JTokenType.Array);
            return (JArray)s_Program;
        }
    }

    [Fact]
    public void ProgramListArrayTest()
    {
        Assert.NotNull(s_Program);
        Assert.Equal(JTokenType.Array, s_Program.Type);
    }

    public static IEnumerable<object[]> ProgramsData => Programs.Select(token =>
    {
        var program = token as JObject;
        Debug.Assert(program is not null);
        return new object[] { program };
    });

    [Theory]
    [MemberData(nameof(ProgramsData))]
    public void ProgramObjectTest(JObject program)
    {
        Assert.True(program.TryGetValue(nameof(Json.program.access_id), out var access_id));
        Assert.Equal(JTokenType.String, access_id.Type);
        Assert.False(string.IsNullOrWhiteSpace(access_id.ToString()));

        Assert.True(program.TryGetValue(nameof(Json.program.id), out var id));
        Assert.Equal(JTokenType.Integer, id.Type);
        Assert.True(id.Value<int>() > 0);

        Assert.True(program.TryGetValue(nameof(Json.program.name), out var name));
        Assert.Equal(JTokenType.String, name.Type);
        Assert.False(string.IsNullOrWhiteSpace(name.ToString()));

        Assert.True(program.TryGetValue(nameof(Json.program.name_kana), out var name_kana));
        Assert.Equal(JTokenType.String, name_kana.Type);

        Assert.True(program.TryGetValue(nameof(Json.program.day_of_week), out var day_of_week));
        Assert.Equal(JTokenType.Integer, day_of_week.Type);
        Assert.True(day_of_week.Value<int>() is >= 0 and < 7);

#error 检查其他JSON属性是否正确映射类型。

        Assert.Empty(
            program.Properties().Select(p => p.Name)
            .Except(typeof(Json.program).GetProperties().Select(p => p.Name)));
    }
}
