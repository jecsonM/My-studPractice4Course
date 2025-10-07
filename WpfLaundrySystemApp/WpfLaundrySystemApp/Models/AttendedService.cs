using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;
[DisplayClassNameAttribute("Оказанные услуги")]
public partial class AttendedService
{
    [DisplayBehaviourAttribute("Идентификатор заказа", Visible =false)]
    public int OrderId { get; set; }

    [DisplayBehaviourAttribute("Идентификатор услуги", Visible = false)]
    public int ServiceId { get; set; }

    [DisplayBehaviourAttribute("Идентификатор прайс-листа", Visible = false)]
    public int PriceListId { get; set; }

    [DisplayBehaviourAttribute("Ожидаемая дата завершения заказа")]
    public DateTime AwaitedDateOfAttendance { get; set; }

    [DisplayBehaviourAttribute("Дата создания")]
    public DateTime DateOfCreation { get; set; }

    [DisplayBehaviourAttribute("Идентификатор цеха", Visible = false)]
    public int WorkshopId { get; set; }

    [DisplayBehaviourAttribute("Идентификатор использования расходных материалов", Visible = false)]
    public int ConsumableUseId { get; set; }


    [DisplayBehaviourAttribute("Использование расходных материалов", IsIncludeRequired = true)]
    public virtual ConsumableUse ConsumableUse { get; set; } = null!;

    [DisplayBehaviourAttribute("Номер заказа", IsIncludeRequired = true)]
    public virtual Order Order { get; set; } = null!;

    [DisplayBehaviourAttribute("Прайс-лист", IsIncludeRequired = true)]
    public virtual PriceList PriceList { get; set; } = null!;


    [DisplayBehaviourAttribute("Услуга", IsIncludeRequired = true)]
    public virtual Service Service { get; set; } = null!;


    [DisplayBehaviourAttribute("Цех", IsIncludeRequired = true)]
    public virtual Workshop Workshop { get; set; } = null!;
}
