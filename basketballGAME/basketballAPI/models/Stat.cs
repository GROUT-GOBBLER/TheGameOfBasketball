using System;
using System.Collections.Generic;

namespace basketballAPI.models;

public partial class Stat
{
    public int Id { get; set; }

    public int? PlayerTeamId { get; set; }

    public int? GameId { get; set; }

    public int? StatTypeId { get; set; }

    public int? StatValue { get; set; }

    public virtual Game? Game { get; set; }

    public virtual TeamPlayer? PlayerTeam { get; set; }

    public virtual StatsType? StatType { get; set; }
}
