using System;
using System.Linq;
using TinyCrm.Core.Data;
using TinyCrm.Core.Model;
using TinyCrm.Core.Services.Options;

namespace TinyCrm.Core.Services
{
    public class ProductService : IProductService
    {
        private TinyCrmDbContext context_;


        public ProductService(TinyCrmDbContext context)
        {
            context_ = context;
        }

        public Product CreateProduct(CreateProductOptions options)
        {
            if (options == null)
            {
                return null;
            }

            var product = new Product()
            {
                Category = options.Category,
                Description = options.Description,
                Name = options.Name,
                Price = options.Price,
            };
            context_.Add(product);
            if (context_.SaveChanges() > 0)
            {
                return product;
            }
            return null;
        }

        public IQueryable<Product> SearchProducts(SearchProductOptions options)
        {
            if (options == null)
            {
                return null;
            }

            var query = context_
                .Set<Product>()
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(options.ProductId))
            {
                query = query.Where(c => c.ProductId == options.ProductId);
            }
            if (!string.IsNullOrWhiteSpace(options.Name))
            {
                query = query.Where(c => c.Name.Contains(options.Name.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(options.Description))
            {
                query = query.Where(c => c.Description.Contains(options.Description.Trim()));
            }
            if (options.Category != null)
            {
                query = query.Where(c => c.Category == options.Category);
            }
            if (options.MinPrice != null)
            {
                query = query.Where(c => c.Price >= options.MinPrice);
            }
            if (options.MaxPrice != null)
            {
                query = query.Where(c => c.Price <= options.MinPrice);
            }

            query = query.Take(500);

            return query;
        }

        public Product GetProductById(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                return null;
            }

            return SearchProducts(new SearchProductOptions()
            {
                ProductId = productId
            }).SingleOrDefault();
        }

        public bool UpdateProduct(UpdateProductOptions options)
        {
            if (options == null)
            {
                return false;
            }

            var product = GetProductById(options.ProductId);

            if (product != null)
            {
                if (options.Category != null)
                {
                    product.Category = options.Category;
                }

                if (!string.IsNullOrWhiteSpace(options.Description))
                {
                    product.Description = options.Description;
                }

                if (!string.IsNullOrWhiteSpace(options.Name))
                {
                    product.Name = options.Name;
                }

                if (options.Price != null)
                {
                    product.Price = options.Price;
                }

            }

            if (context_.SaveChanges() > 0)
            {
                return true;
            }
            return false;

        }
    }
}
