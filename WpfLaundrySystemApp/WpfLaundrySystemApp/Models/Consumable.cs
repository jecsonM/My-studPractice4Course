using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class Consumable
{
    public int ConsumableId { get; set; }

    public int ConsumableTypeId { get; set; }

    public string ConsumableName { get; set; } = null!;

    public string ConsumableDescription { get; set; } = null!;

    public int UnitTypeId { get; set; }

    public float? AmountInOneUnit { get; set; }

    public byte[]? ConsumableImage { get; set; }

    public decimal? Cost { get; set; }

    public virtual ICollection<ConsumableSuppliement> ConsumableSuppliements { get; set; } = new List<ConsumableSuppliement>();

    public virtual ConsumableType ConsumableType { get; set; } = null!;

    public virtual ICollection<ConsumableUse> ConsumableUses { get; set; } = new List<ConsumableUse>();

    public virtual ICollection<ServiceConsumableNeed> ServiceConsumableNeeds { get; set; } = new List<ServiceConsumableNeed>();

    public virtual UnitType UnitType { get; set; } = null!;
}
