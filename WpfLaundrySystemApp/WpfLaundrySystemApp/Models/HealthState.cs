using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class HealthState
{
    public int HealthStateId { get; set; }

    public string HealthStateName { get; set; } = null!;

    public string HealthStateDescription { get; set; } = null!;

    public virtual ICollection<EmployeeHealthState> EmployeeHealthStates { get; set; } = new List<EmployeeHealthState>();
}
