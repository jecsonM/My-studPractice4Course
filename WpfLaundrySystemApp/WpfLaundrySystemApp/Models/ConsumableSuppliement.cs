using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Поступления расходных материалов")]
public partial class ConsumableSuppliement
{
    [DisplayBehaviourAttribute("Идентификатор")]
    public int ConsumableSuppliementId { get; set; }

    [DisplayBehaviourAttribute("FK Идентификатор расходного материала", Visible = false)]
    public int ConsumableId { get; set; }

    [DisplayBehaviourAttribute("FK Идентификатор поставщика", Visible = false)]
    public int SupplierId { get; set; }

    [DisplayBehaviourAttribute("Количество")]
    public int Amount { get; set; }

    [DisplayBehaviourAttribute("Дата создания")]
    public DateTime DateOfCreation { get; set; }

    [DisplayBehaviourAttribute("Расходный материал", IsIncludeRequired =true)]
    public virtual Consumable Consumable { get; set; } = null!;

    [DisplayBehaviourAttribute("Поставщик", IsIncludeRequired = true)]
    public virtual Supplier Supplier { get; set; } = null!;
}
