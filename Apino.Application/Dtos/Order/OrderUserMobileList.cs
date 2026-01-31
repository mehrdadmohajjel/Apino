namespace Apino.Application.Dtos.Order
{
    public class OrderUserMobileList
    {
        public string OrderNumber { get; set; }
        public  long UserId { get; set; }
        public  long BranchId { get; set; }
        public  long BranchAdminUserId { get; set; }
        public  long SystemAdminUserId { get; set; }
        public string UserMobile { get; set; }
        public string BranchAdminMobile { get; set; }
        public string SysAdmimMobile { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TrackingNumber { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
