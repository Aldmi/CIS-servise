using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Domain.Entities;


namespace Domain.FactoryEntities
{
    public class OperativeScheduleFactory
    {
        #region Methode

        public static List<OperativeSchedule> LoadXmlSetting(XElement xml, IEnumerable<Station> station)
        {
            var operativeSchedules= new List<OperativeSchedule>();

            var xElements = xml?.Element("Cashiers")?.Elements("Cashier");
            if (xElements != null)
            {
                foreach (var el in xElements)
                {
                    var op= new OperativeSchedule {
                        RouteName = (string) el.Attribute("Id"),
                        ArrivalTime = DateTime.Parse((string)el.Attribute("ArrivalTime")),
                        DepartureTime = DateTime.Parse((string)el.Attribute("DepartureTime")),
                        NumberOfTrain = (int) el.Attribute("NumberOfTrain"),
                        DispatchStation = station.First(st => st.Name.Equals((string)el.Attribute("Станция отпарвления"))),
                        StationOfDestination = station.First(st => st.Name.Equals((string)el.Attribute("Станция назначения")))
                    };

                    operativeSchedules.Add(op);
                }
            }

            return operativeSchedules;
        }

        #endregion

    }
}