using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Состояния здоровья")]
public partial class HealthState
{
    [DisplayBehaviourAttribute("Идентификатор")]
    public int HealthStateId { get; set; }

    [DisplayBehaviourAttribute("Название")]
    public string HealthStateName { get; set; } = null!;

    [DisplayBehaviourAttribute("Описание")]
    public string HealthStateDescription { get; set; } = null!;

    [DisplayBehaviourAttribute("Работники с этим состоянием", IsSeeAllButtonRequired =true)]
    public virtual ICollection<EmployeeHealthState> EmployeeHealthStates { get; set; } = new List<EmployeeHealthState>();

    public override string ToString()
    {
        return HealthStateName;
    }
}
