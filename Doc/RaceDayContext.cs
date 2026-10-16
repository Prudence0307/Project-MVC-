using Microsoft.EntityFrameworkCore;
using RaceDayMVC.Models;

namespace RaceDayMVC.Data;

public class RaceDayContext : DbContext
{
    public RaceDayContext(DbContextOptions<RaceDayContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<RaceEvent> Events => Set<RaceEvent>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<RaceRoute> Routes => Set<RaceRoute>();
    public DbSet<Enrolment> Enrolments => Set<Enrolment>();
    public DbSet<RaceResult> Results => Set<RaceResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users").HasKey(x => x.UserID);
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<RaceEvent>().ToTable("Events").HasKey(x => x.EventID);
        modelBuilder.Entity<Category>().ToTable("Categories").HasKey(x => x.CategoryID);
        modelBuilder.Entity<RaceRoute>().ToTable("Routes").HasKey(x => x.RouteID);
        modelBuilder.Entity<Enrolment>().ToTable("Enrolments").HasKey(x => x.EnrolmentID);
        modelBuilder.Entity<RaceResult>().ToTable("Results").HasKey(x => x.ResultID);

        modelBuilder.Entity<RaceEvent>().HasOne(x => x.Organiser).WithMany(x => x.OrganisedEvents)
            .HasForeignKey(x => x.OrganiserID).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Category>().HasOne(x => x.Event).WithMany(x => x.Categories)
            .HasForeignKey(x => x.EventID).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<RaceRoute>().HasOne(x => x.Category).WithOne(x => x.Route)
            .HasForeignKey<RaceRoute>(x => x.CategoryID).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<RaceRoute>().HasIndex(x => x.CategoryID).IsUnique();
        modelBuilder.Entity<Enrolment>().HasOne(x => x.Participant).WithMany(x => x.Enrolments)
            .HasForeignKey(x => x.ParticipantID).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Enrolment>().HasOne(x => x.Category).WithMany(x => x.Enrolments)
            .HasForeignKey(x => x.CategoryID).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Enrolment>().HasIndex(x => new { x.ParticipantID, x.CategoryID }).IsUnique();
        modelBuilder.Entity<RaceResult>().HasOne(x => x.Enrolment).WithOne(x => x.Result)
            .HasForeignKey<RaceResult>(x => x.EnrolmentID).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<RaceResult>().HasIndex(x => x.EnrolmentID).IsUnique();
        base.OnModelCreating(modelBuilder);
    }
}
