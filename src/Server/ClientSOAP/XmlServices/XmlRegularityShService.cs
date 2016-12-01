using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Windows;
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



        /* ЗАПРОС
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
        public IEnumerable<RegulatorySchedule> SetResponse(XDocument xmlDoc)
        {
            try
            {
                var identQuery = from train in xmlDoc.Descendants("Place")
                    select new
                    {
                        ESR = (string) train.Attribute("ESR"),
                        Data = (string) train.Attribute("Data")
                    };


                var listRegShQuery =
                    from train in xmlDoc.Descendants("Train")
                    select new
                    {
                        TrainNumber = (string) train.Attribute("TrainNumber"),
                        Name = (string) train.Attribute("Name"),
                        ESRDeparture = (string) train.Attribute("ESRDeparture"),
                        ESRArrival = (string) train.Attribute("ESRArrival"),
                        TimeDeparture = (string) train.Attribute("TimeDeparture"),
                        TimeArrival = (string) train.Attribute("TimeArrival"),
                        DaysTrainType = (string) train.Attribute("DaysTrainType"),
                        DaysTrain = (string) train.Attribute("DaysTrain"),
                        PlacesStop =
                            new List<string>(
                                train.Element("PlacesStop")?
                                    .Elements("Place")
                                    .Select(p => p.Attribute("ESR").ToString())
                                    .ToList()),
                        PlacesNotStop =
                            new List<string>(
                                train.Element("PlacesNotStop")?
                                    .Elements("Place")
                                    .Select(p => p.Attribute("ESR").ToString())
                                    .ToList())
                    };


                var identAnonym = identQuery.FirstOrDefault();
                var listRegShAnonym = listRegShQuery.ToList();

                // сверить идентификатор (для нужной станции расписание)
                // на сонове listRegShAnonym создать список listRegSh.

            }
            catch (Exception ex)
            {      
              throw new XmlException($"Ошибка парсинга XML: {xmlDoc.BaseUri}. ОШИБКА: {ex}");
            }




            return null;
        }
    }
}