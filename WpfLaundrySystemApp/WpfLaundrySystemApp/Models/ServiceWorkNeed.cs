using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class ServiceWorkNeed
{
    public int ServiceId { get; set; }

    public int PositionId { get; set; }

    public TimeSpan OveralWorkTime { get; set; }

    public virtual Position Position { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
