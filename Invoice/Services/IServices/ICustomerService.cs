using Invoice.DTOs.BaseDTOs;
using Invoice.DTOs.CreateDTOs;
using Invoice.Models;

namespace Invoice.Services.IServices
{
    public interface ICustomerService
    {
        public Task<Response<IEnumerable<CustomerResDTO>>> GetCustomers();
        public Task<Response<IEnumerable<CustomerResDTO>>> GetCustomerByUserId(Guid id);
        public Task<Response<CustomerResDTO>> GetCustomerById(Guid id);
        public Task<Response<CustomerResDTO>> CreateCustomer(CreateCustomerDTO createDTO);
        public Task<Response<CustomerResDTO>> UpdateCustomer(Guid id, UpdateCustomerDTO updateDTO);
        public Task<Response<object>> DeleteCustomer(Guid id);
    }
}
