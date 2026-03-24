using Invoice.Data;
using Invoice.Models;
using Invoice.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly InvoiceDbContext dbase;

        public UserRepo(InvoiceDbContext dbase)
        {
            this.dbase = dbase;
        }

        public async Task<List<UserModel>> GetAllAsync()
        {
            return await dbase.user.AsNoTracking().ToListAsync();
        }

        public async Task<UserModel?> GetByIdAsync(Guid id)
        {
            return await dbase.user.FirstOrDefaultAsync(a => a.userid == id);
        }

        public async Task CreateAsync(UserModel model)
        {
            await dbase.AddAsync(model);
            await Save();
        }

        public async Task UpdateAsync(UserModel model)
        {
            dbase.user.Update(model);
            await Save();
        }

        public async Task RemoveAsync(UserModel model)
        {
            dbase.user.Remove(model);
            await Save();
        }

        public async Task Save()
        {
            await dbase.SaveChangesAsync();
        }

        public async Task<bool> IsDataExistAsync(string Email, Guid Id)
        {
            return await dbase.user.AnyAsync(x => x.email.ToLower() == Email.ToLower() && x.userid != Id);
        }

        public async Task<bool> IsEmailExistAsync(string Email)
        {
            return await dbase.user.AnyAsync(x => x.email.ToLower() == Email.ToLower());
        }

        public async Task<UserModel?> GetByEmailAsync(string Email)
        {
            return await dbase.user.AsNoTracking().FirstOrDefaultAsync(x => x.email.ToLower() == Email.ToLower());
        }
    }
}
