using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Виды партнёров")]
public partial class PartnerType
{
    [DisplayBehaviourAttribute("Идентификатор вида партнёра")]
    public int PartnerTypeId { get; set; }

    [DisplayBehaviourAttribute("Вид партнёра")]
    public string PartnerTypeName { get; set; } = null!;

    [DisplayBehaviourAttribute("Партнёры",Visible =false)]
    public virtual ICollection<Partner> Partners { get; set; } = new List<Partner>();

    public override string ToString()
    {
        return PartnerTypeName;
    }
}
