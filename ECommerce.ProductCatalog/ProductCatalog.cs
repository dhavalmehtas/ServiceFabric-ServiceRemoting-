using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.ProductCatalog.Models;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace ECommerce.ProductCatalog
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class ProductCatalog : StatefulService, IProductCatalogService
    {
        private  IProductRepository _repo;
        public ProductCatalog(StatefulServiceContext context)
            : base(context)
        { }

        public async Task AddProduct(Product product)
        {
            await _repo.AddProduct(product);
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
           return await _repo.GetAllProducts();
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener(context => this.CreateServiceRemotingListener(context))
            };
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            _repo = new ServiceFabricProductRepository(this.StateManager);

            var prod1 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Ipad",
                Description = "IPad Device",
                Price = 529,
                Availability = 20
            };
            var prod2 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Laptop Dell",
                Description = "Device",
                Price = 480,
                Availability = 20
            };
            var prod3 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Asus Laptop",
                Description = "Device",
                Price = 259,
                Availability = 20
            };
            var prod4 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Acer Laptop",
                Description = "Device",
                Price = 220,
                Availability = 20
            };

            await _repo.AddProduct(prod1);
            await _repo.AddProduct(prod2);
            await _repo.AddProduct(prod3);
            await _repo.AddProduct(prod4);

            IEnumerable<Product> all = await _repo.GetAllProducts();
        }
    }
}
