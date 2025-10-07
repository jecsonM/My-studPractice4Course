using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Заказы")]
public partial class Order
{
    [DisplayBehaviourAttribute("Идентификатор")]
    public int OrderId { get; set; }

    [DisplayBehaviourAttribute("FK Идентификатор партнёра", Visible =false)]
    public int PartnerId { get; set; }

    [DisplayBehaviourAttribute("Расчитываемая дата оказания услуг")]
    public DateTime AwaitedDateOfAttendance { get; set; }

    [DisplayBehaviourAttribute("Дата оказания услуги")]
    public DateTime? DateOfAttendance { get; set; }

    [DisplayBehaviourAttribute("Дата оплаты")]
    public DateTime? DateOfPaycheck { get; set; }

    [DisplayBehaviourAttribute("Дата создания")]
    public DateTime DateOfCreation { get; set; }

    [DisplayBehaviourAttribute("Отменён?")]
    public bool? IsCancelled { get; set; }

    [DisplayBehaviourAttribute("Оказанные услуги", IsSeeAllButtonRequired =true)]
    public virtual ICollection<AttendedService> AttendedServices { get; set; } = new List<AttendedService>();


    [DisplayBehaviourAttribute("Партнёр", IsIncludeRequired =true)]
    public virtual Partner Partner { get; set; } = null!;

    public override string ToString()
    {
        return OrderId.ToString();
    }
}
