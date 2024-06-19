using System;
using System.Collections.Generic;

namespace Frontend_Eternal.ApplicationData;

public partial class NftView
{
    public int NftId { get; set; }

    public string CoverImage { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int CurrencyId { get; set; }

    public string Currency { get; set; } = null!;

    public string CurrencyShort { get; set; } = null!;

    public decimal PriceRub { get; set; }

    public decimal CostCurrency { get; set; }

    public string Owner { get; set; } = null!;

    public int UserId { get; set; }

    public DateTime CreationDate { get; set; }
}
