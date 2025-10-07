using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;
using WpfLaundrySystemApp.DBContext;

namespace WpfLaundrySystemApp.Models;


[DisplayClassNameAttribute("Пользования расходных материалов")]
public partial class ConsumableUse
{
    [DisplayBehaviourAttribute("Идентификатор")]
    public int ConsumableUseId { get; set; }

    [DisplayBehaviourAttribute("FK Идентификатор расходного материала", Visible = false)]
    public int ConsumableId { get; set; }

    [DisplayBehaviourAttribute("FK Идентификатор вида пользования расходного материала", Visible = false)]
    public int ConsumableMovementTypeId { get; set; }

    [DisplayBehaviourAttribute("Количество")]
    public int Amount { get; set; }

    [DisplayBehaviourAttribute("Дата создания")]
    public DateTime DateOfCreation { get; set; }

    [DisplayBehaviourAttribute("Участвовал в оказаниях", IsSeeAllButtonRequired =true)]
    public virtual ICollection<AttendedService> AttendedServices { get; set; } = new List<AttendedService>();

    [DisplayBehaviourAttribute("Расходный материал", IsIncludeRequired = true)]
    public virtual Consumable Consumable { get; set; } = null!;

    [DisplayBehaviourAttribute("Вид использования", IsIncludeRequired = true)]
    public virtual ConsumableMovementType ConsumableMovementType { get; set; } = null!;

    public override string ToString()
    {
        if(Consumable == null || ConsumableMovementType == null)
        using (LaundryDbContext dbContext = new LaundryDbContext())
        {
                Consumable = dbContext.Consumables.Find(ConsumableId);
                ConsumableMovementType = dbContext.ConsumableMovementTypes.Find(ConsumableMovementTypeId);
            }
        return $"{ConsumableMovementType.ToString()} {Consumable.ToString()}";
    }
}
