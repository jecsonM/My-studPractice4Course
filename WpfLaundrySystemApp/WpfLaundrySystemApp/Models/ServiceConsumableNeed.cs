using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Затраты расходных материалов на услугу")]
public partial class ServiceConsumableNeed
{
    [DisplayBehaviourAttribute("FK Идентификатор услуги", Visible = false)]
    public int ServiceId { get; set; }

    [DisplayBehaviourAttribute("Услуга", IsIncludeRequired = true)]
    public virtual Service Service { get; set; } = null!;

    [DisplayBehaviourAttribute("FK Идентификатор раходника", Visible = false)]
    public int ConsumableId { get; set; }

    [DisplayBehaviourAttribute("Расходник", IsIncludeRequired = true)]
    public virtual Consumable Consumable { get; set; } = null!;

    [DisplayBehaviourAttribute("Количество использования")]
    public float ConsumableUseAmount { get; set; }

}
