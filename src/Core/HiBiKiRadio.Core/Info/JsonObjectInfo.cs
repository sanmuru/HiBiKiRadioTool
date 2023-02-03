using System.Globalization;

namespace SamLu.Utility.HiBiKiRadio.Info;

/// <summary>
/// 表示包裹JSON映射对象的类型的基类。
/// </summary>
/// <typeparam name="T">被包裹的JSON映射对象的类型。</typeparam>
public abstract class JsonObjectInfo<T> where T : notnull
{
    /// <summary>
    /// 被包裹的JSON映射对象。
    /// </summary>
    protected readonly T jObject;

    /// <summary>
    /// 子类调用以初始化新实例，设置包裹的JSON映射对象。
    /// </summary>
    /// <param name="jObject">被包裹的JSON映射对象。</param>
    /// <exception cref="ArgumentNullException"><paramref name="jObject"/>的值为<see langword="null"/>。</exception>
    protected JsonObjectInfo(T jObject) => this.jObject = jObject ?? throw new ArgumentNullException(nameof(jObject));

    protected const string DateTimeFormat = "yyyy/MM/dd HH:mm:ss";
    protected virtual bool TryParseDateTimeUtc(string dateTimeString, out DateTime result)
    {
        if (DateTime.TryParseExact(dateTimeString, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
        {
            result = result.AddHours(-9);
            return true;
        }
        return false;
    }

    public static bool TryParseDateTimeUrc(JsonObjectInfo<T> info, string dateTimeString, out DateTime result) => info.TryParseDateTimeUtc(dateTimeString, out result);

    protected virtual string FormatDateTime(DateTime dateTime) => dateTime.ToString(DateTimeFormat);

    public static string FormatDateTime(JsonObjectInfo<T> info, DateTime dateTime) => info.FormatDateTime(dateTime);

    protected static DateTime UtcToLocal(DateTime local) => local + (DateTime.Now - DateTime.UtcNow);

    protected static DateTime LocalToUtc(DateTime utc) => utc + (DateTime.UtcNow - DateTime.Now);
}
