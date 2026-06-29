using System;
using System.Collections.Generic;
using System.Text;

namespace MeatData.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
