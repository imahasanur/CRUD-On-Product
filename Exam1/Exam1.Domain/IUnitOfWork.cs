using System;
using System.Collections.Generic;
using System.Text;

namespace Exam1.Domain
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        void Save();
        Task SaveAsync();
    }
}
