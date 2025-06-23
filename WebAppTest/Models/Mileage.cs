using System;
using System.Collections.Generic;

namespace WebAppDataBaseCars.Models;

public partial class Mileage
{
    public int Id { get; set; }

    public string Number { get; set; } = null!;

    public DateOnly FixationDate { get; set; }

    public long FixationValue { get; set; }

    public virtual Car NumberNavigation { get; set; } = null!;
}
