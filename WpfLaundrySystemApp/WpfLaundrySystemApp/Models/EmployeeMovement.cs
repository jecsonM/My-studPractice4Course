using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class EmployeeMovement
{
    public int WorkshopId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime? DateOfCreation { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Workshop Workshop { get; set; } = null!;
}
