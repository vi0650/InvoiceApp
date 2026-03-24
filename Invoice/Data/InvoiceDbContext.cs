using Invoice.Models;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Data
{
    public class InvoiceDbContext : DbContext
    {
        public InvoiceDbContext(DbContextOptions<InvoiceDbContext> options) : base(options)
        {
        }
        public DbSet<UserModel> user { get; set; }
        public DbSet<ProductModel> products { get; set; }
        public DbSet<GstModel> gst { get; set; }
        public DbSet<CustomerModel> customers { get; set; }
        public DbSet<InvoiceItemsModel> invoiceitems { get; set; }
        public DbSet<InvoiceModel> invoices { get; set; }
        public DbSet<InvoiceStatusModel> invoicestatus { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().ToTable("users");
            modelBuilder.Entity<GstModel>().ToTable("gstrate");

            modelBuilder.Entity<CustomerModel>()
                .HasOne(c => c.users)
                .WithMany(u => u.customers)
                .HasForeignKey(c => c.userid)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProductModel>()
                .HasOne(c => c.users)
                .WithMany(p => p.products)
                .HasForeignKey(c => c.userid)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<InvoiceModel>()
                .HasOne(c=> c.users)
                .WithMany(p=> p.invoices)
                .HasForeignKey(c=>c.userid)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InvoiceModel>()
                .HasOne(c => c.customer)
                .WithMany(p => p.invoices)
                .HasForeignKey(c => c.customerid)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<InvoiceItemsModel>()    
                .HasOne(c=>c.invoices)
                .WithMany(p=>p.invoiceitems)
                .HasForeignKey(c=>c.invoiceid)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InvoiceItemsModel>()
                .HasOne(c => c.products)
                .WithMany(p => p.invoiceitems)
                .HasForeignKey(c => c.productid)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<InvoiceStatusModel>().ToTable("invoicestatus");

            base.OnModelCreating(modelBuilder);
        }
    }
}
