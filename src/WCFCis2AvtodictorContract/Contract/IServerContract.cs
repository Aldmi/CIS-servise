using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using WCFCis2AvtodictorContract.DataContract;

namespace WCFCis2AvtodictorContract.Contract
{
    [ServiceContract]
    public interface IServerContract
    {
        //private GenericRepository<Station> _stationRepository;
        //private GenericRepository<RegulatorySchedule> _regulatoryScheduleRepository;
        //private GenericRepository<OperativeSchedule> _operativeScheduleRepository;
        //private GenericRepository<RailwayStation> _railwayStationRepository;
        //private GenericRepository<Diagnostic> _diagnosticRepository;
        //private GenericRepository<Info> _infoRepository;

        [OperationContract]
        Task<ICollection<StationsData>> GetStations(int? count= null);

        [OperationContract]
        Task<ICollection<RegulatoryScheduleData>> GetRegulatorySchedules(int? count = null);

        [OperationContract]
        Task<ICollection<OperativeScheduleData>> GetOperativeSchedules(int? count = null);

        [OperationContract]
        Task<ICollection<RailwayStationData>> GetRailwayStations(int? count = null);

        [OperationContract]
        Task<ICollection<DiagnosticData>> GetDiagnostics(int? count = null);

        [OperationContract]
        Task<ICollection<InfoData>> GetInfos(int? count = null);

    }
}