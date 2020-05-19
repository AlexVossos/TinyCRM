using System.Linq;
using TinyCrm.Core.Data;
using TinyCrm.Core.Model;
using TinyCrm.Core.Services.Options;

namespace TinyCrm.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private TinyCrmDbContext context_;
        public CustomerService(TinyCrmDbContext context)
        {
            context_ = context;
        }

        public Customer CreateCustomer(CreateCustomerOptions options)
        {
            if (options == null)
            {
                return null;
            }

            var customer = new Customer()
            {
                Lastname = options.LastName,
                Firstname = options.Firstname,
                VatNumber = options.Vatnumber
            };
            context_.Add(customer);
            if (context_.SaveChanges() > 0)
            {
                return customer;
            }
            return null;
        }

        public IQueryable<Customer> SearchCustomers(SearchCustomerOptions options)
        {
            if (options == null)
            {
                return null;
            }

            var query = context_
                .Set<Customer>()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(options.Firstname))
            {
                query = query.Where(c => c.Firstname == options.Firstname);
            }

            if (!string.IsNullOrWhiteSpace(options.VatNumber))
            {
                query = query.Where(c => c.VatNumber == options.VatNumber);
            }

            if (options.CustomerId != null)
            {
                query = query.Where(c => c.CustomerId == options.CustomerId.Value);
            }

            if (options.CreateFrom != null)
            {
                query = query.Where(c => c.Created >= options.CreateFrom);
            }

            query = query.Take(500);

            return query;
        }

        public Customer GetCustomerById(int? customerId)
        {
            if (customerId == null)
            {
                return null;
            }

            return SearchCustomers(new SearchCustomerOptions()
            {
                CustomerId = customerId
            }).SingleOrDefault();            
        }

        public bool DeleteCustomer(int? customerId)
        {
            var customer = GetCustomerById(customerId);
            context_.Remove(customer);
            if (context_.SaveChanges() > 0)
            {
                return true;
            }
            return false;

        }

        public bool UpdateCustomer(UpdateCustomerOptions options)
        {
            if (options == null)
            {
                return false;
            }

            var customer = GetCustomerById(options.CustomerId);

            if (customer == null)
            {
                return false;
            }

            if (options.IsActive != null)
            {
                customer.IsActive = options.IsActive;
            }

            if (!string.IsNullOrWhiteSpace(options.Firstname))
            {
                customer.Firstname = options.Firstname;
            }

            if (!string.IsNullOrWhiteSpace(options.LastName))
            {
                customer.Lastname = options.LastName;
            }

            if (options.Vatnumber != null)
            {
                customer.VatNumber = options.Vatnumber;
            }

            if (context_.SaveChanges() > 0)
            {
                return true;
            }
            return false;

        }

    }
}
