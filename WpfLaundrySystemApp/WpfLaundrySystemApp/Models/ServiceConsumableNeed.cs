using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class ServiceConsumableNeed
{
    public int ServiceId { get; set; }

    public int ConsumableId { get; set; }

    public float ConsumableUseAmount { get; set; }

    public virtual Consumable Consumable { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
