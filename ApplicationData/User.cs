using System;
using System.Collections.Generic;

namespace Frontend_Eternal.ApplicationData;

public partial class User
{
    public int UserId { get; set; }

    public string Phone { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? CoverImage { get; set; }

    public string Name { get; set; } = null!;

    public string TagName { get; set; } = null!;

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<UsersNft> UsersNfts { get; set; } = new List<UsersNft>();

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}
