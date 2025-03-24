using System;
using System.Collections.Generic;

namespace basketballAPI.models;

public partial class Player
{
    public int PlayerNo { get; set; }

    public string FName { get; set; } = null!;

    public string LName { get; set; } = null!;

    public virtual ICollection<TeamPlayer> TeamPlayers { get; set; } = new List<TeamPlayer>();
}
