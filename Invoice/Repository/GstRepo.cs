using Invoice.Data;
using Invoice.Models;
using Invoice.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Repository
{
    public class GstRepo : IGstRepo
    {
        private readonly InvoiceDbContext dbase;

        public GstRepo(InvoiceDbContext dbase)
        {
            this.dbase = dbase;
        }

        public async Task<List<GstModel>> GetAllGst()
        {
            return await dbase.gst.AsNoTracking().ToListAsync();
        }

        public async Task<GstModel?> GetByIdAsync(Guid id)
        {
            return await dbase.gst.FirstOrDefaultAsync(g => g.rateid == id);
        }

        public async Task<List<GstModel>> GetGstByUser(Guid id)
        {
            return await dbase.gst.AsNoTracking().Where(g => g.userid == id).ToListAsync();
        }

        public async Task CreateAsync(GstModel model)
        {
            await dbase.gst.AddAsync(model);
            await Save();
        }

        public async Task UpdateAsync(GstModel model)
        {
            dbase.Update(model);
            await Save();
        }

        public async Task RemoveAsync(GstModel model)
        {
            dbase.gst.Remove(model);
            await Save();
        }

        public async Task Save()
        {
            await dbase.SaveChangesAsync();
        }

        public async Task<bool> IsDataExistAsync(decimal Rate, Guid id)
        {
            return await dbase.gst.AnyAsync(g => g.gst == Rate && g.rateid != id );
        }

        public async Task<bool> IsGstExistAsync(decimal Rate)
        {
            return await dbase.gst.AnyAsync(g => g.gst == Rate);
        }
    }
}
