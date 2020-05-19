using System.Linq;
using TinyCrm.Core.Model;
using TinyCrm.Core.Services.Options;

namespace TinyCrm.Core.Services
{
    public interface IProductService
    {
        IQueryable<Product> SearchProducts(SearchProductOptions options);
        Product CreateProduct(CreateProductOptions options);
        bool UpdateProduct(UpdateProductOptions options);
        Product GetProductById(string productId);
    }
}
