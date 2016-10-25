using System.Collections.Generic;
using System.ServiceModel;
using WCFCis2AvtodictorContract.DataContract;

namespace WCFCis2AvtodictorContract.Contract
{
    [ServiceContract]
    public interface IServerContract
    {
        [OperationContract]
        ICollection<StationsData> GetStations(int count);

        //[OperationContract]
        //ICollection<OperativeScheduleData> GetOperativeSchedules(int count);
    }
}