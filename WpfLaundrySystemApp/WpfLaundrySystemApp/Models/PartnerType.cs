using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class PartnerType
{
    public int PartnerTypeId { get; set; }

    public string PartnerTypeName { get; set; } = null!;

    public virtual ICollection<Partner> Partners { get; set; } = new List<Partner>();
}
