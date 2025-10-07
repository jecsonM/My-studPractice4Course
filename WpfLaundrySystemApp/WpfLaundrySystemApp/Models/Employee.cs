using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Сотрудники")]
public partial class Employee
{
    [DisplayBehaviourAttribute("Идентификатор")]
    public int EmployeeId { get; set; }

    [DisplayBehaviourAttribute("ФИО")]
    public string FullName { get; set; } = null!;

    [DisplayBehaviourAttribute("Должность", IsIncludeRequired =true)]
    public virtual Position Position { get; set; } = null!;

    [DisplayBehaviourAttribute("FK Идентификатор должности", Visible = false)]
    public int PositionId { get; set; }

    [DisplayBehaviourAttribute("Дата рождения")]
    public DateOnly Birthday { get; set; }

    [DisplayBehaviourAttribute("Серия паспорта")]
    public string PassportSeries { get; set; } = null!;

    [DisplayBehaviourAttribute("Номер паспорта")]
    public string PassportNumber { get; set; } = null!;

    [DisplayBehaviourAttribute("кто выдал")]
    public string WhoIssuedPassport { get; set; } = null!;

    [DisplayBehaviourAttribute("Дата выдачи")]
    public DateOnly DateOfPassportIssue { get; set; }

    [DisplayBehaviourAttribute("Состояния здоровья работника", IsSeeAllButtonRequired = true)]
    public virtual ICollection<EmployeeHealthState> EmployeeHealthStates { get; set; } = new List<EmployeeHealthState>();

    [DisplayBehaviourAttribute("Движения работника", IsSeeAllButtonRequired = true)]
    public virtual ICollection<EmployeeMovement> EmployeeMovements { get; set; } = new List<EmployeeMovement>();

    public override string ToString()
    {
        return FullName;
    }

}
