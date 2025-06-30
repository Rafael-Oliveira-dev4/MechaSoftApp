using MechaSoft.Domain.Model;

namespace MechaSoft.Domain.Core.Interfaces;

public interface ICreateServiceOrderUseCase : IRepository<ServiceOrder>
{
    Task<ServiceOrder> ExecuteAsync(CreateServiceOrderRequest request);
}
