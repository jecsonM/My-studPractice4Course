using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class UnitType
{
    public int UnitTypeId { get; set; }

    public string UnitTypeName { get; set; } = null!;

    public virtual ICollection<Consumable> Consumables { get; set; } = new List<Consumable>();
}
