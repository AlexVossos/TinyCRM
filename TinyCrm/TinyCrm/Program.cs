using System.Linq;
using TinyCrm.Core.Data;
using TinyCrm.Core.Services;
using TinyCrm.Core.Services.Options;

namespace TinyCrm
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new TinyCrmDbContext())
            {
                ICustomerService customerService = new CustomerService(context);

                var customer = customerService.CreateCustomer(
                    new CreateCustomerOptions()
                    {
                        Firstname = "Dimitris",
                        LastName = "Pneumatikos",
                        Vatnumber = "123456789"
                    });

                //IOrderService orderService = new OrderService(context, customerService);
                //var results = customerService.SearchCustomers(new SearchCustomerOptions()
                //{
                //    CustomerId = 3
                //}).SingleOrDefault();

            }

           
        }

       
    }
}
