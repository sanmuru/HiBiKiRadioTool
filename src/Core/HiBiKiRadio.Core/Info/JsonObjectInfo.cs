// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Qtyi.HiBiKiRadio.Json.Converters;

namespace Qtyi.HiBiKiRadio.Info;

/// <summary>
/// 表示包裹JSON映射对象的类型的基类。
/// </summary>
public abstract class JsonObjectInfo
{
    /// <summary>
    /// 获取被包裹的JSON映射对象。
    /// </summary>
    public object JsonObject { get; }

    /// <summary>
    /// 子类调用以初始化新实例，设置包裹的JSON映射对象。
    /// </summary>
    /// <param name="jObject">被包裹的JSON映射对象。</param>
    /// <exception cref="ArgumentNullException"><paramref name="jObject"/>的值为<see langword="null"/>。</exception>
    protected JsonObjectInfo(object jObject) => this.JsonObject = jObject ?? throw new ArgumentNullException(nameof(jObject));

    #region 转换器
    public static DateTimeJsonConverter DateTimeConverter { get; } = new();
    public static DateTimeJsonConverter JapanDateTimeConverter { get; } = DateTimeConverter.WithTimezone(9, adjustConvertFrom: true, adjustConvertTo: false);

    public static SizeJsonConverter SizeConverter { get; } = new();

    public static UriJsonConverter UriConverter { get; } = new();
    #endregion
}
