using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace basketballAPI.models;

public partial class SoftwareBirbContext : DbContext
{
    public SoftwareBirbContext()
    {
    }

    public SoftwareBirbContext(DbContextOptions<SoftwareBirbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Stat> Stats { get; set; }

    public virtual DbSet<StatsType> StatsTypes { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamPlayer> TeamPlayers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=will-software-sql.database.windows.net;Database=SoftwareBirb;User Id=dbadmin;Password=$Egg420gryph;Encrypt=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.GameNo);

            entity.Property(e => e.GameNo)
                .ValueGeneratedNever()
                .HasColumnName("gameNo");
            entity.Property(e => e.ScoreOne).HasColumnName("scoreOne");
            entity.Property(e => e.ScoreTwo).HasColumnName("scoreTwo");
            entity.Property(e => e.TeamNoOne).HasColumnName("teamNoOne");

            entity.HasOne(d => d.TeamNoOneNavigation).WithMany(p => p.GameTeamNoOneNavigations)
                .HasForeignKey(d => d.TeamNoOne)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Games_Teams1");

            entity.HasOne(d => d.TeamNoTwoNavigation).WithMany(p => p.GameTeamNoTwoNavigations)
                .HasForeignKey(d => d.TeamNoTwo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Games_Teams2");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .HasColumnName("ID");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.PlayerNo);

            entity.Property(e => e.PlayerNo).HasColumnName("playerNo");
            entity.Property(e => e.FName)
                .HasMaxLength(25)
                .HasColumnName("fName");
            entity.Property(e => e.LName)
                .HasMaxLength(25)
                .HasColumnName("lName");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => new { e.GameDate, e.GameTime, e.GameNo });

            entity.ToTable("Schedule");

            entity.Property(e => e.GameDate)
                .HasMaxLength(10)
                .HasColumnName("gameDate");
            entity.Property(e => e.GameTime)
                .HasMaxLength(7)
                .HasColumnName("gameTime");
            entity.Property(e => e.GameNo).HasColumnName("gameNo");
            entity.Property(e => e.City)
                .HasMaxLength(30)
                .HasColumnName("city");
            entity.Property(e => e.State)
                .HasMaxLength(30)
                .HasColumnName("state");
            entity.Property(e => e.Zipcode)
                .HasMaxLength(15)
                .HasColumnName("zipcode");

            entity.HasOne(d => d.GameNoNavigation).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.GameNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_Schedule");
        });

        modelBuilder.Entity<Stat>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GameId).HasColumnName("gameId");
            entity.Property(e => e.PlayerTeamId).HasColumnName("playerTeamId");
            entity.Property(e => e.StatTypeId).HasColumnName("statTypeId");
            entity.Property(e => e.StatValue).HasColumnName("statValue");

            entity.HasOne(d => d.Game).WithMany(p => p.Stats)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_Stats_Stats1");

            entity.HasOne(d => d.PlayerTeam).WithMany(p => p.Stats)
                .HasForeignKey(d => d.PlayerTeamId)
                .HasConstraintName("FK_Stats_Stats");

            entity.HasOne(d => d.StatType).WithMany(p => p.Stats)
                .HasForeignKey(d => d.StatTypeId)
                .HasConstraintName("FK_Stats_Stats2");
        });

        modelBuilder.Entity<StatsType>(entity =>
        {
            entity.HasKey(e => e.StatId);

            entity.ToTable("StatsType");

            entity.Property(e => e.StatId).HasColumnName("statId");
            entity.Property(e => e.StatAbv)
                .HasMaxLength(5)
                .HasColumnName("statAbv");
            entity.Property(e => e.StatName)
                .HasMaxLength(50)
                .HasColumnName("statName");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.TeamNo);

            entity.Property(e => e.TeamNo).HasColumnName("teamNo");
            entity.Property(e => e.Losses).HasColumnName("losses");
            entity.Property(e => e.TeamAbbreviation)
                .HasMaxLength(3)
                .HasColumnName("teamAbbreviation");
            entity.Property(e => e.TeamName)
                .HasMaxLength(50)
                .HasColumnName("teamName");
            entity.Property(e => e.Wins).HasColumnName("wins");
        });

        modelBuilder.Entity<TeamPlayer>(entity =>
        {
            entity.HasIndex(e => e.PlayerId, "IX_TeamPlayers").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PlayerId).HasColumnName("playerId");
            entity.Property(e => e.TeamId).HasColumnName("teamId");

            entity.HasOne(d => d.Player).WithOne(p => p.TeamPlayer)
                .HasForeignKey<TeamPlayer>(d => d.PlayerId)
                .HasConstraintName("FK_TeamPlayers_Players");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamPlayers)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_TeamPlayers_Teams");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
