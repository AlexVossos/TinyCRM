using System;
using System.Linq;
using TinyCrm.Core.Data;
using TinyCrm.Core.Model;
using TinyCrm.Core.Services.Options;

namespace TinyCrm.Core.Services
{
    public class OrderService:IOrderService
    {
        private TinyCrmDbContext context_;
        private ICustomerService customerService_;

        public OrderService(TinyCrmDbContext context, ICustomerService customerService)
        {
            context_ = context;
            customerService_ = customerService;
        }

        public Order CreateOrder(CreateOrderOptions options)
        {
            if (options == null)
            {
                return null;
            }
            
            var customer = customerService_.SearchCustomers(new SearchCustomerOptions()
            {
                CustomerId = options.CustomerId
            }).SingleOrDefault();

            if (customer == null)
            {
                return null;
            }

            var order = new Order();
            foreach(var productId in options.ProductIds)
            {
                var orderProduct = new OrderProduct()
                {
                    ProductId = productId,
                    OrderId = order.OrderId,
                };
                order.OrderProducts.Add(orderProduct);

            }
            customer.Orders.Add(order);
            context_.Add(customer);
            if (context_.SaveChanges() > 0)
            {
                return order;
            }
            return null;
        }

        public IQueryable<Order> SearchOrders(SearchOrderOptions options)
        {
            if (options == null)
            {
                return null;
            }

            var query = context_
                .Set<Order>()
                .AsQueryable();

            if (options.OrderId != null)
            {
                query = query.Where(c => c.OrderId == options.OrderId.Value);
            }
            if (options.CustomerId != null)
            {                
                var customer = customerService_.SearchCustomers(new SearchCustomerOptions()
                {
                    CustomerId = options.CustomerId
                }).SingleOrDefault();
                query = query.Where(c => customer.Orders.Contains(c));
            }

            //if (options.ProductIds.Any())
            //{                
            //    query = query.Where(c => !options.ProductIds.Except(c.OrderProducts.Pr).Any());
            //}

            query = query.Take(500);

            return query;
        }

        public Order GetOrderById(int? orderId)
        {
            if (orderId==null)
            {
                return null;
            }

            return SearchOrders(new SearchOrderOptions()
            {
                OrderId = orderId
            }).SingleOrDefault();
        }

        public bool UpdateOrder(UpdateOrderOptions options)
        {
            if (options == null)
            {
                return false;
            }

            var order = GetOrderById(options.OrderId);

            if (order == null)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(options.DeliveryAddress))
            {
                order.DeliveryAddress = options.DeliveryAddress;
            }        

            if (context_.SaveChanges() > 0)
            {
                return true;
            }
            return false;

        }
    }
}
