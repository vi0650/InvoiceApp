using Invoice.Data;
using Invoice.Models;
using Invoice.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Repository
{
    public class InoviceRepo : IInvoiceRepo
    {
        private readonly InvoiceDbContext dbase;

        public InoviceRepo(InvoiceDbContext dbase)
        {
            this.dbase = dbase;
        }

        public async Task<List<InvoiceModel>> GetAllAsync()
        {
            return await dbase.invoices
                .Include(i => i.customer)
                .Include(i => i.invoiceitems)
                .ThenInclude(i => i.products).AsNoTracking().ToListAsync();
        }

        public async Task<List<InvoiceModel>> GetInvoiceByUser(Guid id)
        {
            return await dbase.invoices.Where(x => x.userid == id)
                .Include(i => i.customer)
                .Include(i => i.invoiceitems)
                .ThenInclude(i => i.products).AsNoTracking().ToListAsync();
        }

        public async Task<InvoiceModel?> GetByIdAsync(Guid id)
        {
            return await dbase.invoices
                .Include(x => x.customer)
                .Include(x => x.invoiceitems)
                .FirstOrDefaultAsync(x => x.invoiceid == id);
        }

        public async Task<InvoiceModel?> GetInvoiceByIdAsync(Guid id)
        {
            return await dbase.invoices
                    .Include(i => i.customer)
                    .Include(i => i.invoiceitems)
                    .ThenInclude(i => i.products)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(i => i.invoiceid == id);
        }
        public async Task CreateAsync(InvoiceModel model)
        {
            await dbase.invoices.AddAsync(model);
        }

        public void UpdateAsync(InvoiceModel model)
        {
            dbase.invoices.Update(model);
        }

        public void RemoveAsync(InvoiceModel model)
        {
            dbase.invoices.Remove(model);
        }

        public async Task SaveAsync()
        {
            await dbase.SaveChangesAsync();
        }

        public async Task<bool> IsInvoiceExistAsync(Guid id)
        {
            return await dbase.invoices.AnyAsync(p => p.invoiceid == id);
        }

        public async Task<InvoiceModel?> GetInvoiceWithItemsAsync(Guid id)
        {
            return await dbase.invoices
                .Include(x => x.invoiceitems)
                .FirstOrDefaultAsync(x => x.invoiceid == id);
        }

        public void RemoveInvoiceItemAsync(InvoiceItemsModel model)
        {
            dbase.invoiceitems.RemoveRange(model);
        }
    }
}
