using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Единицы измерения")]
public partial class UnitType
{
    public int UnitTypeId { get; set; }

    public string UnitTypeName { get; set; } = null!;

    public virtual ICollection<Consumable> Consumables { get; set; } = new List<Consumable>();

    public override string ToString()
    {
        return UnitTypeName;
    }
}
