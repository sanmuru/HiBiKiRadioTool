// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace Qtyi.HiBiKiRadio.Info;

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