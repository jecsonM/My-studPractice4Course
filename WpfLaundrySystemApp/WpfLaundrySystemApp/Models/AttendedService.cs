using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class AttendedService
{
    public int OrderId { get; set; }

    public int ServiceId { get; set; }

    public int PriceListId { get; set; }

    public DateTime AwaitedDateOfAttendance { get; set; }

    public DateTime DateOfCreation { get; set; }

    public int WorkshopId { get; set; }

    public int ConsumableUseId { get; set; }

    public virtual ConsumableUse ConsumableUse { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual PriceList PriceList { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;

    public virtual Workshop Workshop { get; set; } = null!;
}
