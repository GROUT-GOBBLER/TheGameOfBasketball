using System;
using System.Collections.Generic;

namespace basketballUI.models;

public partial class SavedList
{
    public static List<Player> CurrentPlayerList { get; set; } = new List<Player>();
}