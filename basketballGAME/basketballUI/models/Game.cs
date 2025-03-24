using System;
using System.Collections.Generic;

namespace basketballUI.models;

public partial class Game
{
    public int GameNo { get; set; }

    public int TeamNoOne { get; set; }

    public int TeamNoTwo { get; set; }

    public short ScoreOne { get; set; }

    public short ScoreTwo { get; set; }

    public virtual ICollection<Stat> Stats { get; set; } = new List<Stat>();

    public virtual Team TeamNoOneNavigation { get; set; } = null!;

    public virtual Team TeamNoTwoNavigation { get; set; } = null!;
}
