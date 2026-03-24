using AutoMapper;
using Invoice.DTOs.CreateDTOs;
using Invoice.DTOs.ResponseDTOs;
using Invoice.Models;
using Invoice.Repository.IRepository;
using Invoice.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo repo;
        private readonly IMapper mapper;

        public ProductService(IProductRepo repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<Response<IEnumerable<ProductResDTO>>> GetProducts()
        {
            var product = await repo.GetAllAsync();
            if (!product.Any())
            {
                return Response<IEnumerable<ProductResDTO>>.NotFound();
            }
            var res = mapper.Map<IEnumerable<ProductResDTO>>(product);
            return Response<IEnumerable<ProductResDTO>>.Ok(res, "Product Data retrieved successfully");
        }

        public async Task<Response<IEnumerable<ProductResDTO>>> GetProductByUserId(Guid id)
        {
            var product = await repo.GetProductByUser(id);
            if (!product.Any())
            {
                return Response<IEnumerable<ProductResDTO>>.NotFound();
            }
            var res = mapper.Map<IEnumerable<ProductResDTO>>(product);
            return Response<IEnumerable<ProductResDTO>>.Ok(res, "Product Data retrieved successfully");
        }

        public async Task<Response<ProductResDTO>> GetProductById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Response<ProductResDTO>.BadRequest("Invalid Product id");
            }
            var product = await repo.GetByIdAsync(id);

            if (product is null)
            {
                return Response<ProductResDTO>.NotFound($"Product of Id {id} does not exist");
            }

            var dto = mapper.Map<ProductResDTO>(product);

            return Response<ProductResDTO>.Ok(dto, "Record retrieved successfully");
        }

        public async Task<Response<ProductResDTO>> CreateProduct(CreateProductDTO createDTO)
        {
            if (createDTO == null)
            {
                return Response<ProductResDTO>.BadRequest("Product data is required");
            }
            if (await repo.IsProductExistAsync(createDTO.ProductName))
            {
                return Response<ProductResDTO>.Conflict("Product is already exists");
            }

            var model = mapper.Map<ProductModel>(createDTO);
            model.productid = Guid.NewGuid();
            model.createdat = DateTime.UtcNow;
            await repo.CreateAsync(model);
            var createProduct = mapper.Map<ProductResDTO>(model);
            return Response<ProductResDTO>.Ok(createProduct, "Product Created Successfully");
        }

        public async Task<Response<ProductResDTO>> UpdateProduct(Guid id, UpdateProductDTO updateDTO)
        {
            if (id == Guid.Empty || id != updateDTO.ProductId)
            {
                return Response<ProductResDTO>.BadRequest("Product id does not match to entered records with request body");
            }
            var existingProduct = await repo.GetByIdAsync(updateDTO.ProductId);
            if (existingProduct is null)
            {
                return Response<ProductResDTO>.NotFound($"Product with id {id} does not exist in Records");
            }
            if (await repo.IsDataExistAsync(updateDTO.ProductName, id))
            {
                return Response<ProductResDTO>.Conflict($"{updateDTO.ProductName} is already Taken");
            }
            mapper.Map(updateDTO, existingProduct);
            existingProduct.updatedat = DateTime.UtcNow;
            await repo.UpdateAsync(existingProduct);
            var updateUser = mapper.Map<ProductResDTO>(existingProduct);
            return Response<ProductResDTO>.Ok(updateUser, "Product Updated Successfully");
        }

        public async Task<Response<object>> DeleteProduct(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return Response<object>.NotFound("Invalid Product id");
                }
                var existingProduct = await repo.GetByIdAsync(id);
                if (existingProduct is null)
                {
                    return Response<object>.NotFound($"Product with id {id} does not exist in Records");
                }

                await repo.RemoveAsync(existingProduct);
                var res = mapper.Map<ProductResDTO>(existingProduct);
                return Response<object>.Ok(null, "Product Deleted Successfully...");
            }
            catch (DbUpdateException)
            {
                return Response<object>.Error(409, "Products has invoices. Delete invoices first.");
            }
        }
    }
}
