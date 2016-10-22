using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface IUnitOfWork
    {
        IRepository<Station> StationRepository { get; }                                 //Станции
        IRepository<RegulatorySchedule> RegulatoryScheduleRepository { get; }           //Регулярное расписание
        IRepository<OperativeSchedule> OperativeScheduleRepository { get; }             //Оперативное расписание
        IRepository<RailwayStation> RailwayStationRepository { get; }                   //Вокзалы

        Task<int> SaveAsync();
    }
}