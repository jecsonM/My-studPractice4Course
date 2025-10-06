using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class SupplierType
{
    public int SupplierTypeId { get; set; }

    public string SupplierTypeName { get; set; } = null!;

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}
