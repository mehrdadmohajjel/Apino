namespace Apino.Application.Dtos.Order
{
    public class OrderPaymentDto
    {
        public long OrderId { get; set; }
        public long BranchId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public long TrackingNumber { get; set; }
    }
}
