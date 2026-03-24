using Invoice.Data;
using Invoice.Models;
using Invoice.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Repository
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly InvoiceDbContext dbase;

        public CustomerRepo(InvoiceDbContext dbase)
        {
            this.dbase = dbase;
        }

        public async Task<List<CustomerModel>> GetAllAsync()
        {
            return await dbase.customers.AsNoTracking().ToListAsync();
        }

        public async Task<List<CustomerModel>> GetCustomerByUser(Guid id)
        {
            return await dbase.customers.AsNoTracking().Where(x => x.userid == id).ToListAsync();
        }

        public async Task<CustomerModel?> GetByIdAsync(Guid id)
        {
            return await dbase.customers.FirstOrDefaultAsync(a => a.customerid == id);
        }

        public async Task CreateAsync(CustomerModel model)
        {
            await dbase.customers.AddAsync(model);
            await Save();
        }

        public async Task UpdateAsync(CustomerModel model)
        {
            dbase.customers.Update(model);
            await Save();
        }

        public async Task RemoveAsync(CustomerModel model)
        {
            dbase.customers.Remove(model);
            await Save();
        }

        public async Task Save()
        {
            await dbase.SaveChangesAsync();
        }

        public async Task<bool> IsDataExistAsync(string Email, Guid id)
        {
            return await dbase.customers.AnyAsync(p => p.email == Email && p.customerid != id);
        }

        public async Task<bool> IsCustomerExistAsync(string Email)
        {
            return await dbase.customers.AnyAsync(p => p.email == Email);
        }
    }
}
