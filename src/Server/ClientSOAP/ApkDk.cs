using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Castle.Windsor;
using Domain.Abstract;
using Domain.Entities;
using Server.ClientSOAP.XmlServices;


namespace Server.ClientSOAP
{
    public enum Status
    {
        LoadRegSh,
        ProcessingRegSh,
        SaveDbRegSh,
        OkRegSh
    }



    public class ApkDk
    {
        #region Fields

        private readonly IWindsorContainer _windsorContainer;
        private IUnitOfWork _unitOfWork;

        #endregion




        #region prop

        public Status StatusRegSh { get; set; }

        #endregion




        #region ctor

        public ApkDk(IWindsorContainer windsorContainer)
        {
            _windsorContainer = windsorContainer;
        }

        #endregion





        public async Task RequestRegulatorySchedule()
        {
            StatusRegSh = Status.LoadRegSh;

            //DEBUG-- ЗАПРОС
            XmlRegularityShService xmlRegShService = new XmlRegularityShService();
            var station = new Station { EcpCode = 34567 };
            var xmlDoc = xmlRegShService.GetRequest(station);
            //xmlDoc.Save(Path.Combine(Environment.CurrentDirectory, "doc.xml"));

            await Task.Delay(1000);//Имитация отправки запроса и получения ответа

            //DEBUG-- ОТВЕТ
            var doc = XDocument.Load(Path.Combine(Environment.CurrentDirectory, "xmlRespRegSh.xml"));
            var newRegSh= xmlRegShService.SetResponse(doc, station);


            // Выполнили запрос и получили список Регулярноного расписания.

            //var newRegSh = new List<RegulatorySchedule>
            //{
            //    new RegulatorySchedule
            //    {
            //        ArrivalTime = DateTime.Now,
            //        DepartureTime = DateTime.Today,
            //        RouteName = "Маршурт новый 000",
            //        NumberOfTrain = "34Ф",
            //        DestinationStation = new Station {EcpCode = 851, Name = ""},        //создаем фейковую станцию и заполняем EcpCode из ответа.
            //        DispatchStation = new Station {EcpCode = 741, Name = ""},
            //        DaysFollowings = "qwert",
            //        ListOfStops = new ObservableCollection<Station> {new Station {EcpCode = 851, Name = ""}, new Station {EcpCode = 741, Name = ""}},
            //        ListWithoutStops = new ObservableCollection<Station>()
            //    },
            //    new RegulatorySchedule
            //    {
            //        ArrivalTime = DateTime.Now,
            //        DepartureTime = DateTime.Today,
            //        RouteName = "Маршурт новый 001",
            //        NumberOfTrain = "45",
            //        DestinationStation = new Station {EcpCode = 452, Name = ""},
            //        DispatchStation = new Station {EcpCode = 963, Name = ""},
            //        DaysFollowings = "mjb,jb",
            //        ListOfStops = new ObservableCollection<Station>(),
            //        ListWithoutStops = new ObservableCollection<Station> {new Station {EcpCode = 888, Name = ""}, new Station {EcpCode = 777, Name = ""}}
            //    },
            //    new RegulatorySchedule
            //    {
            //        ArrivalTime = DateTime.Now,
            //        DepartureTime = DateTime.Today,
            //        RouteName = "Маршурт новый 002",
            //        NumberOfTrain = "89",
            //        DestinationStation = new Station {EcpCode = 756, Name = ""},
            //        DispatchStation = new Station {EcpCode = 996, Name = ""},
            //        DaysFollowings = "fzck",
            //        ListOfStops = new ObservableCollection<Station>(),
            //        ListWithoutStops = new ObservableCollection<Station>()
            //    }
            //};

            StatusRegSh = Status.ProcessingRegSh;

            await DbAcsessRegSh(newRegSh);



            StatusRegSh = Status.SaveDbRegSh;


            StatusRegSh = Status.OkRegSh;
        }


        public async Task DbAcsessRegSh(IEnumerable<RegulatorySchedule> newRegSh)
        {
            const string railwayStationName = "Вокзал 1";


            using (_unitOfWork = _windsorContainer.Resolve<IUnitOfWork>())
            {            
                var railwayStation = await await Task.Factory.StartNew(async () =>
                 {
                     var query = _unitOfWork.RailwayStationRepository.Search(r => r.Name == railwayStationName)
                     .Include(s => s.Stations)
                     .Include(reg => reg.RegulatorySchedules);
                     return await query.FirstOrDefaultAsync();
                 });

                if(railwayStation == null)
                    return;


                //Все зарегистрированные станции в БД
                var allStations = _unitOfWork.StationRepository.Get().ToList();
  

                //Нашли станции в БД или создали новые
                foreach (var regSh in newRegSh)
                {
                    var stationOfDestinationDb = allStations.FirstOrDefault(st => st.EcpCode == regSh.DestinationStation.EcpCode);
                    if (stationOfDestinationDb != null)
                    {
                        regSh.DestinationStation = stationOfDestinationDb;
                    }
                    else
                    {
                        allStations.Add(regSh.DestinationStation);
                    }

                    var dispatchStationDb = allStations.FirstOrDefault(st => st.EcpCode == regSh.DispatchStation.EcpCode);
                    if (dispatchStationDb != null)
                    {
                        regSh.DispatchStation = dispatchStationDb;
                    }
                    else
                    {
                        allStations.Add(regSh.DispatchStation);
                    }

                    for (int i=0; i < regSh.ListOfStops.Count; i++)
                    {
                        var findStDb = allStations.FirstOrDefault(st => st.EcpCode == regSh.ListOfStops[i].EcpCode);
                        if (findStDb != null)
                        {
                            regSh.ListOfStops[i] = findStDb;
                        }
                        else
                        {
                            allStations.Add(regSh.ListOfStops[i]);
                        }
                    }

                    for (int i = 0; i < regSh.ListWithoutStops.Count; i++)
                    {
                        var findStDb = allStations.FirstOrDefault(st => st.EcpCode == regSh.ListWithoutStops[i].EcpCode);
                        if (findStDb != null)
                        {
                            regSh.ListWithoutStops[i] = findStDb;
                        }
                        else
                        {
                            allStations.Add(regSh.ListWithoutStops[i]);
                        }
                    }

                    //Повторить для списка станций остановчных и проездых.
                }

                //вычислим добавленные станции данного вокзала
                var addedStations = allStations.Except(railwayStation.Stations).ToList();

                //выполним запрос для получения имен добавленных станиций к сервису
                foreach (var needAddStation in addedStations)
                {
                    needAddStation.Name = $"Скоректированное имя {needAddStation.EcpCode}";
                    needAddStation.RailwayStations= new List<RailwayStation> {railwayStation};
                }

                //Новые станции добавим в БД.
                if (addedStations.Any())
                {
                    _unitOfWork.StationRepository.InsertRange(addedStations);
                    await _unitOfWork.SaveAsync();
                }


                //Удалим старое расписание.
                var currentRegSh = railwayStation.RegulatorySchedules.ToList();
                foreach (var regSh in currentRegSh)
                {
                    _unitOfWork.RegulatoryScheduleRepository.Remove(regSh);
                }


                //Добавим новое расписание.
                railwayStation.RegulatorySchedules.Clear();
                foreach (var regSh in newRegSh)
                {
                    railwayStation.RegulatorySchedules.Add(regSh);
                }


                //Сохраним изменения
                try
                {
                    _unitOfWork.RailwayStationRepository.Update(railwayStation);
                    await _unitOfWork.SaveAsync();
                }
                catch (DbEntityValidationException ex)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }

            }

        }
    }

}