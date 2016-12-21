using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Domain.Entities;

namespace DataExchange.XmlApkDkProtokol
{
    public class XmlStationProtokol : XmlAbstractProtokol
    {

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
        public override XDocument GetRequest(IEnumerable<Station> stations)
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



        public IEnumerable<Station> SetResponse(XDocument xmlDoc, IEnumerable<Station> stationValid)
        {
            try
            {
                var listStationsNameQuery =
                from train in xmlDoc.Descendants("rc")
                from station in stationValid          
                where (string)train.Attribute("int_ESR") == station.EcpCode.ToString()
                select new
                {
                    Esr = (string)train.Attribute("int_ESR"),
                    Name = (string)train.Attribute("str_Name"),
                };

                var listStationsNameAnonym = listStationsNameQuery.ToList();


                //var listStation= new List<Station>();
                //foreach (var anonym in listStationsNameAnonym)
                //{
                //    if(anonym)
                //}


                var listStation= listStationsNameAnonym.Select(anonym => new Station
                {
                    EcpCode = int.Parse(anonym.Esr),
                    Name = anonym.Name
                }).ToList();

                return listStation;
            }
            catch (Exception ex)
            {
                throw new XmlException($"Ошибка парсинга XML: {xmlDoc.BaseUri}. ОШИБКА: {ex}");
            }
        }

    }
}