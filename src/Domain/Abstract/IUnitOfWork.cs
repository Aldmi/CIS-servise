using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface IUnitOfWork
    {
        IRepository<Station> StationRepository { get; }
        IRepository<RegulatorySchedule> RegulatoryScheduleRepository { get; }
        IRepository<OperativeSchedule> OperativeScheduleRepository { get; }

        Task<int> SaveAsync();
    }
}