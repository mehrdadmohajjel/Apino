using Apino.Application.Common;
using Apino.Application.Dtos.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Services.Payments
{
    public interface IPaymentReportService
    {
        Task<PagedResult<PaymentReportItemDto>> GetBranchPaymentsAsync(long branchId, string fromDate, string toDate, int page = 1, int pageSize = 10);
    }
}
