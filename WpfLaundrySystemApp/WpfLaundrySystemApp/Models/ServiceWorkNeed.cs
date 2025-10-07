using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Затраты рабочей силы на услугу")]
public partial class ServiceWorkNeed
{
    [DisplayBehaviourAttribute("FK Идентификатор", Visible = false)]
    public int ServiceId { get; set; }

    [DisplayBehaviourAttribute("Услуга", IsIncludeRequired = true)]
    public virtual Service Service { get; set; } = null!;

    [DisplayBehaviourAttribute("FK Идентификатор", Visible = false)]
    public int PositionId { get; set; }

    [DisplayBehaviourAttribute("Должность", IsIncludeRequired = true)]
    public virtual Position Position { get; set; } = null!;

    public TimeSpan OveralWorkTime { get; set; }


}
