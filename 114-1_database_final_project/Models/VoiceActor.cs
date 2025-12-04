using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace _114_1_database_final_project.Models;

public partial class VoiceActor
{
    [DisplayName("藝人編號")]
    public int VoiceActorId { get; set; }

    [DisplayName("藝人姓氏")]
    public string FirstName { get; set; } = null!;

    [DisplayName("藝人名稱")]
    public string LastName { get; set; } = null!;

    [DisplayName("藝人生日")]
    public DateOnly? BirthDate { get; set; }

    [DisplayName("所屬國家")]
    public string? Subsidiary { get; set; }

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();

}
