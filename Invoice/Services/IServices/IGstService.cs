using Invoice.DTOs.CreateDTOs;
using Invoice.DTOs.ResponseDTOs;
using Invoice.DTOs.UpdateDTOs;
using Invoice.Models;

namespace Invoice.Services.IServices
{
    public interface IGstService
    {
        public Task<Response<IEnumerable<GstResDTO>>> GetGst();
        public Task<Response<IEnumerable<GstResDTO>>> GetGstByUserId(Guid id);
        public Task<Response<GstResDTO>> GetGstById(Guid id);
        public Task<Response<GstResDTO>> CreateGst(CreateGstDTO createDTO);
        public Task<Response<GstResDTO>> UpdateGst(Guid id, UpdateGstDTO updateDTO);
        public Task<Response<object>> DeleteGst(Guid id);
    }
}
