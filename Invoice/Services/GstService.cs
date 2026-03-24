using AutoMapper;
using Invoice.DTOs.CreateDTOs;
using Invoice.DTOs.ResponseDTOs;
using Invoice.DTOs.UpdateDTOs;
using Invoice.Models;
using Invoice.Repository.IRepository;
using Invoice.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Services
{
    public class GstService : IGstService
    {

        private readonly IGstRepo repo;
        private readonly IMapper mapper;

        public GstService(IGstRepo repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<Response<IEnumerable<GstResDTO>>> GetGst()
        {
            var gst = await repo.GetAllGst();
            if (!gst.Any())
            {
                return Response<IEnumerable<GstResDTO>>.NotFound();
            }
            var res = mapper.Map<IEnumerable<GstResDTO>>(gst);
            return Response<IEnumerable<GstResDTO>>.Ok(res, "Gst Data retrieved successfully");
        }

        public async Task<Response<GstResDTO>> GetGstById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Response<GstResDTO>.BadRequest("Invalid gst id");
            }
            var gst = await repo.GetByIdAsync(id);

            if (gst is null)
            {
                return Response<GstResDTO>.NotFound($"Gst of Id {id} does not exist");
            }

            var dto = mapper.Map<GstResDTO>(gst);

            return Response<GstResDTO>.Ok(dto, "Record retrieved successfully");
        }

        public async Task<Response<IEnumerable<GstResDTO>>> GetGstByUserId(Guid id)
        {
            var product = await repo.GetGstByUser(id);
            if (!product.Any())
            {
                return Response<IEnumerable<GstResDTO>>.NotFound();
            }
            var res = mapper.Map<IEnumerable<GstResDTO>>(product);
            return Response<IEnumerable<GstResDTO>>.Ok(res, "Gst Data retrieved successfully");
        }

        public async Task<Response<GstResDTO>> CreateGst(CreateGstDTO createDTO)
        {
            if (createDTO == null)
            {
                return Response<GstResDTO>.BadRequest("Gst data is required");
            }
            if (await repo.IsGstExistAsync(createDTO.Gst))
            {
                return Response<GstResDTO>.Conflict("Gst is already exists");
            }

            var model = mapper.Map<GstModel>(createDTO);
            model.rateid = Guid.NewGuid();
            model.createdat = DateTime.UtcNow;
            await repo.CreateAsync(model);
            var createProduct = mapper.Map<GstResDTO>(model);
            return Response<GstResDTO>.Ok(createProduct, "Gst Created Successfully");
        }

        public async Task<Response<GstResDTO>> UpdateGst(Guid id, UpdateGstDTO updateDTO)
        {
            if (id == Guid.Empty || id != updateDTO.RateId)
            {
                return Response<GstResDTO>.BadRequest("Gst id does not match to entered records with request body");
            }
            var existingGst = await repo.GetByIdAsync(updateDTO.RateId);
            if (existingGst is null)
            {
                return Response<GstResDTO>.NotFound($"Gst with id {id} does not exist in Records");
            }
            if (await repo.IsDataExistAsync(updateDTO.Gst, id))
            {
                return Response<GstResDTO>.Conflict($"{updateDTO.Gst} is already Taken");
            }
            mapper.Map(updateDTO, existingGst);
            existingGst.updatedat = DateTime.UtcNow;
            await repo.UpdateAsync(existingGst);
            var updateUser = mapper.Map<GstResDTO>(existingGst);
            return Response<GstResDTO>.Ok(updateUser, "Gst Updated Successfully");
        }

        public async Task<Response<object>> DeleteGst(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return Response<object>.NotFound("Invalid Gst id");
                }
                var existingGst = await repo.GetByIdAsync(id);
                if (existingGst is null)
                {
                    return Response<object>.NotFound($"Gst with id {id} does not exist in Records");
                }

                await repo.RemoveAsync(existingGst);
                var res = mapper.Map<GstResDTO>(existingGst);
                return Response<object>.Ok(null, "Gst Deleted Successfully...");
            }
            catch (DbUpdateException)
            {
                return Response<object>.Error(409, "Gst has invoices. Delete invoices first.");
            }
        }
    }
}
