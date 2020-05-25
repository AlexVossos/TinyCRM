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

        public Result<Order> CreateOrder(CreateOrderOptions options)
        {
            if (options == null)
            {
                return Result<Order>.CreateFailed(
                    StatusCode.BadRequest, "Null options");
            }
            
            var customer = customerService_.SearchCustomers(new SearchCustomerOptions()
            {
                CustomerId = options.CustomerId
            }).SingleOrDefault();

            if (customer == null)
            {
                return Result<Order>.CreateFailed(
                    StatusCode.NotFound, $"Customer with id {options.CustomerId} was not found");
            }

            if (options.ProductIds == null)
            {
                return Result<Order>.CreateFailed(
                    StatusCode.NotFound, $"No products have been added to the order");
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
            var rows = 0;

            try
            {
                rows = context_.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result<Order>.CreateFailed(
                    StatusCode.InternalServerError, ex.ToString());
            }

            if (rows <= 0)
            {
                return Result<Order>.CreateFailed(
                    StatusCode.InternalServerError,
                    "Order could not be created");
            }

            return Result<Order>.CreateSuccessful(order);           
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

        public Result<Order> GetOrderById(int? orderId)
        {
            var order = SearchOrders(new SearchOrderOptions()
            {
                OrderId = orderId
            }).SingleOrDefault();

            if (order == null)
            {
                return Result<Order>.CreateFailed(
                    StatusCode.NotFound, $"Order with id {orderId} was not found");
            }

            return Result<Order>.CreateSuccessful(order);
        }

        public Result<bool> UpdateOrder(int orderId, UpdateOrderOptions options)
        {
            var result = new Result<bool>();

            if (options == null)
            {
                result.ErrorCode = StatusCode.BadRequest;
                result.ErrorText = "Null options";

                return result;
            }

            var order = GetOrderById(orderId).Data;

            if (order == null)
            {
                result.ErrorCode = StatusCode.NotFound;
                result.ErrorText = $"Order with id {orderId} was not found";

                return result;
            }

            if (!string.IsNullOrWhiteSpace(options.DeliveryAddress))
            {
                order.DeliveryAddress = options.DeliveryAddress;
            }

            if (context_.SaveChanges() == 0)
            {
                result.ErrorCode = StatusCode.InternalServerError;
                result.ErrorText = $"Customer could not be updated";
                return result;
            }

            result.ErrorCode = StatusCode.OK;
            result.Data = true;
            return result;

        }
    }
}
