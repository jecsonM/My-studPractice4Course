using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class ConsumableUse
{
    public int ConsumableUseId { get; set; }

    public int ConsumableId { get; set; }

    public int ConsumableMovementTypeId { get; set; }

    public int Amount { get; set; }

    public DateTime DateOfCreation { get; set; }

    public virtual ICollection<AttendedService> AttendedServices { get; set; } = new List<AttendedService>();

    public virtual Consumable Consumable { get; set; } = null!;

    public virtual ConsumableMovementType ConsumableMovementType { get; set; } = null!;
}
