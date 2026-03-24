using AutoMapper;
using Invoice.DTOs.BaseDTOs;
using Invoice.DTOs.CreateDTOs;
using Invoice.Models;
using Invoice.Repository.IRepository;
using Invoice.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepo repo;
        private readonly IMapper mapper;

        public CustomerService(ICustomerRepo repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<Response<IEnumerable<CustomerResDTO>>> GetCustomers()
        {
            var customer = await repo.GetAllAsync();
            if (!customer.Any())
            {
                return Response<IEnumerable<CustomerResDTO>>.NotFound();
            }
            var res = mapper.Map<IEnumerable<CustomerResDTO>>(customer);
            return Response<IEnumerable<CustomerResDTO>>.Ok(res, "Customer Data retrieved successfully");
        }

        public async Task<Response<IEnumerable<CustomerResDTO>>> GetCustomerByUserId(Guid id)
        {
            var customer = await repo.GetCustomerByUser(id);
            if (!customer.Any())
            {
                return Response<IEnumerable<CustomerResDTO>>.NotFound();
            }
            var res = mapper.Map<IEnumerable<CustomerResDTO>>(customer);
            return Response<IEnumerable<CustomerResDTO>>.Ok(res, "Customer Data retrieved successfully");
        }

        public async Task<Response<CustomerResDTO>> GetCustomerById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Response<CustomerResDTO>.BadRequest("Invalid Customer id");
            }
            var customer = await repo.GetByIdAsync(id);

            if (customer is null)
            {
                return Response<CustomerResDTO>.NotFound($"Customer of Id {id} does not exist");
            }

            var dto = mapper.Map<CustomerResDTO>(customer);

            return Response<CustomerResDTO>.Ok(dto, "Record retrieved successfully");
        }

        public async Task<Response<CustomerResDTO>> CreateCustomer(CreateCustomerDTO createDTO)
        {
            if (createDTO == null)
            {
                return Response<CustomerResDTO>.BadRequest("Customer data is required");
            }
            if (await repo.IsCustomerExistAsync(createDTO.Email))
            {
                return Response<CustomerResDTO>.Conflict("Customer is already exists");
            }
            var model = mapper.Map<CustomerModel>(createDTO);
            model.customerid = Guid.NewGuid();
            model.createdat = DateTime.UtcNow;
            await repo.CreateAsync(model);
            var createCustomer = mapper.Map<CustomerResDTO>(model);
            return Response<CustomerResDTO>.Ok(createCustomer, "Customer Created Successfully");
        }

        public async Task<Response<CustomerResDTO>> UpdateCustomer(Guid id, UpdateCustomerDTO updateDTO)
        {
            if (id == Guid.Empty || id != updateDTO.CustomerId)
            {
                return Response<CustomerResDTO>.BadRequest("Customer id does not match to entered records with request body");
            }
            var existingCustomer = await repo.GetByIdAsync(updateDTO.CustomerId);
            if (existingCustomer is null)
            {
                return Response<CustomerResDTO>.NotFound($"Customer with id {id} does not exist in Records");
            }
            if (await repo.IsDataExistAsync(updateDTO.Email, id))
            {
                return Response<CustomerResDTO>.Conflict($"{updateDTO.Email} is already Taken");
            }
            mapper.Map(updateDTO, existingCustomer);
            existingCustomer.updatedat = DateTime.UtcNow;
            await repo.UpdateAsync(existingCustomer);
            var updateCustomer = mapper.Map<CustomerResDTO>(existingCustomer);
            return Response<CustomerResDTO>.Ok(updateCustomer, "Customer Updated Successfully");
        }

        public async Task<Response<object>> DeleteCustomer(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return Response<object>.NotFound("Invalid customer id");
                }
                var existingCustomer = await repo.GetByIdAsync(id);
                if (existingCustomer is null)
                {
                    return Response<object>.NotFound($"Customer with id {id} does not exist in Records");
                }

                await repo.RemoveAsync(existingCustomer);
                var res = mapper.Map<object>(existingCustomer);
                return Response<object>.Ok(null, "Customer Deleted Successfully...");
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                {
                    return Response<object>.BadRequest("Cannot delete this customer because invoices exist.");
                }
                throw;
            }
        }
    }
}
