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

        public Result<Product> CreateProduct(CreateProductOptions options)
        {
            if (options == null)
            {
                return Result<Product>.CreateFailed(
                    StatusCode.BadRequest, "Null options");
            }

            var product = new Product()
            {
                Category = options.Category,
                Description = options.Description,
                Name = options.Name,
                Price = options.Price,
            };
            context_.Add(product);

            var rows = 0;

            try
            {
                rows = context_.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result<Product>.CreateFailed(
                    StatusCode.InternalServerError, ex.ToString());
            }

            if (rows <= 0)
            {
                return Result<Product>.CreateFailed(
                    StatusCode.InternalServerError,
                    "Product could not be created");
            }

            return Result<Product>.CreateSuccessful(product);
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

        public Result<Product> GetProductById(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                return Result<Product>.CreateFailed(
                    StatusCode.BadRequest, "No productId given");
            }

            var product= SearchProducts(new SearchProductOptions()
            {
                ProductId = productId
            }).SingleOrDefault();

            if (product == null)
            {
                return Result<Product>.CreateFailed(
                    StatusCode.NotFound, $"Product with id {productId} was not found");
            }

            return Result<Product>.CreateSuccessful(product);
        }

        public Result<bool> UpdateProduct(string productId,UpdateProductOptions options)
        {
            var result = new Result<bool>();

            if (options == null)
            {
                result.ErrorCode = StatusCode.BadRequest;
                result.ErrorText = "Null options";

                return result;
            }

            var product = GetProductById(productId).Data;

            if (product == null)
            {
                result.ErrorCode = StatusCode.NotFound;
                result.ErrorText = $"Product with id {productId} was not found";

                return result;
            }

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
