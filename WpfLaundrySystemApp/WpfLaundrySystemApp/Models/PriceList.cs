using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class PriceList
{
    public int PriceListId { get; set; }

    public int ServiceId { get; set; }

    public decimal Price { get; set; }

    public DateTime DateOfCreation { get; set; }

    public virtual ICollection<AttendedService> AttendedServices { get; set; } = new List<AttendedService>();

    public virtual Service Service { get; set; } = null!;
}
