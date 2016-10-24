using System.Collections.Generic;
using Domain.Abstract;
using WCFCis2AvtodictorContract.Contract;
using WCFCis2AvtodictorContract.DataContract;

namespace WCFCis2AvtodictorService
{
    public class CisServise : IServerContract
    {
        private readonly IUnitOfWork _unitOfWork;




        public CisServise(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




        public ICollection<StationsData> GetStations(int count)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<OperativeScheduleData> GetOperativeSchedules(int count)
        {
            throw new System.NotImplementedException();
        }
    }
}