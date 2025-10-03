    using crud_backend.Models;
    using global::crud_backend.Models;
    using Microsoft.EntityFrameworkCore;

    namespace crud_backend.Data
    {
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            public DbSet<User> Users { get; set; }
        }
    }
