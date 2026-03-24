using AutoMapper;
using Invoice.DTOs.BaseDTOs;
using Invoice.DTOs.CreateDTOs;
using Invoice.Models;
using Invoice.Repository.IRepository;
using Invoice.Services.IServices;

namespace Invoice.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepo repo;
        private readonly IMapper mapper;

        public InvoiceService(IInvoiceRepo repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<Response<IEnumerable<InvoiceResDTO>>> GetInvoice()
        {
            var invoices = await repo.GetAllAsync();
            if (!invoices.Any())
            {
                return Response<IEnumerable<InvoiceResDTO>>.NotFound("No Records Found");
            }
            var res = mapper.Map<IEnumerable<InvoiceResDTO>>(invoices);
            return Response<IEnumerable<InvoiceResDTO>>.Ok(res, "Invoice Data retrieved successfully");
        }

        public async Task<Response<IEnumerable<InvoiceResDTO>>> GetInvoiceByUser(Guid id)
        {
            var invoices = await repo.GetInvoiceByUser(id);
            if (!invoices.Any())
            {
                return Response<IEnumerable<InvoiceResDTO>>.NotFound("No Records Found");
            }
            var res = mapper.Map<IEnumerable<InvoiceResDTO>>(invoices);
            return Response<IEnumerable<InvoiceResDTO>>.Ok(res, "Invoice Data retrieved successfully");
        }

        public async Task<Response<InvoiceResDTO>> GetInvoiceById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Response<InvoiceResDTO>.BadRequest("Invalid Invoice id");
            }
            var invoice = await repo.GetInvoiceByIdAsync(id);

            if (invoice is null)
            {
                return Response<InvoiceResDTO>.NotFound($"Invoice of Id {id} does not exist");
            }

            var invoiceRes = mapper.Map<InvoiceResDTO>(invoice);

            return Response<InvoiceResDTO>.Ok(invoiceRes, "Record retrieved successfully");
        }

        public async Task<Response<InvoiceResDTO>> CreateInvoice(CreateInvoiceDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return Response<InvoiceResDTO>.BadRequest("Invoice data is required");
                }
                var Invoice = mapper.Map<InvoiceModel>(createDTO);
                await repo.CreateAsync(Invoice);
                await repo.SaveAsync();
                var getInvoice = await repo.GetInvoiceByIdAsync(Invoice.invoiceid);
                var createdInvoice = mapper.Map<InvoiceResDTO>(getInvoice);
                return Response<InvoiceResDTO>.Ok(createdInvoice, "Invoice Created Successfully");
            }
            catch (Exception e)
            { 
                throw;
            }
        }   

        public async Task<Response<InvoiceResDTO>> UpdateInvoice(Guid id, UpdateInvoiceDTO updateDTO)
        {
            if (id == Guid.Empty)
            {
                return Response<InvoiceResDTO>.BadRequest("Invoice id does not match to entered records with request body");
            }
            var existingInvoice = await repo.GetInvoiceWithItemsAsync(id);
            if (existingInvoice is null)
            {
                return Response<InvoiceResDTO>.NotFound($"Invoice with id {id} does not exist in Records");
            }
            //if (await repo.IsInvoiceExistAsync(id))
            //{
            //    return Response<InvoiceResDTO>.Conflict($"{id} is already Taken");
            //}

            existingInvoice.customerid = updateDTO.CustomerId;
            existingInvoice.customername = updateDTO.CustomerName;
            existingInvoice.userid = updateDTO.UserId;
            existingInvoice.email = updateDTO.Email;
            existingInvoice.mobileno = updateDTO.MobileNo;
            existingInvoice.address = updateDTO.Address;
            existingInvoice.invoicedate = updateDTO.InvoiceDate;
            existingInvoice.description = updateDTO.Description;
            existingInvoice.invoicestatus = updateDTO.InvoiceStatus;
            existingInvoice.subtotal = updateDTO.SubTotal;
            existingInvoice.gstamount = updateDTO.GstAmount;
            existingInvoice.grandtotal = updateDTO.GrandTotal;

            var removedItems = existingInvoice.invoiceitems
                .Where(e => !updateDTO.InvoiceItems
                .Any(dtoItem => dtoItem.InvoiceItemId == e.invoiceitemid))
                .ToList();

            foreach (var removed in removedItems)
            {
                existingInvoice.invoiceitems.Remove(removed);
                repo.RemoveInvoiceItemAsync(removed);
            }

            foreach (var item in updateDTO.InvoiceItems)
            {
                if (item.InvoiceItemId.HasValue)
                {
                    var existingItem = existingInvoice.invoiceitems.First(i => i.invoiceitemid == item.InvoiceItemId);
                    existingItem.productid = item.ProductId;
                    existingItem.productname = item.ProductName;
                    existingItem.rate = item.Rate;
                    existingItem.qty = item.Qty;
                    existingItem.rateid = item.RateId;
                    existingItem.gst = item.Gst;
                    existingItem.amount = item.Amount;
                }
                else
                {
                    existingInvoice.invoiceitems.Add(new InvoiceItemsModel {
                        invoiceitemid = Guid.NewGuid(),
                        productid = item.ProductId,
                        productname = item.ProductName,
                        rate = item.Rate,
                        qty = item.Qty,
                        rateid = item.RateId,
                        gst = item.Gst,
                        amount = item.Amount,
                    });
                }
            }
            existingInvoice.updatedat = DateTime.UtcNow;
            repo.UpdateAsync(existingInvoice);
            await repo.SaveAsync();
            var updateInvoice = mapper.Map<InvoiceResDTO>(existingInvoice);
            return Response<InvoiceResDTO>.Ok(updateInvoice, "Invoice Updated Successfully");
        }

        public async Task<Response<object>> DeleteInvoice(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Response<object>.NotFound("Invalid Invoice id");
            }
            var existingInvoice = await repo.GetByIdAsync(id);
            if (existingInvoice is null)
            {
                return Response<object>.NotFound($"Invoice with id {id} does not exist in Records");
            }
            repo.RemoveAsync(existingInvoice);
            await repo.SaveAsync();
            var res = mapper.Map<object>(existingInvoice);
            return Response<object>.Ok(null, "Invoice Deleted Successfully...");
        }
    }
}
