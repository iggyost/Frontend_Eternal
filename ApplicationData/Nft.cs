using System;
using System.Collections.Generic;

namespace Frontend_Eternal.ApplicationData;

public partial class Nft
{
    public int NftId { get; set; }

    public string CoverImage { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int CurrencyId { get; set; }

    public decimal CostCurrency { get; set; }

    public DateTime CreationDate { get; set; }

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<TagsNft> TagsNfts { get; set; } = new List<TagsNft>();

    public virtual ICollection<UsersNft> UsersNfts { get; set; } = new List<UsersNft>();
}
