using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Движение работников")]
public partial class EmployeeMovement
{
    [DisplayBehaviourAttribute("FK Идентификатор цеха", Visible = false)]
    public int WorkshopId { get; set; }

    [DisplayBehaviourAttribute("Цех", IsIncludeRequired =true)]
    public virtual Workshop Workshop { get; set; } = null!;

    [DisplayBehaviourAttribute("FK Идентификатор сотрдника", Visible = false)]
    public int EmployeeId { get; set; }

    [DisplayBehaviourAttribute("Работник", IsIncludeRequired = true)]
    public virtual Employee Employee { get; set; } = null!;

    [DisplayBehaviourAttribute("Дата")]
    public DateTime? DateOfCreation { get; set; }


}
