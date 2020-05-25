using System.Linq;
using TinyCrm.Core.Model;
using TinyCrm.Core.Services.Options;

namespace TinyCrm.Core.Services
{
    public interface IOrderService
    {
        Result<Order> CreateOrder(CreateOrderOptions options);
        IQueryable<Order> SearchOrders(SearchOrderOptions options);        
        Result<bool> UpdateOrder(int orderId, UpdateOrderOptions options);
        Result<Order> GetOrderById(int? orderId);
    }
}
