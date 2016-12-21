using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using System.Xml;
using System.Xml.Linq;
using Domain.Entities;


namespace Server.ClientSOAP.XmlServices
{

    public class XmlRegularityShService
    {
        /* ЗАПРОС
         <?xmlversion="1.0" encoding=" utf-8" ?>
           <root>
            <Authentication User=’ddd’ Password=’samba’/>
            <Place ESR=’34567’/>
           </root>
         */
        public XDocument GetRequest(Station station)
        {
            var user = "ddd";
            var password = "samba";

            XDocument xDoc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement("root",
                new XElement("Authentication", new XAttribute("User", user), new XAttribute("Password", password)),
                new XElement("Place", new XAttribute("ESR", station.EcpCode))
                ));

            return xDoc;
        }



        /* ОТВЕТ
         <?xml version="1.0" encoding="utf-8" ?>
           <root>
             <Place ESR=’34567’ Data=’2015.09.21’/>
             <Train TrainNumber=’6022’ Name=’Москва-Ожерелье’ ESRDeparture=’34567’ ESRArrival=’34560’ TimeDeparture=’2015.09.21 11:11:00’ TimeArrival=’’DaysTrainType=’ОН’ DaysTrain= ‘2015.09.15;2015.09.17;2015.09.22’>
	            <PlacesStop>
		           <Place ESR=’23451’ />
		           <Place ESR=’23457’ />
	            </PlacesStop>
                <PlacesNotStop>
	            </PlacesNotStop>
              </Train>
             <Train TrainNumber=’6021’ Name=’Ожерелье-МОсква’ ESRDeparture=’34560’ ESRArrival=’ 34567’ TimeDeparture=’’ TimeArrival=’2015.09.21 12:05:00’ DaysTrainType=’Ежедневно’ DaysTrain= ‘’>
	            <PlacesStop>
	            </PlacesStop>
                <PlacesNotStop>
	            </PlacesNotStop>
            </Train>
          </root>
         */
        public IEnumerable<RegulatorySchedule> SetResponse(XDocument xmlDoc, Station stationValid)
        {
            try
            {
                var identQuery = from train in xmlDoc.Descendants("Place")
                                 select new
                                 {
                                     ESR = (string)train.Attribute("ESR"),
                                     Data = (string)train.Attribute("Data")
                                 };

                // сверить идентификатор (для нужной станции расписания)
                var identAnonym = identQuery.FirstOrDefault();
                if (identAnonym != null && identAnonym.ESR != stationValid.EcpCode.ToString())
                    return null;



                var listRegShQuery =
                    from train in xmlDoc.Descendants("Train")
                    select new
                    {
                        TrainNumber = (string)train.Attribute("TrainNumber"),
                        Name = (string)train.Attribute("Name"),
                        ESRDeparture = (string)train.Attribute("ESRDeparture"),
                        ESRArrival = (string)train.Attribute("ESRArrival"),
                        TimeDeparture = (string)train.Attribute("TimeDeparture"),
                        TimeArrival = (string)train.Attribute("TimeArrival"),
                        DaysTrainType = (string)train.Attribute("DaysTrainType"),
                        DaysTrain = (string)train.Attribute("DaysTrain"),
                        PlacesStop =
                            new List<string>(
                                train.Element("PlacesStop")?
                                    .Elements("Place")
                                    .Select(p => (string)p.Attribute("ESR"))
                                    .ToList()),
                        PlacesNotStop =
                            new List<string>(
                                train.Element("PlacesNotStop")?
                                    .Elements("Place")
                                    .Select(p => (string)p.Attribute("ESR"))
                                    .ToList())
                    };


                var listRegShAnonym = listRegShQuery.ToList();

                var newRegulatorySchedules = listRegShAnonym.Select(anonym =>
                {
                    return new RegulatorySchedule
                    {
                        NumberOfTrain = anonym.TrainNumber,
                        ArrivalTime = DateTimeConverter(anonym.TimeArrival),
                        DepartureTime = DateTimeConverter(anonym.TimeDeparture),
                        DaysFollowings = DaysFollowingsConverter(anonym.DaysTrain, anonym.DaysTrainType),
                        RouteName = anonym.Name,
                        DestinationStation = new Station { EcpCode = int.Parse(anonym.ESRDeparture), Name = "" },
                        DispatchStation = new Station { EcpCode = int.Parse(anonym.ESRArrival), Name = "" },
                        ListOfStops = new ObservableCollection<Station>(anonym.PlacesStop.Select(stStr => new Station { EcpCode = int.Parse(stStr), Name = "" })),
                        ListWithoutStops = new ObservableCollection<Station>(anonym.PlacesNotStop.Select(stStr => new Station { EcpCode = int.Parse(stStr), Name = "" }))
                    };
                }).ToList();

                return newRegulatorySchedules;

            }
            catch (Exception ex)
            {
                throw new XmlException($"Ошибка парсинга XML: {xmlDoc.BaseUri}. ОШИБКА: {ex}");
            }
        }




        /* ЗАПРОС получение Имен станций
           <?xmlversion="1.0" encoding="utf-8" ?>
           <root>
             <AuthenticationUser=’ddd’ Password=’rumba’/>
             <Place ESR=’11111’/>
             <Place ESR=’22222’/>
                    …
             <PlaceESR=’99999’/>
           </root>
       */
        public XDocument GetRequestStationsName(IEnumerable<Station> stations)
        {
            if (stations == null || !stations.Any())
                return null;

            var user = "ddd";
            var password = "samba";
            XDocument xDoc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement("root",
                new XElement("Authentication", new XAttribute("User", user), new XAttribute("Password", password))
                ));

            foreach (var st in stations)
            {
                xDoc.Root?.Add(new XElement("Place", new XAttribute("ESR", st.EcpCode)));
            }

            return xDoc;
        }



        public IEnumerable<Station> SetResponseStationsName(XDocument xmlDoc, IEnumerable<Station> stationValid)
        {
            try
            {
                var listStationsNameQuery =
                from train in xmlDoc.Descendants("Place")
                select new
                {
                    Esr = (string)train.Attribute("ESR"),
                    Name = (string)train.Attribute("Name"),
                };

                var listStationsNameAnonym = listStationsNameQuery.ToList();
            }
            catch (Exception ex)
            {
                throw new XmlException($"Ошибка парсинга XML: {xmlDoc.BaseUri}. ОШИБКА: {ex}");
            }


          

            return null;
        }




        private DateTime? DateTimeConverter(string date)
        {
            if (string.IsNullOrEmpty(date))
                return null;

            try
            {
                return DateTime.ParseExact(date, "HH:mm:ss", null);

                //return DateTime.ParseExact(date, "yyyy.MM.dd HH:mm:ss", null);
            }
            catch (Exception)
            {
                return null;
            }
        }


        private string DaysFollowingsConverter(string daysTrain, string daysTrainType)
        {
            return daysTrain;
        }
    }
}