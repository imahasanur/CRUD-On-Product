using Exam1.Domain.Entities;
using Exam1.Domain.Exceptions;
using Exam1.Domain.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam1.Application.Features
{
    public class ProductManagementService:IProductManagementService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public ProductManagementService(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CreateProductAsync(string name, uint price, uint weight)
        {
            bool isDuplicateName = await _unitOfWork.ProductRepository.IsNameDuplicateAsync(name);
            if (isDuplicateName)
            {
                throw new DuplicateNameException();
            }
            Product product = new Product
            {
                Name = name,
                Price = price,
                Weight = weight
            };
            _unitOfWork.ProductRepository.Add(product);
            _unitOfWork.SaveAsync();

        }



        public async Task<(IList<Product> records, int total, int totalDisplay)>
            GetPagedProductsAsync(int pageIndex, int pageSize, string searchName, uint searchPriceFrom, uint searchPriceTo, string sortBy)
        {
            return await _unitOfWork.ProductRepository.GetTableDataAsync(searchName, searchPriceFrom, searchPriceTo, sortBy, pageIndex, pageSize);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            await _unitOfWork.ProductRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateProductAsync(Guid id, string name, uint price, uint weight)
        {
            bool isDuplicateName = await _unitOfWork.ProductRepository.IsNameDuplicateAsync(name, id);
            if (isDuplicateName)
            {
                throw new DuplicateNameException();
            }
            var product = await GetProductAsync(id);
            if(product is not null)
            {
                product.Name = name;
                product.Price = price;
                product.Weight = weight;
            }
            await _unitOfWork.SaveAsync();
        }

        public async Task<Product> GetProductAsync(Guid id)
        {
            return await _unitOfWork.ProductRepository.GetByIdAsync(id);
        }
    }
}
