using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Отметки о здоровье сотрудников")]
public partial class EmployeeHealthState
{
    [DisplayBehaviourAttribute("FK Идентификатор сотрудника", Visible =false)]
    public int EmployeeId { get; set; }

    [DisplayBehaviourAttribute("Сотрудник", IsIncludeRequired = true)]
    public virtual Employee Employee { get; set; } = null!;

    [DisplayBehaviourAttribute("FK Идентификатор состояния здоровья", Visible = false)]
    public int HealthStateId { get; set; }

    [DisplayBehaviourAttribute("Состояние здоровья", IsIncludeRequired = true)]
    public virtual HealthState HealthState { get; set; } = null!;

    [DisplayBehaviourAttribute("Дата создания")]
    public DateTime? DateOfCreation { get; set; }

}