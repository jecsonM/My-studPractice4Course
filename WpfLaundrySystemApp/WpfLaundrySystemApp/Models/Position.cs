using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class Position
{
    public int PositionId { get; set; }

    public string PositionName { get; set; } = null!;

    public decimal AverageHourWage { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<ServiceWorkNeed> ServiceWorkNeeds { get; set; } = new List<ServiceWorkNeed>();
}
