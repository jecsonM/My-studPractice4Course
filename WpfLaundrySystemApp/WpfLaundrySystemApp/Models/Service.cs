using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Услуги")]
public partial class Service
{
    [DisplayBehaviourAttribute("Идентификатор")]
    public int ServiceId { get; set; }

    [DisplayBehaviourAttribute("FK Идентификатор вида услуги", Visible = false)]
    public int ServiceTypeId { get; set; }

    [DisplayBehaviourAttribute("Вид услуги", IsIncludeRequired = true)]
    public virtual ServiceType ServiceType { get; set; } = null!;

    [DisplayBehaviourAttribute("Название")]
    public string ServiceName { get; set; } = null!;

    [DisplayBehaviourAttribute("Описание")]
    public string ServiceDescription { get; set; } = null!;

    [DisplayBehaviourAttribute("Изображение")]
    public byte[]? ServiceImage { get; set; }

    [DisplayBehaviourAttribute("Минимальная цена")]
    public decimal MinimalCost { get; set; }

    [DisplayBehaviourAttribute("Время исполнения")]
    public TimeSpan ExecutionTime { get; set; }

    [DisplayBehaviourAttribute("Цена")]
    public decimal? Cost { get; set; }

    [DisplayBehaviourAttribute("Оказанные услуги", IsSeeAllButtonRequired = true)]
    public virtual ICollection<AttendedService> AttendedServices { get; set; } = new List<AttendedService>();

    [DisplayBehaviourAttribute("Цены (прайс-листы)", IsSeeAllButtonRequired = true)]
    public virtual ICollection<PriceList> PriceLists { get; set; } = new List<PriceList>();

    [DisplayBehaviourAttribute("Затраты расходников", IsSeeAllButtonRequired = true)]
    public virtual ICollection<ServiceConsumableNeed> ServiceConsumableNeeds { get; set; } = new List<ServiceConsumableNeed>();

    [DisplayBehaviourAttribute("Затраты рабочей силы", IsSeeAllButtonRequired = true)]
    public virtual ICollection<ServiceWorkNeed> ServiceWorkNeeds { get; set; } = new List<ServiceWorkNeed>();


    public override string ToString()
    {
        return ServiceName;
    }
}
