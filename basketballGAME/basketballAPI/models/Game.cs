using System;
using System.Collections.Generic;

namespace basketballAPI.models;

public partial class Game
{
    public int GameNo { get; set; }

    public int TeamNoOne { get; set; }

    public int TeamNoTwo { get; set; }

    public int ScoreOne { get; set; }

    public int ScoreTwo { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<Stat> Stats { get; set; } = new List<Stat>();

    public virtual Team TeamNoOneNavigation { get; set; } = null!;

    public virtual Team TeamNoTwoNavigation { get; set; } = null!;
}
