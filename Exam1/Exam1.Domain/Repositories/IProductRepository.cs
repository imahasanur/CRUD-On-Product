using Exam1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam1.Domain.Repositories
{
    public interface IProductRepository: IRepositoryBase<Product,Guid>
    {
        Task<bool> IsNameDuplicateAsync(string name, Guid? id= null);

        Task<(IList<Product> records, int total, int totalDisplay)>
            GetTableDataAsync(string searchName, uint searchPriceFrom, uint searchPriceTo, string orderBy, int pageIndex, int pageSize);


    }
}
