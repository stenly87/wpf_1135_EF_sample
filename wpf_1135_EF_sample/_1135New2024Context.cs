using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace wpf_1135_EF_sample;

public partial class _1135New2024Context : DbContext
{
    public _1135New2024Context()
    {
    }

    public _1135New2024Context(DbContextOptions<_1135New2024Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Agency> Agencies { get; set; }

    public virtual DbSet<Music> Musics { get; set; }

    public virtual DbSet<Singer> Singers { get; set; }

    public virtual DbSet<YellowPress> YellowPresses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { 
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        optionsBuilder.UseMySql("server=192.168.200.13;userid=student;password=student;database=1135_new_2024", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.3.39-mariadb"));
        //optionsBuilder.UseLazyLoadingProxies();
        optionsBuilder.LogTo(s=> File.AppendAllText("log.txt", s));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Agency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("agency");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Music>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("music");

            entity.HasIndex(e => e.IdSinger, "FK_music_id_singer");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CreateHash)
                .HasMaxLength(255)
                .HasColumnName("create_hash");
            entity.Property(e => e.IdSinger)
                .HasColumnType("int(11)")
                .HasColumnName("id_singer");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.IdSingerNavigation).
                WithMany(p => p.Musics)
                .HasForeignKey(d => d.IdSinger)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_music_id_singer");
        });

        modelBuilder.Entity<Singer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("singer");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CreateHash)
                .HasMaxLength(255)
                .HasColumnName("create_hash");
            entity.Property(e => e.Firstname)
                .HasMaxLength(255)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(255)
                .HasColumnName("lastname");
        });

        modelBuilder.Entity<YellowPress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("yellow_press");

            entity.HasIndex(e => e.IdSinger, "FK_yellow_press_id_singer");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CreateHash)
                .HasMaxLength(255)
                .HasColumnName("create_hash");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.IdSinger)
                .HasColumnType("int(11)")
                .HasColumnName("id_singer");
            entity.Property(e => e.TitleArticle)
                .HasMaxLength(255)
                .HasColumnName("title_article");

            entity.HasOne(d => d.IdSingerNavigation).WithMany(p => p.YellowPresses)
                .HasForeignKey(d => d.IdSinger)
                .HasConstraintName("FK_yellow_press_id_singer");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
