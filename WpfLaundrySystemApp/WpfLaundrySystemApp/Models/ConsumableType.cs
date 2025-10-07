using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Вид расходного материала")]
public partial class ConsumableType
{
    public int ConsumableTypeId { get; set; }

    public string ConsumableTypeName { get; set; } = null!;

    public virtual ICollection<Consumable> Consumables { get; set; } = new List<Consumable>();

    public override string ToString()
    {
        return ConsumableTypeName;
    }
}
