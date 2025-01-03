using System;
using System.Collections.Generic;

public partial class Mileage
{
    public string Number { get; set; } = null!;

    public DateOnly FixationDate { get; set; }

    public long FixationValue { get; set; }

    public virtual Car NumberNavigation { get; set; } = null!;
}
