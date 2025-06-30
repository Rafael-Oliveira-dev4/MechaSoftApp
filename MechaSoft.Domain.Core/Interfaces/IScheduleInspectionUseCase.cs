using MechaSoft.Domain.Model;

namespace MechaSoft.Domain.Core.Interfaces;

public interface IScheduleInspectionUseCase : IRepository<Inspection>
{
    Task<Inspection> ExecuteAsync(ScheduleInspectionRequest request);
}