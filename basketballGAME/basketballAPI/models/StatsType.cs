using System;
using System.Collections.Generic;

namespace basketballAPI.models;

public partial class StatsType
{
    public int StatId { get; set; }

    public string? StatName { get; set; }

    public string? StatAbv { get; set; }

    public virtual ICollection<Stat> Stats { get; set; } = new List<Stat>();
}
