using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Caliburn.Micro;
using Castle.Windsor;
using DataExchange.InitDb;
using DataExchange.XmlGetter;
using Domain.Abstract;
using Domain.Entities;

namespace DataExchange.WebClient
{
    public class ApkDkWebClient
    {
        #region Fields

        private readonly IWindsorContainer _windsorContainer;
        private readonly IEventAggregator _eventAggregator;
        private readonly IEnumerable<Station> _stationsOwner;

        #endregion





        #region ctor

        public ApkDkWebClient(IWindsorContainer windsorContainer, IEventAggregator eventAggregator, IEnumerable<Station> stationsOwner )   
        {
            _windsorContainer = windsorContainer;
            _eventAggregator = eventAggregator;
            _stationsOwner = stationsOwner;
        }

        #endregion




        public async Task LoadXmlDataInDb(string pathShedule, string pathStations, string tableName, Station stationOwner)
        {
            var initDb = new InitDbFromXml(_windsorContainer, _eventAggregator, stationOwner);

            try
            {
                switch (tableName)
                {
                    case "regular":
                        var sheduleGetter = new GetterXmlFromDisk(pathShedule, stationOwner);
                        var stationsGetter = new GetterXmlFromDisk(pathStations, stationOwner);
                        await initDb.InitRegulatorySh(sheduleGetter, stationsGetter);
                        break;


                    case "operative":
                        break;


                    case "info":
                        break;
                }
            }
            catch (Exception)    //TODO: более точно определять тип исключения и выводить его в окно.
            {
               // MessageBox.Show($"Ошибка работы с БД.");
            }
        }


        public async Task LoadHttpDataInDb(string tableName, Station stationOwner)
        {
            string httpAdr = ConfigurationManager.AppSettings.Get("httpAddress");
            string intetfacesShedule = string.Empty;
            string uriShedule = string.Empty;
            string intetfacesStations = ConfigurationManager.AppSettings.Get("Stations");
            string uriStations = $"{httpAdr}/{intetfacesStations}";
            var stationsGetter = new GetterXmlFromHttp(uriStations, stationOwner);

            var initDb = new InitDbFromXml(_windsorContainer, _eventAggregator, stationOwner);
            try
            {
                switch (tableName)
                {
                    case "regular":                 
                        intetfacesShedule = ConfigurationManager.AppSettings.Get("Regular");
                        uriShedule = $"{httpAdr}/{intetfacesShedule}";
                        var sheduleGetter = new GetterXmlFromHttp(uriShedule, stationOwner);
                        await initDb.InitRegulatorySh(sheduleGetter, stationsGetter);
                        break;

                    case "operative":
                        intetfacesShedule = ConfigurationManager.AppSettings.Get("Operat");
                        uriShedule = $"{httpAdr}/{intetfacesShedule}";
                        sheduleGetter = new GetterXmlFromHttp(uriShedule, stationOwner);
                        //await initDb.InitOperativeSh(sheduleGetter, stationsGetter);
                        break;

                    case "info":
                        intetfacesShedule = ConfigurationManager.AppSettings.Get("Info");
                        uriShedule = $"{httpAdr}/{intetfacesShedule}";
                        sheduleGetter = new GetterXmlFromHttp(uriShedule, stationOwner);
                        //await initDb.InitinfoSh(sheduleGetter, stationsGetter);
                        break;
                }
            }
            catch (Exception ex)    //TODO: более точно определять тип исключения и выводить его в окно.
            {
              
            }
        }



        public async Task LoadHttpSheduleAndLoadXmlStationsInDb(string pathStations, string tableName, Station stationOwner)
        {
            string httpAdr = ConfigurationManager.AppSettings.Get("httpAddress");
            string intetfacesShedule = string.Empty;
            string uriShedule = string.Empty;
           
            var stationsGetter = new GetterXmlFromDisk(pathStations, stationOwner);

            var initDb = new InitDbFromXml(_windsorContainer, _eventAggregator, stationOwner);
            try
            {
                switch (tableName)
                {
                    case "regular":
                        intetfacesShedule = ConfigurationManager.AppSettings.Get("Regular");
                        uriShedule = $"{httpAdr}/{intetfacesShedule}";
                        var sheduleGetter = new GetterXmlFromHttp(uriShedule, stationOwner);
                        await initDb.InitRegulatorySh(sheduleGetter, stationsGetter);
                        break;

                    case "operative":
                        intetfacesShedule = ConfigurationManager.AppSettings.Get("Operat");
                        uriShedule = $"{httpAdr}/{intetfacesShedule}";
                        sheduleGetter = new GetterXmlFromHttp(uriShedule, stationOwner);
                        //await initDb.InitOperativeSh(sheduleGetter, stationsGetter);
                        break;

                    case "info":
                        intetfacesShedule = ConfigurationManager.AppSettings.Get("Info");
                        uriShedule = $"{httpAdr}/{intetfacesShedule}";
                        sheduleGetter = new GetterXmlFromHttp(uriShedule, stationOwner);
                        //await initDb.InitinfoSh(sheduleGetter, stationsGetter);
                        break;
                }
            }
            catch (Exception ex)    //TODO: более точно определять тип исключения и выводить его в окно.
            {

            }


        }




        public void StartQurtzShedeule()
        {
            //передается 
            //1.  _windsorContainer
            //2.  _eventAggregator
            //3. List<Station> stationOwners- все станции для опроса по реглменту.

            // ----------------
            //передавать сам экземпляр ApkDkWebClient и tableName

            // для каждого запроса (рег, опер, инфо, диагност) вызывается функция старт.



        }


    }
}