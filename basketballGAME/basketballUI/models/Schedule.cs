using System;
using System.Collections.Generic;

namespace basketballUI.models;

public partial class Schedule
{
    public string GameDate { get; set; } = null!;

    public string GameTime { get; set; } = null!;

    public int GameNo { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Zipcode { get; set; }
}
