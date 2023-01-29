using SamLu.Utility.HiBiKiRadio.Info;
using SamLu.Utility.HiBiKiRadio.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HiBiKiRadioTool.Launcher;

internal sealed class DataModelView
{
    public static ProgramList AllPrograms => ProgramList.Total;
}

internal sealed class ProgramList : IEnumerable<ProgramInfo>, INotifyCollectionChanged
{
    private readonly ProgramList? _owner;
    private readonly DayOfWeek _weekday;

    private ProgramInfo[] _programs =
#if NET35
        new ProgramInfo[0];
#else
        Array.Empty<ProgramInfo>();
#endif

    internal static readonly ProgramList Total = new();
    internal static readonly ProgramList OnMonday = new(Total, DayOfWeek.Monday);
    internal static readonly ProgramList OnTuesday = new(Total, DayOfWeek.Tuesday);
    internal static readonly ProgramList OnWednesday = new(Total, DayOfWeek.Wednesday);
    internal static readonly ProgramList OnThursday = new(Total, DayOfWeek.Thursday);
    internal static readonly ProgramList OnFriday = new(Total, DayOfWeek.Friday);
    internal static readonly ProgramList OnSaturday = new(Total, DayOfWeek.Saturday);
    internal static readonly ProgramList OnSunday = new(Total, DayOfWeek.Sunday);

    private ProgramList(ProgramList owner, DayOfWeek weekday)
    {
        this._owner = owner;
        this._weekday = weekday;
    }

    public ProgramList() { }

    public Task Refresh()
    {
        if (this._owner is not null)
            return this._owner.Refresh();
        else
            return this.RefreshTotal();
    }

    private Task RefreshTotal() => Task.Factory.StartNew(() =>
    {
        Debug.Assert(this._owner is null); // 仅在顶层节目列表中调用。

        ProgramListTask task = new();

        var newPrograms = task.FetchAsync().Result;
        this._programs = newPrograms;

        if (true)
        {
            OnMonday.RefreshWeekday();
            OnTuesday.RefreshWeekday();
            OnWednesday.RefreshWeekday();
            OnThursday.RefreshWeekday();
            OnFriday.RefreshWeekday();
            OnSaturday.RefreshWeekday();
            OnSunday.RefreshWeekday();
        }

        Application.Current.Dispatcher.Invoke(() =>
        {
            this.CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Reset));
            this.CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Add, newPrograms));
        });
    });

    [MemberNotNull(nameof(_owner))]
    private void RefreshWeekday()
    {
        Debug.Assert(this._owner is not null); // 仅在每天节目列表中调用。

        var newPrograms = this._owner._programs.Where(pi => pi.DayOfWeek == this._weekday).ToArray();
        this._programs = newPrograms;

        Application.Current.Dispatcher.Invoke(() =>
        {
            this.CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Reset));
            this.CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Reset, newPrograms));
        });
    }

    public IEnumerator<ProgramInfo> GetEnumerator() => ((IEnumerable<ProgramInfo>)this._programs).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
}
