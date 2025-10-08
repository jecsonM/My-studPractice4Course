using System;
using System.Collections.Generic;
using WpfLaundrySystemApp.Attributes;
using WpfLaundrySystemApp.DBContext;

namespace WpfLaundrySystemApp.Models;
[DisplayClassNameAttribute("Партнёры")]
public partial class Partner
{
    [DisplayBehaviourAttribute("Идентификатор")]
    public int PartnerId { get; set; }

    [DisplayBehaviourAttribute("Вид партнёра", IsIncludeRequired = true)]
    public virtual PartnerType PartnerType { get; set; } = null!;

    [DisplayBehaviourAttribute("FK Вид партнёра", Visible =false)]
    public int PartnerTypeId { get; set; }

    [DisplayBehaviourAttribute("Имя")]
    public string PartnerName { get; set; } = null!;

    [DisplayBehaviourAttribute("Юр. адрес")]
    public string LegalAddress { get; set; } = null!;

    [DisplayBehaviourAttribute("Эл. почта")]
    public string? Email { get; set; }

    [DisplayBehaviourAttribute("Номер телефона")]
    public string? Phone { get; set; }

    [DisplayBehaviourAttribute("Логотип")]
    public byte[]? Logo { get; set; }

    [DisplayBehaviourAttribute("ИНН")]
    public string Inn { get; set; } = null!;

    [DisplayBehaviourAttribute("Последний рейтинг", Visible = true)]
    public Raiting LastRaiting {
        get
        {
            
            if (_lastRaiting == null)
                using (LaundryDbContext dbContext = new LaundryDbContext())
                {
                    _lastRaiting = dbContext.Raitings.Where(r=>r.PartnerId == PartnerId).ToList().Max(new RaitingDateCreationComparer());
                    if (_lastRaiting is null)
                        _lastRaiting = new Raiting() { Raiting1 = 0};
                }
            return _lastRaiting;
        }
    }

    [DisplayBehaviourAttribute("Филиалы", IsSeeAllButtonRequired = true)]
    public virtual ICollection<BranchPoint> BranchPoints { get; set; } = new List<BranchPoint>();

    [DisplayBehaviourAttribute("Заказы", IsSeeAllButtonRequired = true)]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [DisplayBehaviourAttribute("Рейтинги", IsSeeAllButtonRequired = true)]
    public virtual ICollection<Raiting> Raitings { get; set; } = new List<Raiting>();

    public class RaitingDateCreationComparer : IComparer<Raiting>
    {
        public int Compare(Raiting? x, Raiting? y)
        {
            return x.CreatedAt.CompareTo(y.CreatedAt);
        }
    }

    [DisplayBehaviourAttribute("Последний рейтинг", Visible = false)]
    private Raiting _lastRaiting { get; set; }

    public override string ToString()
    {
        return PartnerName;
    }
}
