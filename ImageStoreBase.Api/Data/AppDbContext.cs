using ImageStoreBase.Api.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ImageStoreBase.Api.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<CommandInFunction> CommandInFunctions { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ImageInAlbum> ImageInAlbums { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Collection>(entity =>
            {
                entity.HasOne(c => c.User)
                      .WithMany(u => u.Collections)
                      .HasForeignKey(c => c.UserId);
            });

            builder.Entity<Album>(entity =>
            {
                entity.HasOne(a => a.Collection)
                      .WithMany(c => c.Albums)
                      .HasForeignKey(a => a.CollectionId);
            });

            builder.Entity<ImageInAlbum>(entity =>
            {
                entity.HasKey(ia => new { ia.AlbumId, ia.ImageId });

                entity.HasOne(ia => ia.Album)
                      .WithMany(a => a.ImageInAlbums)
                      .HasForeignKey(ia => ia.AlbumId);

                entity.HasOne(ia => ia.Image)
                      .WithMany(i => i.ImageInAlbums)
                      .HasForeignKey(ia => ia.ImageId);
            });

            builder.Entity<Function>(entity =>
            {
                entity.HasOne(f => f.FunctionParent)
                      .WithMany(f => f.ChildFunctions)
                      .HasForeignKey(f => f.ParentId)
                      .IsRequired(false);
            });

            builder.Entity<CommandInFunction>(entity =>
            {
                entity.HasKey(cf => new { cf.CommandId, cf.FunctionId });

                entity.HasOne(cf => cf.Command)
                      .WithMany(c => c.CommandInFunctions)
                      .HasForeignKey(cf => cf.CommandId);

                entity.HasOne(cf => cf.Function)
                      .WithMany(f => f.CommandInFunctions)
                      .HasForeignKey(cf => cf.FunctionId);
            });

            builder.Entity<Permission>(entity =>
            {
                entity.HasKey(p => new { p.RoleId, p.FunctionId, p.CommandId });

                entity.HasOne(p => p.Role)
                      .WithMany(r => r.Permissions)
                      .HasForeignKey(p => p.RoleId);

                entity.HasOne(p => p.Function)
                      .WithMany(f => f.Permissions)
                      .HasForeignKey(p => p.FunctionId);

                entity.HasOne(p => p.Command)
                      .WithMany(c => c.Permissions)
                      .HasForeignKey(p => p.CommandId);
            });
        }
    }
}
