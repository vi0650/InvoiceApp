namespace Invoice.DTOs.ResponseDTOs
{
    public class DashboardSummaryResDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalInvoices { get; set; }
        public int TotalCustomers { get; set; }
        public decimal OutstandingAmount { get; set; }
    }
}
