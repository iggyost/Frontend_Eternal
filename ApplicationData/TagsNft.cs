using System;
using System.Collections.Generic;

namespace Frontend_Eternal.ApplicationData;

public partial class TagsNft
{
    public int TagNftId { get; set; }

    public int NftId { get; set; }

    public int TagId { get; set; }

    public virtual Nft Nft { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}
