using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;

namespace WpfLaundrySystemApp.Models;

[DisplayClassNameAttribute("Рейтинги")]
public partial class Raiting
{
    [DisplayBehaviourAttribute("Идентификатор")]
    public int RaitingId { get; set; }

    [DisplayBehaviourAttribute("FK Идентификатор партнёра", Visible = false)]
    public int PartnerId { get; set; }

    [DisplayBehaviourAttribute("Партнёр", IsIncludeRequired =true)]
    public virtual Partner Partner { get; set; } = null!;

    [DisplayBehaviourAttribute("Рейтинг")]
    public float Raiting1 { get; set; }

    [DisplayBehaviourAttribute("Дата создания")]
    public DateTime CreatedAt { get; set; }

    public override string ToString()
    {
        return Raiting1.ToString();
    }

}
