using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Должности")]
public partial class Position
{
    [DisplayBehaviourAttribute("Идентификатор")]
    public int PositionId { get; set; }

    [DisplayBehaviourAttribute("Название")]
    public string PositionName { get; set; } = null!;

    [DisplayBehaviourAttribute("Часовая ставка")]
    public decimal AverageHourWage { get; set; }

    [DisplayBehaviourAttribute("Сотрудники с этой должностью", IsSeeAllButtonRequired = true)]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    [DisplayBehaviourAttribute("В каких услугах нужен сотрудник этой должности", IsSeeAllButtonRequired = true)]
    public virtual ICollection<ServiceWorkNeed> ServiceWorkNeeds { get; set; } = new List<ServiceWorkNeed>();

    public override string ToString()
    {
        return PositionName;
    }
}
