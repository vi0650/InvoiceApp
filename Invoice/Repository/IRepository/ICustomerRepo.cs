using Invoice.Models;

namespace Invoice.Repository.IRepository
{
    public interface ICustomerRepo
    {
        public Task<List<CustomerModel>> GetAllAsync();
        public Task<List<CustomerModel>> GetCustomerByUser(Guid id);
        public Task<CustomerModel?> GetByIdAsync(Guid id);
        public Task CreateAsync(CustomerModel model);
        public Task UpdateAsync(CustomerModel model);
        public Task RemoveAsync(CustomerModel model);
        public Task Save();
        public Task<bool> IsDataExistAsync(string Email, Guid Id);
        public Task<bool> IsCustomerExistAsync(string Email);
    }
}
