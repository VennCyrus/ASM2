using FPTBOOK_STORE.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FPTBOOK_STORE.Areas.Identity.Data;

public class FPTBOOK_STOREIdentityDbContext : IdentityDbContext<FPTBOOKUser>
{
    public FPTBOOK_STOREIdentityDbContext(DbContextOptions<FPTBOOK_STOREIdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<FPTBOOK_STORE.Models.Book>()
        .Property(p => p.Price).HasColumnType("decimal(18,4)");
        modelBuilder.Entity<FPTBOOK_STORE.Models.Category>()
        .Property(p => p.Status).HasDefaultValue(false);

    }

    public DbSet<FPTBOOK_STORE.Models.Category> Category { get; set; } = default!;

    public DbSet<FPTBOOK_STORE.Models.Publisher>? Publisher { get; set; }

    public DbSet<FPTBOOK_STORE.Models.Author>? Author { get; set; }

    public DbSet<FPTBOOK_STORE.Models.Book>? Book { get; set; }


    public DbSet<FPTBOOK_STORE.Models.Order>? Order { get; set; }

    public DbSet<FPTBOOK_STORE.Models.OrderDetail>? OrderDetail { get; set; }
}
