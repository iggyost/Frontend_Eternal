using System;
using System.Collections.Generic;

namespace Frontend_Eternal.ApplicationData;

public partial class UsersNft
{
    public int UserNftId { get; set; }

    public int NftId { get; set; }

    public int UserId { get; set; }

    public virtual Nft Nft { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
