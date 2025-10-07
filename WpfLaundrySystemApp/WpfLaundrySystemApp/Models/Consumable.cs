using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Расходные материалы")]
public partial class Consumable
{
    [DisplayBehaviourAttribute("Идентификатор")]
    public int ConsumableId { get; set; }

    [DisplayBehaviourAttribute("FK Идентификатор вида расходного материала", Visible =false)]
    public int ConsumableTypeId { get; set; }
    [DisplayBehaviourAttribute("Название")]
    public string ConsumableName { get; set; } = null!;

    [DisplayBehaviourAttribute("Описание")]
    public string ConsumableDescription { get; set; } = null!;

    [DisplayBehaviourAttribute("FK Идентификатор единицы измерения", Visible = false)]
    public int UnitTypeId { get; set; }

    [DisplayBehaviourAttribute("Вид", IsIncludeRequired = true)]
    public virtual ConsumableType ConsumableType { get; set; } = null!;

    [DisplayBehaviourAttribute("Количество в одной упаковке")]
    public float? AmountInOneUnit { get; set; }

    [DisplayBehaviourAttribute("Единица измерения", IsIncludeRequired = true)]
    public virtual UnitType UnitType { get; set; } = null!;

    [DisplayBehaviourAttribute("Изображения")]
    public byte[]? ConsumableImage { get; set; }

    [DisplayBehaviourAttribute("Цена за одну упаковку")]
    public decimal? Cost { get; set; }

    [DisplayBehaviourAttribute("Поставщики", IsSeeAllButtonRequired = true)]
    public virtual ICollection<ConsumableSuppliement> ConsumableSuppliements { get; set; } = new List<ConsumableSuppliement>();

    [DisplayBehaviourAttribute("Использования", IsSeeAllButtonRequired = true)]
    public virtual ICollection<ConsumableUse> ConsumableUses { get; set; } = new List<ConsumableUse>();

    [DisplayBehaviourAttribute("Для каких услуг", IsSeeAllButtonRequired = true)]
    public virtual ICollection<ServiceConsumableNeed> ServiceConsumableNeeds { get; set; } = new List<ServiceConsumableNeed>();


    public override string ToString()
    {
        return ConsumableName;
    }
}
