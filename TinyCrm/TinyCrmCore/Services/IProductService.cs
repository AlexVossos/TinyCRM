using System.Linq;
using TinyCrm.Core.Model;
using TinyCrm.Core.Services.Options;

namespace TinyCrm.Core.Services
{
    public interface IProductService
    {
        IQueryable<Product> SearchProducts(SearchProductOptions options);
        Result<Product> CreateProduct(CreateProductOptions options);
        Result<bool> UpdateProduct(string productId, UpdateProductOptions options);
        Result<Product> GetProductById(string productId);
    }
}
