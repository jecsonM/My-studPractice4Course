using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class ConsumableMovementType
{
    public int ConsumableMovementTypeId { get; set; }

    public string ConsumableMovementTypeName { get; set; } = null!;

    public virtual ICollection<ConsumableUse> ConsumableUses { get; set; } = new List<ConsumableUse>();
}
