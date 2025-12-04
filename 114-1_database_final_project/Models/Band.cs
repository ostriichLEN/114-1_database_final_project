using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace _114_1_database_final_project.Models;

public partial class Band
{
    [DisplayName("樂團編號")]
    public int BandId { get; set; }

    [DisplayName("樂團名稱")]
    public string BandName { get; set; } = null!;

    [DisplayName("樂團創立年分")]
    public int? SinceYear { get; set; }

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();

    
}
