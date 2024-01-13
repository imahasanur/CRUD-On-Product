using Exam1.Domain.Entities;
using Exam1.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Exam1.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product, Guid>, IProductRepository
    {
        public ProductRepository(IApplicationDbContext context) : base((DbContext)context)
        {
        }
        public async Task<bool> IsNameDuplicateAsync(string name, Guid? id = null)
        {
            if (id.HasValue)
            {
                return (await GetCountAsync(x => x.Id != id.Value && x.Name == name)) > 0;
            }
            else
            {
                return (await GetCountAsync(x => x.Name == name)) > 0;
            }
        }

        public async Task<(IList<Product> records, int total, int totalDisplay)>
            GetTableDataAsync(string searchName, uint searchPriceFrom, uint searchPriceTo, string orderBy, int pageIndex, int pageSize)
        {
            Expression<Func<Product, bool>> expression = null;
            if (!string.IsNullOrWhiteSpace(searchName))
            {
                expression = x => x.Name.Contains(searchName) && (x.Price >= searchPriceFrom && x.Price <= searchPriceTo);
            }
            
            return await GetDynamicAsync(expression,
                orderBy, null, pageIndex, pageSize, true);
        }
    }
}
