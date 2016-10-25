﻿using System.Data.Entity;
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


        public void UndoChanges()
        {
            foreach(var entry in _context.ChangeTracker.Entries())
            {
                switch(entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;

                    case EntityState.Deleted:
                        entry.Reload();
                        break;

                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}