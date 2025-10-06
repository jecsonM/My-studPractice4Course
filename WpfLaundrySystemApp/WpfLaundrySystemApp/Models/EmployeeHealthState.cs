using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class EmployeeHealthState
{
    public int EmployeeId { get; set; }

    public int HealthStateId { get; set; }

    public DateTime? DateOfCreation { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual HealthState HealthState { get; set; } = null!;
}
