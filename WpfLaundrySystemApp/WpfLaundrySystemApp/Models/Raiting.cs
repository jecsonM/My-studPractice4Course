using System;
using System.Collections.Generic;

namespace WpfLaundrySystemApp.Models;

public partial class Raiting
{
    public int RaitingId { get; set; }

    public int PartnerId { get; set; }

    public float Raiting1 { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Partner Partner { get; set; } = null!;
}
