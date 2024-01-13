using Exam1.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exam1.Infrastructure
{
    public interface IApplicationDbContext
    {
        //DbSet<Course> Courses { get; set; }
        DbSet<Product> Products { get; set; }
    }
}