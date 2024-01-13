using Autofac;
using Exam1.Domain.Features;
using Exam1.Infrastructure;
using System.Web;

namespace Exam1.Web.Areas.Admin.Models
{
    public class ProductListModel
    {
        private ILifetimeScope _scope;
        private IProductManagementService _productManagementService;
        public ProductSearch SearchItem { get; set; }
        public ProductListModel() { }

        public ProductListModel(IProductManagementService productManagementService)
        {
            _productManagementService = productManagementService;
        }

        internal void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _productManagementService = _scope.Resolve<IProductManagementService>();
        }

        internal async Task<object> GetPagedProductsAsync(DataTablesAjaxRequestUtility dataTableUtility)
        {
            var data = await _productManagementService.GetPagedProductsAsync(
                dataTableUtility.PageIndex,
                dataTableUtility.PageSize,
                SearchItem.Name,
                SearchItem.ProductPriceFrom,
                SearchItem.ProductPriceTo,
                dataTableUtility.GetSortText(new string[] {"Name","Price","Weight"})
                );
            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                            HttpUtility.HtmlEncode(record.Name),
                            record.Price.ToString(),
                            record.Weight.ToString(),
                            record.Id.ToString()
                        }
                        ).ToArray()
            };
        }

        internal async Task DeleteProductAsync(Guid id)
        {
            await _productManagementService.DeleteProductAsync(id);
        }

    }
}
