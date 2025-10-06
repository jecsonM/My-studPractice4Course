using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public int SupplierTypeId { get; set; }

    public string SupplierName { get; set; } = null!;

    public string Inn { get; set; } = null!;

    public virtual ICollection<ConsumableSuppliement> ConsumableSuppliements { get; set; } = new List<ConsumableSuppliement>();

    public virtual SupplierType SupplierType { get; set; } = null!;
}
