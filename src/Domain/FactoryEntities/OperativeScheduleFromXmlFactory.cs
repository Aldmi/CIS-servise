using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Domain.Entities;


namespace Domain.FactoryEntities
{
    public static class OperativeScheduleFromXmlFactory
    {
        #region Methode

        public static List<OperativeSchedule> LoadXmlSetting(XElement xml, IEnumerable<Station> stations)
        {
            var operativeSchedules= new List<OperativeSchedule>();

            var xElements = xml?.Element("Cashiers")?.Elements("Cashier");
            if (xElements != null)
            {
                foreach (var el in xElements)
                {
                    int ecpCodeDispatchStation = int.Parse((string) el.Attribute("Станция отправления"));
                    int ecpCodeStationOfDestination = int.Parse((string)el.Attribute("Станция назначения"));

                    var dispatchStation= stations.FirstOrDefault(st => st.EcpCode == ecpCodeStationOfDestination) ?? new Station { EcpCode = ecpCodeDispatchStation };
                    var stationOfDestination = stations.FirstOrDefault(st => st.EcpCode == ecpCodeStationOfDestination) ?? new Station { EcpCode = ecpCodeStationOfDestination };


                    var op= new OperativeSchedule {
                        RouteName = (string) el.Attribute("Id"),
                        ArrivalTime = DateTime.Parse((string)el.Attribute("ArrivalTime")),
                        DepartureTime = DateTime.Parse((string)el.Attribute("DepartureTime")),
                        NumberOfTrain = (int) el.Attribute("NumberOfTrain"),
                        DispatchStation = dispatchStation,
                        StationOfDestination = stationOfDestination
                    };

                    operativeSchedules.Add(op);
                }
            }

            return operativeSchedules;
        }

        #endregion

    }
}