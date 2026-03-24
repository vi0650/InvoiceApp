using Invoice.DTOs.BaseDTOs;
using Invoice.DTOs.CreateDTOs;
using Invoice.Models;

namespace Invoice.Services.IServices
{
    public interface IInvoiceItemService
    {
        public Task<Response<IEnumerable<InvoiceItemResDTO>>> GetInvoiceItem();
        public Task<Response<InvoiceItemResDTO>> GetInvoiceItemById(Guid id);
        public Task<Response<InvoiceItemResDTO>> CreateInvoiceItem(CreateInvoiceItemDTO createItemDTO);
        public Task<Response<InvoiceItemResDTO>> UpdateInvoiceItem(Guid id, UpdateInvoiceItemDTO updateItemDTO);
        public Task<Response<InvoiceItemResDTO>> DeleteInvoiceItem(Guid id);
    }
}
