using System;
using System.Collections.Generic;

namespace _114_1_database_final_project.Models;

public partial class Relation
{
    public int Id1 { get; set; }

    public int Id2 { get; set; }

    public string Relationship { get; set; } = null!;
}
