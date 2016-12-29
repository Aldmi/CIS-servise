using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.DbContext;
using Domain.Entities;

namespace Domain.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CisDbContext _context;
        private GenericRepository<Station> _stationRepository;
        private GenericRepository<RegulatorySchedule> _regulatoryScheduleRepository;
        private GenericRepository<OperativeSchedule> _operativeScheduleRepository;
        private GenericRepository<RailwayStation> _railwayStationRepository;
        private GenericRepository<Diagnostic> _diagnosticRepository;
        private GenericRepository<Info> _infoRepository;



        public UnitOfWork(CisDbContext context)
        {
            _context = context;
        }



        public IRepository<Station> StationRepository => _stationRepository ?? (_stationRepository = new GenericRepository<Station>(_context));
        public IRepository<RegulatorySchedule> RegulatoryScheduleRepository => _regulatoryScheduleRepository ?? (_regulatoryScheduleRepository = new GenericRepository<RegulatorySchedule>(_context));
        public IRepository<OperativeSchedule> OperativeScheduleRepository => _operativeScheduleRepository ?? (_operativeScheduleRepository = new GenericRepository<OperativeSchedule>(_context));
        public IRepository<RailwayStation> RailwayStationRepository => _railwayStationRepository ?? (_railwayStationRepository = new GenericRepository<RailwayStation>(_context));
        public IRepository<Diagnostic> DiagnosticRepository => _diagnosticRepository ?? (_diagnosticRepository = new GenericRepository<Diagnostic>(_context));
        public IRepository<Info> InfoRepository => _infoRepository ?? (_infoRepository = new GenericRepository<Info>(_context));



        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }


        public int Save()
        {
           return _context.SaveChanges();
        }



        public void UndoChanges()
        {
            foreach (var entity in _context.ChangeTracker.Entries())
            {
                entity.Reload();
            }
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}