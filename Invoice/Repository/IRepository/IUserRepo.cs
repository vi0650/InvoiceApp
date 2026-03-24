using Invoice.Models;

namespace Invoice.Repository.IRepository
{
    public interface IUserRepo
    {
        public Task<List<UserModel>> GetAllAsync();
        public Task<UserModel?> GetByIdAsync(Guid id);
        public Task<UserModel?> GetByEmailAsync(string email);
        public Task CreateAsync(UserModel model);
        public Task UpdateAsync(UserModel model);
        public Task RemoveAsync(UserModel model);
        public Task Save();
        public Task<bool> IsEmailExistAsync(string Email);
        public Task<bool> IsDataExistAsync(string Email, Guid Id);
    }
}