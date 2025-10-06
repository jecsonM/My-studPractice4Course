using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class ConsumableSuppliement
{
    public int ConsumableSuppliementId { get; set; }

    public int ConsumableId { get; set; }

    public int SupplierId { get; set; }

    public int Amount { get; set; }

    public DateTime DateOfCreation { get; set; }

    public virtual Consumable Consumable { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;
}
