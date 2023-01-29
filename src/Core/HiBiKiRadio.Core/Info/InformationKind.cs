using System.Diagnostics;

namespace SamLu.Utility.HiBiKiRadio.Info;

public enum InformationKind
{
    [DebuggerDisplay("番組情報")]
    Bangumi = 1,
    [DebuggerDisplay("イベント情報")]
    Event = 2,
    [DebuggerDisplay("商品情報")]
    Goods = 3,
    [DebuggerDisplay("その他のお知らせ")]
    Else = 4
}