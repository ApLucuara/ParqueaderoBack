using Parqueadero.Domain.Entities;

namespace Parqueadero.Domain.Ports;
public interface IUnitOfWork
{
    Task SaveAsync(CancellationToken? cancellationToken = null);
}
