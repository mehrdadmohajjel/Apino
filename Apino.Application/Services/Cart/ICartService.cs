using Apino.Application.Dtos;
using Apino.Application.Dtos.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Services.Cart
{
    public interface ICartService
    {
        Task AddAsync(long userId, long branchId, long productId, int qty);
        Task<Apino.Domain.Entities.Cart> GetActiveCartAsync(long userId, long branchId);
        Task MergeGuestAsync(long userId, List<GuestCartItem> items);
        Task ClearAsync(long cartId);
        Task<CartViewModel> GetCartAsync(long userId);
        Task<int> GetCartItemCountAsync(long userId);


    }
}
