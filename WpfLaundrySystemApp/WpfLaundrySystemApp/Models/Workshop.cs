using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Цеха")]
public partial class Workshop
{
    
    [DisplayBehaviourAttribute("Идентификатор")]
    public int WorkshopId { get; set; }

    [DisplayBehaviourAttribute("Название")]
    public string WorkShopName { get; set; } = null!;

    //[HiddenColumn]
    [DisplayBehaviourAttribute("Оказываемые услуги", Visible = false)]
    public virtual ICollection<AttendedService> AttendedServices { get; set; } = new List<AttendedService>();

    //[HiddenColumn]
    [DisplayBehaviourAttribute("Движения работников", Visible =false)]
    public virtual ICollection<EmployeeMovement> EmployeeMovements { get; set; } = new List<EmployeeMovement>();
}
