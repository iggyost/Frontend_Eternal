using System;
using System.Collections.Generic;

namespace Frontend_Eternal.ApplicationData;

public partial class FavoritesView
{
    public int FavoriteId { get; set; }

    public int UserId { get; set; }

    public int NftId { get; set; }

    public string CoverImage { get; set; } = null!;
}
