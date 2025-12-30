using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<FlashCard> FlashCards { get; set; }
    public DbSet<FlashCardCollection> FlashCardCollections { get; set; }
    public DbSet<MindMap> MindMaps { get; set; }
    public DbSet<MindMapNode> MindMapNodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasDefaultValue(UserRole.User);
        });

        // User → Collections
        modelBuilder.Entity<FlashCardCollection>()
            .HasOne(fc => fc.User)
            .WithMany(u => u.FlashCardCollections)
            .HasForeignKey(fc => fc.UserId);

        // Self-referencing (collection → parent/children)
        modelBuilder.Entity<FlashCardCollection>()
            .HasOne(fc => fc.Parent)
            .WithMany(fc => fc.Children)
            .HasForeignKey(fc => fc.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Collection → FlashCards
        modelBuilder.Entity<FlashCard>()
            .HasOne(f => f.FlashCardCollection)
            .WithMany(fc => fc.FlashCards)
            .HasForeignKey(f => f.FlashCardCollectionId)
            .OnDelete(DeleteBehavior.Cascade);

        // User → MindMaps
        modelBuilder.Entity<MindMap>()
            .HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // MindMap → Nodes
        modelBuilder.Entity<MindMapNode>()
            .HasOne(n => n.MindMap)
            .WithMany(m => m.Nodes)
            .HasForeignKey(n => n.MindMapId)
            .OnDelete(DeleteBehavior.Cascade);

        // Node → FlashCard
        modelBuilder.Entity<MindMapNode>()
            .HasOne(n => n.FlashCard)
            .WithMany()
            .HasForeignKey(n => n.FlashCardId)
            .OnDelete(DeleteBehavior.Restrict);

        // Self-referencing (node → parent/children)
        modelBuilder.Entity<MindMapNode>()
            .HasOne(n => n.ParentNode)
            .WithMany(n => n.ChildNodes)
            .HasForeignKey(n => n.ParentNodeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
