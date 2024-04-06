using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DomainModels
{
    public partial class ASTDBContext : DbContext
    {
        public ASTDBContext()
        {

        }
        public ASTDBContext(DbContextOptions<ASTDBContext> options)
: base(options)
        {
        }

        //public virtual DbSet<OpcionesCategoria> OpcionesCategoria { get; set; }
        //public virtual DbSet<OpcionesMenu> OpcionesMenu { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<OpcionesMenu>(entity =>
            //{
            //    entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            //    entity.HasOne(d => d.IdCategoriaNavigation)
            //       .WithMany()
            //       .HasForeignKey(d => d.IdCategoria);
                  
            //});

            //modelBuilder.Entity<OpcionesCategoria>(entity =>
            //{
            //    entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            //});
        }
    }
}
