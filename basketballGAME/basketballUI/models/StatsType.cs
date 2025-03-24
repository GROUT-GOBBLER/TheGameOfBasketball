using System;
using System.Collections.Generic;

namespace basketballUI.models;

public partial class StatsType
{
    public short StatId { get; set; }

    public string? StatName { get; set; }

    public string? StatAbv { get; set; }

    public virtual ICollection<Stat> Stats { get; set; } = new List<Stat>();
}
