using MechaSoft.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechaSoft.Domain.Core.UseCases;

public class CreateServiceOrderRequest
{
    public Guid CustomerId { get; set; }
    public Guid VehicleId { get; set; }
    public string Description { get; set; } = null!;
    public Priority Priority { get; set; }
    public List<ServiceItemRequest> Services { get; set; } = [];
    public List<PartItemRequest> Parts { get; set; } = [];
    public Guid? MechanicId { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
}

public interface ICreateServiceOrderUseCase
{
    Task<ServiceOrder> ExecuteAsync(CreateServiceOrderRequest request);
}