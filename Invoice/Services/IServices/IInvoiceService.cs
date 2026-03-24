using Invoice.DTOs.BaseDTOs;
using Invoice.DTOs.CreateDTOs;
using Invoice.Models;

namespace Invoice.Services.IServices
{
    public interface IInvoiceService
    {
        public Task<Response<IEnumerable<InvoiceResDTO>>> GetInvoice();
        public Task<Response<IEnumerable<InvoiceResDTO>>> GetInvoiceByUser(Guid id);
        public Task<Response<InvoiceResDTO>> GetInvoiceById(Guid id);
        public Task<Response<InvoiceResDTO>> CreateInvoice(CreateInvoiceDTO createDTO);
        public Task<Response<InvoiceResDTO>> UpdateInvoice(Guid id, UpdateInvoiceDTO updateDTO);
        public Task<Response<object>> DeleteInvoice(Guid id);
    }
}
