using System;
using System.Collections.Generic;

namespace wpf_1135_EF_sample;

public partial class YellowPress
{
    public int Id { get; set; }

    public string? TitleArticle { get; set; }

    public string? Description { get; set; }

    public int? IdSinger { get; set; }

    public string? CreateHash { get; set; }

    public virtual Singer? IdSingerNavigation { get; set; }
}
