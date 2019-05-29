using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.API.Models;
using ECommerce.ProductCatalog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductCatalogService _catalogService;
        public ProductsController()
        {
            _catalogService = ServiceProxy.Create<IProductCatalogService>(
                new Uri("fabric:/ECommerce/ECommerce.ProductCatalog"), new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(0));
        }
        // GET api/product
        [HttpGet]
        public async Task<IEnumerable<ApiProduct>> Get()
        {
            IEnumerable<Product> allproducts = await _catalogService.GetAllProducts();

            return allproducts.Select(p => new ApiProduct
            {
                Id = p.Id,
                Description = p.Description,
                Name = p.Name,
                Price = p.Price,
                isAvailable = p.Availability > 0
            });
        }

        [HttpPost]
        public async Task Post([FromBody] ApiProduct product)
        {
            var newprod = new Product
            {
                Id = Guid.NewGuid(),
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Availability = 100
            };
            await _catalogService.AddProduct(newprod);
        }
    }
}