using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class Partner
{
    public int PartnerId { get; set; }

    public int PartnerTypeId { get; set; }

    public string PartnerName { get; set; } = null!;

    public string LegalAddress { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public byte[]? Logo { get; set; }

    public string Inn { get; set; } = null!;

    public virtual ICollection<BranchPoint> BranchPoints { get; set; } = new List<BranchPoint>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual PartnerType PartnerType { get; set; } = null!;

    public virtual ICollection<Raiting> Raitings { get; set; } = new List<Raiting>();
}
