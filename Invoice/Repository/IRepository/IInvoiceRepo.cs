using Invoice.Models;

namespace Invoice.Repository.IRepository
{
    public interface IInvoiceRepo
    {
        public Task<List<InvoiceModel>> GetAllAsync();
        public Task<List<InvoiceModel>> GetInvoiceByUser(Guid id);
        public Task<InvoiceModel?> GetByIdAsync(Guid id);
        public Task<InvoiceModel?> GetInvoiceByIdAsync(Guid id);
        public Task CreateAsync(InvoiceModel model);
        public void UpdateAsync(InvoiceModel model);
        public void RemoveAsync(InvoiceModel model);
        public Task SaveAsync();
        public Task<bool> IsInvoiceExistAsync(Guid id);
        public Task<InvoiceModel?> GetInvoiceWithItemsAsync(Guid id);
        public void RemoveInvoiceItemAsync(InvoiceItemsModel model);
    }
}
