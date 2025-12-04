using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace _114_1_database_final_project.Models;

public partial class Character
{
    [DisplayName("角色編號")]
    public int CharacterId { get; set; }
    [DisplayName("姓氏")]
    public string FirstName { get; set; } = null!;
    [DisplayName("名稱")]
    public string LastName { get; set; } = null!;
    [DisplayName("設定生日")]
    public DateOnly? Birthdate { get; set; }
    [DisplayName("設定身高")]
    public int? Height { get; set; }
    [DisplayName("對應藝人名稱")]
    public int? VoiceActorId { get; set; }
    [DisplayName("樂團編號")]
    public int? BandId { get; set; }
    [DisplayName("擔任樂團位置")]
    public string? BandPosition { get; set; }

    [DisplayName("所屬樂團編號")]
    public virtual Band? Band { get; set; }

    [DisplayName("所屬藝人編號")]
    public virtual VoiceActor? VoiceActor { get; set; }

    
}
