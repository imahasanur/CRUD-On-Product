using Autofac;
using Exam1.Domain.Features;

namespace Exam1.Web.Areas.Admin.Models
{
    public class ProductCreateModel
    {
        private ILifetimeScope _scope;
        private IProductManagementService _productManagementService;
        public string Name { get; set; }
        public uint Price { get; set; }
        public uint Weight { get; set; }

        public ProductCreateModel() { }
        public ProductCreateModel(IProductManagementService productManagementService)
        {
            _productManagementService = productManagementService;
        }

        internal void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _productManagementService = _scope.Resolve<IProductManagementService>();
        }
        internal async Task CreateProductAsync()
        {
            await _productManagementService.CreateProductAsync(Name, Price, Weight);
        }
    }
}
