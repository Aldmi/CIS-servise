using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Castle.Windsor;
using Domain.Abstract;
using Domain.Entities;



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





        public void RequestRegulatorySchedule()
        {
            StatusRegSh = Status.LoadRegSh;
            // Выполнили запрос и получили список Регулярноного расписания.

           // var data = DateTime.Parse("2015.09.21");

            var newRegSh = new List<RegulatorySchedule>
            {

                new RegulatorySchedule
                {
                    ArrivalTime = DateTime.Now,
                    DepartureTime = DateTime.Today,
                    RouteName = "Маршурт новый 555",
                    NumberOfTrain = "34Ф",
                    StationOfDestination = new Station {EcpCode = 851, Name = ""},        //создаем фейковую станцию и заполняем EcpCode из ответа.
                    DispatchStation = new Station {EcpCode = 741, Name = ""},
                    DaysFollowings = "qwert",
                    ListOfStops = new ObservableCollection<Station>(),
                    ListWithoutStops = new ObservableCollection<Station>()
                },
                new RegulatorySchedule
                {
                    ArrivalTime = DateTime.Now,
                    DepartureTime = DateTime.Today,
                    RouteName = "Маршурт новый 666",
                    NumberOfTrain = "45",
                    StationOfDestination = new Station {EcpCode = 452, Name = ""},
                    DispatchStation = new Station {EcpCode = 963, Name = ""},
                    DaysFollowings = "mjb,jb",
                    ListOfStops = new ObservableCollection<Station>(),
                    ListWithoutStops = new ObservableCollection<Station>()
                },
                new RegulatorySchedule
                {
                    ArrivalTime = DateTime.Now,
                    DepartureTime = DateTime.Today,
                    RouteName = "Маршурт новый 555",
                    NumberOfTrain = "45",
                    StationOfDestination = new Station {EcpCode = 741, Name = ""},
                    DispatchStation = new Station {EcpCode = 996, Name = ""},
                    DaysFollowings = "fzck",
                    ListOfStops = new ObservableCollection<Station>(),
                    ListWithoutStops = new ObservableCollection<Station>()
                }
            };

            StatusRegSh = Status.ProcessingRegSh;

            DbAcsessRegSh(newRegSh);



            StatusRegSh = Status.SaveDbRegSh;


            StatusRegSh = Status.OkRegSh;
        }


        public async void DbAcsessRegSh(IEnumerable<RegulatorySchedule> newRegSh)
        {
            //Обработаем полученные данные
            //1. Сделам запрос в БД на список всех станций. Получум локальную копию
            //2. Переберем полученный список регулярного расписания и для каждой станции елси ее НЕТ в списке стануции то пусть остается, если ЕСТЬ то заменяем на найденную.
            //3. Итого получим готовый список регулярного расписания со всеми станциями из таблицы станций.
            //4. Очистим спсисок локальной копии рег. расписания.
            //5. Заполним список новыми элементами.
            //6. Сделаем Update на репозитории. и SaveAsync.

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


                foreach (var regSh in newRegSh)
                {
                    var stationOfDestinationDb = railwayStation.Stations.FirstOrDefault(st => st.EcpCode == regSh.StationOfDestination.EcpCode);
                    if (stationOfDestinationDb != null)
                    {
                        regSh.StationOfDestination = stationOfDestinationDb;
                    }

                    var dispatchStationDb = railwayStation.Stations.FirstOrDefault(st => st.EcpCode == regSh.DispatchStation.EcpCode);
                    if (dispatchStationDb != null)
                    {
                        regSh.DispatchStation = dispatchStationDb;
                    }

                    //Повторить для списка станций остоновчных и проездых.
                }

               // railwayStation.RegulatorySchedules.Clear();

                //foreach (var regSh in newRegSh)
                //{
                //    railwayStation.RegulatorySchedules.Add(regSh);
                //}

                //railwayStation.RegulatorySchedules= new List<RegulatorySchedule>(newRegSh);


                //Сначала добавляем новую станцию
               var addStation = new Station
               {
                   EcpCode = 455, Name = "yyyyy", 
                   RailwayStations = new List<RailwayStation> { railwayStation }
               };
                _unitOfWork.StationRepository.Insert(addStation);
                railwayStation.Stations.Add(addStation);                //в локальном списке она уже будет
                await _unitOfWork.SaveAsync();


                //в новой строке расписания подставляем станции из локального списка  railwayStation.Stations
                var tt = new RegulatorySchedule
                {
                    ArrivalTime = DateTime.Now,
                    DepartureTime = DateTime.Now,
                    RouteName = "Маршурт новый 555",
                    NumberOfTrain = "34Ф",
                    StationOfDestination = addStation,
                    DispatchStation = null,
                   DaysFollowings = "jghkgjfg",
                   ListOfStops = new ObservableCollection<Station>(),
                   ListWithoutStops = new ObservableCollection<Station>() 
               };

                railwayStation.RegulatorySchedules.Add(tt);
              

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