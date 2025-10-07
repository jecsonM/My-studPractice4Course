using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Прайс-листы")]
public partial class PriceList
{
    [DisplayBehaviourAttribute("Идентификатор")]
    public int PriceListId { get; set; }

    [DisplayBehaviourAttribute("FK Идентификатор услуги", Visible = false)]
    public int ServiceId { get; set; }

    [DisplayBehaviourAttribute("Услуга", IsIncludeRequired = true)]
    public virtual Service Service { get; set; } = null!;

    [DisplayBehaviourAttribute("Цена")]
    public decimal Price { get; set; }

    [DisplayBehaviourAttribute("Дата создания")]
    public DateTime DateOfCreation { get; set; }

    [DisplayBehaviourAttribute("Оказанные услуги по этому прайс-листу", IsSeeAllButtonRequired = true)]
    public virtual ICollection<AttendedService> AttendedServices { get; set; } = new List<AttendedService>();

    public override string ToString()
    {
        return $"{Price:f2} ₽";
    }
}
