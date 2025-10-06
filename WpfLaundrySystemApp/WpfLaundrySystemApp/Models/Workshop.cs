using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

public partial class Workshop
{
    
    [DisplayNameAttribute("Идентификатор цеха")]
    public int WorkshopId { get; set; }

    [DisplayNameAttribute("Название цеха")]
    public string WorkShopName { get; set; } = null!;

    //[HiddenColumn]
    [DisplayNameAttribute("Оказываемые услуги", Visible = false)]
    public virtual ICollection<AttendedService> AttendedServices { get; set; } = new List<AttendedService>();

    //[HiddenColumn]
    [DisplayNameAttribute("Движения работников", Visible =false)]
    public virtual ICollection<EmployeeMovement> EmployeeMovements { get; set; } = new List<EmployeeMovement>();
}
