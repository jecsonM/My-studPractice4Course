using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Движение расходных материалов")]
public partial class ConsumableMovementType
{
    [DisplayBehaviourAttribute("Идентификатор")]
    public int ConsumableMovementTypeId { get; set; }

    [DisplayBehaviourAttribute("Название вида")]
    public string ConsumableMovementTypeName { get; set; } = null!;

    [DisplayBehaviourAttribute("Движения с этим видом", IsSeeAllButtonRequired =true)]
    public virtual ICollection<ConsumableUse> ConsumableUses { get; set; } = new List<ConsumableUse>();

    public override string ToString()
    {
        return ConsumableMovementTypeName;
    }
}
