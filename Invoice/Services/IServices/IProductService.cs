using Invoice.DTOs.CreateDTOs;
using Invoice.DTOs.ResponseDTOs;
using Invoice.Models;

namespace Invoice.Services.IServices
{
    public interface IProductService
    {
        public Task<Response<IEnumerable<ProductResDTO>>> GetProducts();
        public Task<Response<IEnumerable<ProductResDTO>>> GetProductByUserId(Guid id);
        public Task<Response<ProductResDTO>> GetProductById(Guid id);
        public Task<Response<ProductResDTO>> CreateProduct(CreateProductDTO createDTO);
        public Task<Response<ProductResDTO>> UpdateProduct(Guid id, UpdateProductDTO updateDTO);
        public Task<Response<object>> DeleteProduct(Guid id);
    }
}
