using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Domain.Abstract;
using WCFCis2AvtodictorContract.Contract;
using WCFCis2AvtodictorContract.DataContract;

namespace Server.HostWCF
{

    [ServiceBehavior(InstanceContextMode= InstanceContextMode.PerSession)]
    public class CisServise : IServerContract
    {
        private readonly IUnitOfWork _unitOfWork;

        public CisServise(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




        public ICollection<StationsData> GetStations(int count)
        {
            return  _unitOfWork.StationRepository.Get().ToList().Select(st=> new StationsData {Description = st.Description, Id = st.Id, Name = st.Name, EcpCode = st.EcpCode}).ToList();
        }

        //TODO: добавить методы "получить N данных вокзала по id вокзала: станции, расписание,..."
        //TODO: добавить методы "получить N данных из какой-то таблицы: таблица станций, таблица расписаний, ..."
        //TODO: добавить методы "передать данные об состояния автодиктора"

        //public ICollection<OperativeScheduleData> GetOperativeSchedules(int count)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}