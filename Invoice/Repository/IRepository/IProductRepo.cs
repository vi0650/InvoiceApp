using Invoice.Models;

namespace Invoice.Repository.IRepository
{
    public interface IProductRepo
    {
        public Task<List<ProductModel>> GetAllAsync();
        public Task<List<ProductModel>> GetProductByUser(Guid id);
        public Task<ProductModel?> GetByIdAsync(Guid id);
        public Task CreateAsync(ProductModel model);
        public Task UpdateAsync(ProductModel model);
        public Task RemoveAsync(ProductModel model);
        public Task Save();
        public Task<bool> IsDataExistAsync(string Name,Guid Id);
        public Task<bool> IsProductExistAsync(string Name);
    }
}