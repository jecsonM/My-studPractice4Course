using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Поставщики")]
public partial class Supplier
{
    [DisplayBehaviourAttribute("Идентификатор")]
    public int SupplierId { get; set; }

    [DisplayBehaviourAttribute("FK Идентификатор", Visible = false)]
    public int SupplierTypeId { get; set; }

    [DisplayBehaviourAttribute("Вид поставщика", IsIncludeRequired = true)]
    public virtual SupplierType SupplierType { get; set; } = null!;

    [DisplayBehaviourAttribute("Имя")]
    public string SupplierName { get; set; } = null!;

    [DisplayBehaviourAttribute("ИНН")]
    public string Inn { get; set; } = null!;

    [DisplayBehaviourAttribute("Поставленные расходники", IsSeeAllButtonRequired = true)]
    public virtual ICollection<ConsumableSuppliement> ConsumableSuppliements { get; set; } = new List<ConsumableSuppliement>();

    public override string ToString()
    {
        return SupplierName;
    }
}
