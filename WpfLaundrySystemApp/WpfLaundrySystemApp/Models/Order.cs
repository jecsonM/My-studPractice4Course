using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int PartnerId { get; set; }

    public DateTime AwaitedDateOfAttendance { get; set; }

    public DateTime? DateOfAttendance { get; set; }

    public DateTime? DateOfPaycheck { get; set; }

    public DateTime DateOfCreation { get; set; }

    public bool? IsCancelled { get; set; }

    public virtual ICollection<AttendedService> AttendedServices { get; set; } = new List<AttendedService>();

    public virtual Partner Partner { get; set; } = null!;
}
