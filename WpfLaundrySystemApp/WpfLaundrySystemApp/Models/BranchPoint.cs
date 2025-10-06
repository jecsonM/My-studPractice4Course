using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Филиалы")]
public partial class BranchPoint
{
    [DisplayBehaviourAttribute("Идентификатор")]
    public int BranchPointId { get; set; }

    [DisplayBehaviourAttribute("Владелец", IsIncludeRequired =true)]
    public virtual Partner Partner { get; set; } = null!;

    [DisplayBehaviourAttribute("FK Идентификатор партнёра", Visible = false)]
    public int PartnerId { get; set; }

    [DisplayBehaviourAttribute("Адрес")]
    public string Address { get; set; } = null!;

    public override string ToString()
    {
        return Address;
    }
}
