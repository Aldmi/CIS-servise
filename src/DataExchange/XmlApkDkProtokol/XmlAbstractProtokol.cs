using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using Domain.Entities;

namespace DataExchange.XmlApkDkProtokol
{
    public abstract class XmlAbstractProtokol
    {
        /* ЗАПРОС
         <?xmlversion="1.0" encoding=" utf-8" ?>
          <root>
             <Authentication User=’ddd’ Password=’samba’/>
             <Place ESR=’34567’/>
          </root>
       */
        public virtual XDocument GetRequest(IEnumerable<Station> stations)
        {
            var station = stations.FirstOrDefault();

            var user = ConfigurationManager.AppSettings.Get("User");
            var password = ConfigurationManager.AppSettings.Get("Password");

            XDocument xDoc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement("root",
                new XElement("Authentication", new XAttribute("User", user), new XAttribute("Password", password)),
                new XElement("Place", new XAttribute("ESR", station.EcpCode))
                ));

            return xDoc;
        }



        // <root>
        //    <error>Place 19823 not allowed</error>
        // </root>
        public virtual string CheckNotAllowedResponse(XDocument xmlDoc)
        {
           return xmlDoc.Descendants("error").FirstOrDefault()?.Value;
        }


        #region Methode

        protected DateTime? DateTimeConverter(string date)
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


        protected string DaysFollowingsConverter(string daysTrain, string daysTrainType)
        {
            return $"Тип: \"{daysTrainType}\"   Дни: \"{daysTrain}\"";
        }



        protected List<string> FuncCalcListStation(XElement element, string nameElement)
        {
            if (element.Element(nameElement)?.Element("Place") == null)
                return null;



            return new List<string>(
                element.Element(nameElement)
                       .Elements("Place")
                       .Select(p => (string)p.Attribute("ESR"))
                       .ToList());

        }

        #endregion

    }
}