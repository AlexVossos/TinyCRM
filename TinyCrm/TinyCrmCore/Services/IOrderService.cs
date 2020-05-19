using System.Linq;
using TinyCrm.Core.Model;
using TinyCrm.Core.Services.Options;

namespace TinyCrm.Core.Services
{
    public interface IOrderService
    {
        Order CreateOrder(CreateOrderOptions options);
        IQueryable<Order> SearchOrders(SearchOrderOptions options);        
        bool UpdateOrder(UpdateOrderOptions options);
        Order GetOrderById(int? orderId);
    }
}
