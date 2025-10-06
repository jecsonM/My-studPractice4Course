using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class BranchPoint
{
    public int BranchPointId { get; set; }

    public int PartnerId { get; set; }

    public string Address { get; set; } = null!;

    public virtual Partner Partner { get; set; } = null!;
}
