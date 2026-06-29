using MeatData.Application.Interfaces;

namespace MeatData.Infrastructure.Persistence;

// UnitOfWork é só um wrapper sobre o SaveChangesAsync do DbContext
// No Spring Boot o @Transactional faz isso implicitamente
// Aqui a vantagem é que o Application layer não precisa conhecer o DbContext
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context) => _context = context;

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);
}
