using System;
using System.Collections.Generic;

namespace Frontend_Eternal.ApplicationData;

public partial class Currency
{
    public int CurrencyId { get; set; }

    public string Name { get; set; } = null!;

    public string NameShort { get; set; } = null!;

    public decimal PriceRub { get; set; }

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}
