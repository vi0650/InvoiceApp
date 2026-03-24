using Invoice.Controllers;
using Invoice.Data;
using Invoice.Models;
using Invoice.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Repository
{
    public class ProductRepo : IProductRepo
    {
        private readonly InvoiceDbContext dbase;

        public ProductRepo(InvoiceDbContext dbase)
        {
            this.dbase = dbase;
        }

        public async Task<List<ProductModel>> GetAllAsync()
        {
            return await dbase.products.AsNoTracking().ToListAsync();
        }

        public async Task<List<ProductModel>> GetProductByUser(Guid id)
        {
            return await dbase.products.AsNoTracking().Where(x => x.userid == id).ToListAsync();
        }

        public async Task<ProductModel?> GetByIdAsync(Guid id)
        {
            return await dbase.products.FirstOrDefaultAsync(a => a.productid == id);
        }

        public async Task CreateAsync(ProductModel model)
        {
            await dbase.products.AddAsync(model);
            await Save();
        }

        public async Task UpdateAsync(ProductModel model)
        {
            dbase.products.Update(model);
            await Save();
        }

        public async Task RemoveAsync(ProductModel model)
        {
            dbase.products.Remove(model);
            await Save();
        }

        public async Task Save()
        {
            await dbase.SaveChangesAsync();
        }

        public async Task<bool> IsDataExistAsync(string Name, Guid id)
        {
            return await dbase.products.AnyAsync(p => p.productname == Name && p.productid != id);
        } 

        public async Task<bool> IsProductExistAsync(string Name)
        {
            return await dbase.products.AnyAsync(p => p.productname == Name);
        }
    }
}
