using Invoice.Models;

namespace Invoice.Repository.IRepository
{
    public interface IGstRepo
    {
        public Task<List<GstModel>> GetAllGst();
        public Task<List<GstModel>> GetGstByUser(Guid id);
        public Task<GstModel?> GetByIdAsync(Guid id);
        public Task CreateAsync(GstModel model);
        public Task UpdateAsync(GstModel model);
        public Task RemoveAsync(GstModel model);
        public Task Save();
        public Task<bool> IsDataExistAsync(decimal GstRate, Guid id);
        public Task<bool> IsGstExistAsync(decimal GstRate);
    }
}
