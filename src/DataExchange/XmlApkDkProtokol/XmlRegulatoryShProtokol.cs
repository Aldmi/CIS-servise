using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Domain.Entities;

namespace DataExchange.XmlApkDkProtokol
{

    public class XmlRegulatoryShProtokol : XmlAbstractProtokol
    {
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
                        PlacesStop = FuncCalcListStation(train, "PlacesStop"), 
                        PlacesNotStop = FuncCalcListStation(train, "PlacesNotStop")
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
                        DestinationStation = new Station { EcpCode = int.Parse(anonym.ESRDeparture), Name = "Not Use" },
                        DispatchStation = new Station { EcpCode = int.Parse(anonym.ESRArrival), Name = "Not Use" },
                        ListOfStops = (anonym.PlacesStop == null) ? new ObservableCollection<Station>() : new ObservableCollection<Station>(anonym.PlacesStop.Select(stStr => new Station { EcpCode = int.Parse(stStr), Name = "Not Use" })),
                        ListWithoutStops = (anonym.PlacesNotStop == null) ? new ObservableCollection<Station>() : new ObservableCollection<Station>(anonym.PlacesNotStop.Select(stStr => new Station { EcpCode = int.Parse(stStr), Name = "Not Use" }))
                    };
                }).ToList();

                return newRegulatorySchedules;

            }
            catch (Exception ex)
            {
                throw new XmlException($"Ошибка парсинга XML: {xmlDoc.BaseUri}. ОШИБКА: {ex}");
            }
        }
    }
}