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


        public UnitOfWork(CisDbContext context)
        {
            _context = context;
        }



        public IRepository<Station> StationRepository => _stationRepository ?? (_stationRepository = new GenericRepository<Station>(_context));
        public IRepository<RegulatorySchedule> RegulatoryScheduleRepository => _regulatoryScheduleRepository ?? (_regulatoryScheduleRepository = new GenericRepository<RegulatorySchedule>(_context));
        public IRepository<OperativeSchedule> OperativeScheduleRepository => _operativeScheduleRepository ?? (_operativeScheduleRepository = new GenericRepository<OperativeSchedule>(_context));


        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}