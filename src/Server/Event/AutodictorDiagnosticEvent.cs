using System.Collections.Generic;
using WCFCis2AvtodictorContract.DataContract;

namespace Server.Event
{
    public class AutodictorDiagnosticEvent
    {
        public string NameRailwayStation { get; set; }   

        public ICollection<DiagnosticData> DiagnosticData { get; set; }
    }
}