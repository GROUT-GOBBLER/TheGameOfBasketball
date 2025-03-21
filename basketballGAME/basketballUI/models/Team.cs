using System;
using System.Collections.Generic;

namespace basketballUI.models;

public partial class Team
{
    public int TeamNo { get; set; }

    public string TeamName { get; set; } = null!;

    public string TeamAbbreviation { get; set; } = null!;

    public int Wins { get; set; }

    public int Losses { get; set; }

    public virtual ICollection<Game> GameTeamNoOneNavigations { get; set; } = new List<Game>();

    public virtual ICollection<Game> GameTeamNoTwoNavigations { get; set; } = new List<Game>();

    public virtual ICollection<TeamPlayer> TeamPlayers { get; set; } = new List<TeamPlayer>();
}
