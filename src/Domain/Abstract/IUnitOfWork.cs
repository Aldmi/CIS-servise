using System;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Station> StationRepository { get; }                                 //Станции

        IRepository<RegulatorySchedule> RegulatoryScheduleRepository { get; }          //Регулярное расписание

        IRepository<OperativeSchedule> OperativeScheduleRepository { get; }             //Оперативное расписание

        IRepository<RailwayStation> RailwayStationRepository { get; }                   //Вокзалы

        IRepository<Diagnostic> DiagnosticRepository { get; }                           //Диагностика оборудования автодиктора

        IRepository<Info> InfoRepository { get; }                                       //Оперативная информация про поезда


        int Save();
        Task<int> SaveAsync();

        void UndoChanges();
    }
}