using System;
using System.Collections.Generic;

namespace wpf_1135_EF_sample;

public partial class Music
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int IdSinger { get; set; }

    public string? CreateHash { get; set; }

    public virtual Singer IdSingerNavigation { get; set; } = null!;
}
