using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Виды поставщиков")]
public partial class SupplierType
{
    [DisplayBehaviourAttribute("Идентификатор")]
    public int SupplierTypeId { get; set; }

    [DisplayBehaviourAttribute("Название")]
    public string SupplierTypeName { get; set; } = null!;

    [DisplayBehaviourAttribute("Поставщики этого вида", IsSeeAllButtonRequired = true)]
    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();

    public override string ToString()
    {
        return SupplierTypeName;
    }
}
