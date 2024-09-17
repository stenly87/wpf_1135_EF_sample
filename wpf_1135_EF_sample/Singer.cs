using System;
using System.Collections.Generic;

namespace wpf_1135_EF_sample;

public partial class Singer
{
    public int Id { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string? CreateHash { get; set; }

    public virtual ICollection<Music> Musics { get; set; } = new List<Music>();

    public virtual ICollection<YellowPress> YellowPresses { get; set; } = new List<YellowPress>();
}
