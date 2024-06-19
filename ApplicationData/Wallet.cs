using System;
using System.Collections.Generic;

namespace Frontend_Eternal.ApplicationData;

public partial class Wallet
{
    public int WalletId { get; set; }

    public int UserId { get; set; }

    public decimal Balance { get; set; }

    public int CurrencyId { get; set; }

    public virtual Currency Currency { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
