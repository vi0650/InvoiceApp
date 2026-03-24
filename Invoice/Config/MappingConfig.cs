using AutoMapper;
using Invoice.DTOs.AuthDTO;
using Invoice.DTOs.BaseDTOs;
using Invoice.DTOs.CreateDTOs;
using Invoice.DTOs.ResponseDTOs;
using Invoice.DTOs.UpdateDTOs;
using Invoice.Models;

namespace Invoice.Config
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<UserModel, CreateUserDTO>().ReverseMap();
            CreateMap<UserModel, UpdateUserDTO>().ReverseMap();
            CreateMap<UserModel, UserResDTO>();

            CreateMap<ProductModel,CreateProductDTO>().ReverseMap();
            CreateMap<ProductModel, UpdateProductDTO>().ReverseMap();
            CreateMap<ProductModel, ProductResDTO>();

            CreateMap<CustomerModel, CreateCustomerDTO>().ReverseMap();
            CreateMap<CustomerModel, UpdateCustomerDTO>().ReverseMap();
            CreateMap<CustomerModel, CustomerResDTO>();

            CreateMap<GstModel, CreateGstDTO>().ReverseMap();
            CreateMap<GstModel, UpdateGstDTO>().ReverseMap();
            CreateMap<GstModel, GstResDTO>();

            CreateMap<InvoiceItemsModel, InvoiceItemResDTO>();
            //.ForMember(d => d.Products, o => o.Ignore());
            CreateMap<UpdateInvoiceItemDTO, InvoiceItemsModel>()
                //.ForMember(d => d.InvoiceItemId, opt => opt.Ignore())
                .ForMember(d => d.invoiceid, opt => opt.Ignore())
                .ForMember(d => d.createdat, opt => opt.Ignore())
                .ForMember(d => d.updatedat, opt => opt.MapFrom(_ => DateTime.UtcNow));


            CreateMap<CreateInvoiceItemDTO, InvoiceItemsModel>()
                .ForMember(item => item.invoiceitemid,
                o => o.MapFrom(_ => Guid.NewGuid()))
                .ForMember(date => date.createdat,
                o => o.MapFrom(_ => DateTime.UtcNow));

            CreateMap<InvoiceModel, InvoiceResDTO>()
                .ForMember(dest => dest.Customer,
                opt => opt.MapFrom(src => src.customer))
                .ForMember(item => item.InvoiceItems,
                opt => opt.MapFrom(src => src.invoiceitems));

            CreateMap<CreateInvoiceDTO, InvoiceModel>()
                .ForMember(id => id.invoiceid,
                o => o.MapFrom(_ => Guid.NewGuid()))
                .ForMember(date => date.createdat, 
                o => o.MapFrom(_ => DateTime.UtcNow))
                .ForMember(item => item.invoiceitems,
                o => o.MapFrom(src => src.InvoiceItems));

            CreateMap<UpdateInvoiceDTO, InvoiceModel>()
                .ForMember(id => id.invoiceid, opt => opt.Ignore())
                .ForMember(d => d.createdat, opt => opt.Ignore())
                .ForMember(d => d.updatedat, d => d.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.invoiceitems, opt => opt.Ignore());
        }
    }
}
