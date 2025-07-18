﻿using System;
using System.Collections.Generic;

namespace WebAppDataBaseCars.Models;

public partial class Owner
{
    public int Id { get; set; }

    public string Number { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? SecondName { get; set; }

    public string? Surname { get; set; }

    public virtual Car NumberNavigation { get; set; } = null!;
}
