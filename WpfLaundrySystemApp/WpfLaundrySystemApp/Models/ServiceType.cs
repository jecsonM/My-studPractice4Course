using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Вид услуги")]
public partial class ServiceType
{
    [DisplayBehaviourAttribute("Идентификатор")]
    public int ServiceTypeId { get; set; }

    [DisplayBehaviourAttribute("Название")]
    public string ServiceTypeName { get; set; } = null!;

    [DisplayBehaviourAttribute("Услуги этого вида", IsSeeAllButtonRequired = true)]
    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public override string ToString()
    {
        return ServiceTypeName;
    }
}
