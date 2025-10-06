using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class ConsumableType
{
    public int ConsumableTypeId { get; set; }

    public string ConsumableTypeName { get; set; } = null!;

    public virtual ICollection<Consumable> Consumables { get; set; } = new List<Consumable>();
}
