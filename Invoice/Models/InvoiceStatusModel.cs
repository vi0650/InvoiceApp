using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoice.Models
{
    public class InvoiceStatusModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int invoicestatusid { get; set; }
        public string? status { get; set; }
        public string? text { get; set; }
        public string? updatedat { get; set; }
    }
}
