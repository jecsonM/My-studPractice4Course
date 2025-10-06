using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = null!;

    public int PositionId { get; set; }

    public DateOnly Birthday { get; set; }

    public string PassportSeries { get; set; } = null!;

    public string PassportNumber { get; set; } = null!;

    public string WhoIssuedPassport { get; set; } = null!;

    public DateOnly DateOfPassportIssue { get; set; }

    public virtual ICollection<EmployeeHealthState> EmployeeHealthStates { get; set; } = new List<EmployeeHealthState>();

    public virtual ICollection<EmployeeMovement> EmployeeMovements { get; set; } = new List<EmployeeMovement>();

    public virtual Position Position { get; set; } = null!;
}
