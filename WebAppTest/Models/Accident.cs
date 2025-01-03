using System;
using System.Collections.Generic;

public partial class Accident
{
    public string Number { get; set; } = null!;

    public DateOnly Date { get; set; }

    public string Type { get; set; } = null!;

    public string Region { get; set; } = null!;

    public virtual Car NumberNavigation { get; set; } = null!;
}
