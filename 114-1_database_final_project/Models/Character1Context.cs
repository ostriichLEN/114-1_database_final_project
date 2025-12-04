using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace _114_1_database_final_project.Models;

public partial class Character1Context : DbContext
{
    public Character1Context()
    {
    }

    public Character1Context(DbContextOptions<Character1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Band> Bands { get; set; }

    public virtual DbSet<Character> Characters { get; set; }

    public virtual DbSet<Relation> Relations { get; set; }

    public virtual DbSet<VoiceActor> VoiceActors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=character_1;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Band>(entity =>
        {
            entity.HasKey(e => e.BandId).HasName("PK__Band__CE8030A500A297E1");

            entity.ToTable("Band");

            entity.Property(e => e.BandId)
                .ValueGeneratedNever()
                .HasColumnName("band_id");
            entity.Property(e => e.BandName)
                .HasMaxLength(50)
                .HasColumnName("band_name");
            entity.Property(e => e.SinceYear).HasColumnName("since_year");
        });

        modelBuilder.Entity<Character>(entity =>
        {
            entity.HasKey(e => e.CharacterId).HasName("PK__characte__11D7652E0BEC5013");

            entity.ToTable("character");

            entity.Property(e => e.CharacterId)
                .ValueGeneratedNever()
                .HasColumnName("character_id");
            entity.Property(e => e.BandId).HasColumnName("band_id");
            entity.Property(e => e.BandPosition)
                .HasMaxLength(100)
                .HasColumnName("band_position");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.VoiceActorId).HasColumnName("voice_actor_id");
        });

        modelBuilder.Entity<Relation>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("relation");

            entity.Property(e => e.Id1).HasColumnName("id_1");
            entity.Property(e => e.Id2).HasColumnName("id_2");
            entity.Property(e => e.Relationship)
                .HasMaxLength(50)
                .HasColumnName("relationship");
        });

        modelBuilder.Entity<VoiceActor>(entity =>
        {
            entity.HasKey(e => e.VoiceActorId).HasName("PK__voice_ac__457D0293ED7A61C0");

            entity.ToTable("voice_actor");

            entity.Property(e => e.VoiceActorId)
                .ValueGeneratedNever()
                .HasColumnName("voice_actor_id");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Subsidiary)
                .HasMaxLength(100)
                .HasColumnName("subsidiary");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
