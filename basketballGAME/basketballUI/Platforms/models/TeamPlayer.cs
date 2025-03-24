using System;
using System.Collections.Generic;

namespace basketballAPI.models;

public partial class TeamPlayer
{
    public int Id { get; set; }

    public int? PlayerId { get; set; }

    public int? TeamId { get; set; }

    public virtual Player? Player { get; set; }

    public virtual ICollection<Stat> Stats { get; set; } = new List<Stat>();

    public virtual Team? Team { get; set; }
}
