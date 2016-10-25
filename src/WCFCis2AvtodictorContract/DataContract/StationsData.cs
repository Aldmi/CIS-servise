using System.Collections.Generic;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace WCFCis2AvtodictorContract.DataContract
{
    [DataContract]
    public class StationsData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int EcpCode { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        //public ICollection<OperativeScheduleData> OperativeSchedulesListOfStops { get; set; }      

        //public ICollection<OperativeSchedule> OperativeSchedulesListWithoutStops { get; set; } 

        //public ICollection<RailwayStation> RailwayStations { get; set; }
    }
}