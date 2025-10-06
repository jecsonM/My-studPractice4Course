using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public int ServiceTypeId { get; set; }

    public string ServiceName { get; set; } = null!;

    public string ServiceDescription { get; set; } = null!;

    public byte[]? ServiceImage { get; set; }

    public decimal MinimalCost { get; set; }

    public TimeSpan ExecutionTime { get; set; }

    public decimal? Cost { get; set; }

    public virtual ICollection<AttendedService> AttendedServices { get; set; } = new List<AttendedService>();

    public virtual ICollection<PriceList> PriceLists { get; set; } = new List<PriceList>();

    public virtual ICollection<ServiceConsumableNeed> ServiceConsumableNeeds { get; set; } = new List<ServiceConsumableNeed>();

    public virtual ServiceType ServiceType { get; set; } = null!;

    public virtual ICollection<ServiceWorkNeed> ServiceWorkNeeds { get; set; } = new List<ServiceWorkNeed>();
}
