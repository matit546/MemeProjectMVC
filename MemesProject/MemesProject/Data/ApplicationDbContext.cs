using MemesProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MemesProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        //public DbSet<CommentActivity> CommentActivities { get; set; }
        public DbSet<CommentsHub> CommentsHubs { get; set; }
        public DbSet<FavoritesMemes> FavoritesMemes { get; set; }
        public DbSet<LikedMemes> LikedMemes { get; set; }
        public DbSet<Meme> Memes { get; set; }

        public DbSet<Observation> Observations { get; set; }
        //public DbSet<Report> Reports { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>(entity =>
            {
                entity.HasMany(x => x.Meme)
                .WithOne(y => y.CategoryEntity)
                .HasForeignKey(z => z.IdCategory);
            });
            //builder.Entity<Meme>(entity =>
            //{
            //    entity.HasOne(x => x.ApplicationUser)
            //    .WithMany(y => y.MemesCollection)
            //    .HasForeignKey(z => z.IdUser)
            //    .IsRequired();
            //});
            builder.Entity<CommentsHub>(entity =>
            {
                entity.HasMany(x => x.Comments)
                .WithOne(y => y.CommentsHub)
                .HasForeignKey(z => z.IdCommentsHub);
            });
            //builder.Entity<Comment>(entity =>
            //{
            //    entity.HasOne(x => x.CommentActivity)
            //    .WithOne(y => y.Comment)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //});
            builder.Entity<LikedMemes>(entity =>
            {
                entity.HasOne(x => x.Meme)
                .WithMany(y => y.LikedMemes)
               .HasForeignKey(z=> z.IdMeme);
            });
            builder.Entity<FavoritesMemes>(entity =>
            {
                entity.HasOne(x => x.Meme)
                 .WithMany(y => y.FavoritesMemes)
                .HasForeignKey(z => z.IdMeme);
            });
            builder.Entity<CommentsHub>(entity =>
            {
                entity.HasOne(x => x.Meme)
                .WithMany(y => y.CommentsHub)
                .HasForeignKey(z => z.IdMeme);
            });

            builder.Entity<Observation>(entity =>
            {
                entity.HasOne(x => x.ApplicationUser)
                .WithMany(y => y.Observations)
                .HasForeignKey(z => z.IdObservedUser);
            });
        }
    }
}