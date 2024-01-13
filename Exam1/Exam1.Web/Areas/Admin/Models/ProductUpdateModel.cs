using Autofac;
using Exam1.Domain.Entities;
using Exam1.Domain.Features;
//using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Exam1.Web.Areas.Admin.Models
{
    public class ProductUpdateModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, Range(0,100000, ErrorMessage ="Price should be between 0 to 100000")]
        public uint Price { get; set; }
        [Required, Range(0,10000, ErrorMessage ="Weight should be between 0 to 10000")]
        public uint Weight { get; set; }

        private IProductManagementService _productManagementService;
        public ProductUpdateModel() { }
        public ProductUpdateModel(IProductManagementService productManagementService)
        {
            _productManagementService = productManagementService;
        }

        internal void Resolve(ILifetimeScope scope)
        {
            _productManagementService = scope.Resolve<IProductManagementService>();
        }
        public async Task LoadAsync(Guid id)
        {
            Product product = await _productManagementService.GetProductAsync(id);
            if(product != null)
            {
                Id = product.Id;
                Name = product.Name;
                Price = product.Price;
                Weight = product.Weight;
            }
        }
        public async Task UpdateProductAsync()
        {
            if(!string.IsNullOrWhiteSpace(Name) && Price >= 0 && Weight >= 0)
            {
                await _productManagementService.UpdateProductAsync(Id, Name, Price, Weight);
            }
        }


    }
}
