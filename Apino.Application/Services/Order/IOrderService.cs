using Apino.Application.Dtos.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Services.Order
{
    public interface IOrderService
    {
        Task<long> CreateFromCartAsync(long userId, long branchId);
        Task<OrderUserMobileList> GetOrderMobileInfo(long orderId);
        Task<OrderPaymentDto> GetOrderForPaymentAsync(long orderId);

        Task MarkAsPaidAsync(long orderId, long paymentTypeId, string transactionCode);
        Task MarkAsPaidAsync(long orderId, string transactionCode);
        Task DecreaseProductStockAsync(long orderId);

        Task<List<BranchOrderListDto>> GetOrdersAsync(long branchId);
        Task ChangeStatusAsync(long orderId, long branchId, long userId);



    }
}
