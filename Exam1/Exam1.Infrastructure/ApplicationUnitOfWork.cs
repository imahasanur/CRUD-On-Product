﻿using Exam1.Application;
using Exam1.Domain.Repositories;
using Exam1.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam1.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        
        public IProductRepository ProductRepository { get; private set; }
        public ApplicationUnitOfWork(IProductRepository productRepository,IApplicationDbContext dbContext) : base((DbContext)dbContext)
        {
            //ProductRepository = new ProductRepository(dbContext);
            ProductRepository = productRepository;
        }

        
    }
}
