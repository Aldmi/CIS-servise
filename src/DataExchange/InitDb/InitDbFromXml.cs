using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Castle.Windsor;
using DataExchange.Event;
using DataExchange.Transaction;
using DataExchange.XmlApkDkProtokol;
using DataExchange.XmlGetter;
using Domain.Abstract;
using Domain.Entities;


namespace DataExchange.InitDb
{
    public enum Status
    {
        LoadRegSh,
        ProcessingRegSh,
        SaveDbRegSh,
        OkRegSh,
        NotFoundStation
    }



    public class InitDbFromXml
    {

        #region Fields

        private readonly IWindsorContainer _windsorContainer;
        private IUnitOfWork _unitOfWork;
        private readonly IEventAggregator _events;

        private readonly Station _stationOwner;


        #endregion





        #region prop

        private string _statusString;
        public string StatusString
        {
            get { return _statusString; }
            set
            {
                if (_statusString == value)
                    return;

                _statusString = value;

                var eventData = new InitDbFromXmlStatus
                {
                    Status = StatusString
                };
                _events.Publish(eventData,
                    action =>
                    {
                        Task.Factory.StartNew(action);
                    });
            }
        }

        public Status StatusRegSh { get; set; }

        #endregion





        #region ctor

        public InitDbFromXml(IWindsorContainer windsorContainer, IEventAggregator events, Station stationOwner)
        {
            _windsorContainer = windsorContainer;
            _stationOwner = stationOwner;
            _events = events;
        }

        #endregion








        public async Task<bool> InitRegulatorySh(IGetterXml sheduleGetter, IGetterXml stationGetter)
        {
            StatusRegSh = Status.LoadRegSh;

            var regShProt = new XmlRegulatoryShProtokol();
            var xmlDocResp = sheduleGetter.Get();
            var newRegSh = regShProt.SetResponse(xmlDocResp, _stationOwner);

            if (newRegSh == null)
            {
                StatusString = $"ECP код станци в загруженном документе не совпадает с кодом нужной станции: {_stationOwner.EcpCode}";
                return false;
            }


            StatusRegSh = Status.SaveDbRegSh;
            await DbAcsessRegSh(newRegSh, stationGetter);
            StatusRegSh = Status.OkRegSh;

            return true;
        }




        private async Task DbAcsessRegSh(IEnumerable<RegulatorySchedule> newRegSh, IGetterXml stationGetter)
        {
            using (_unitOfWork = _windsorContainer.Resolve<IUnitOfWork>())
            {
                var railwayStation = await await Task.Factory.StartNew(async () =>
                {
                    var query = _unitOfWork.RailwayStationRepository.Search(r => r.Name == _stationOwner.Name)
                    .Include(s => s.Stations)
                    .Include(reg => reg.RegulatorySchedules);
                    return await query.FirstOrDefaultAsync();
                });

                if (railwayStation == null)
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

                    for (int i = 0; i < regSh.ListOfStops.Count; i++)
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
                }



                //вычислим добавленные станции данного вокзала
                var allStationsCurrent = _unitOfWork.StationRepository.Get().ToList();
                var addedStations = allStations.Except(allStationsCurrent).ToList();



                //выполним запрос для получения имен добавленных станций к сервису
                var xmlStationProt = new XmlStationProtokol();
                var xmlDocResp = stationGetter.Get(xmlStationProt.GetRequest(addedStations));
                var newCorrectNameStation = xmlStationProt.SetResponse(xmlDocResp, addedStations).ToList();


                //Скорректируем имена
                List<Station> notFoundStations = null;
                foreach (var addStation in addedStations)
                {
                    var station = newCorrectNameStation.FirstOrDefault(s => s.EcpCode == addStation.EcpCode);
                    if (station != null)
                    {
                        addStation.Name = station.Name;
                        addStation.RailwayStations = new List<RailwayStation> { railwayStation };
                    }
                    else
                    {
                        notFoundStations = notFoundStations ?? new List<Station>();
                        notFoundStations.Add(addStation);
                        StatusRegSh = Status.NotFoundStation;
                    }
                }


                //удалим не найденные станции из списка добавленных
                if (StatusRegSh != Status.NotFoundStation)
                {
                    StatusString = "Корректировка имен станций прошла успешно";
                }
                else
                {
                    StatusString = "Выявленны станции для которых не удалось скорректировать имя";
                    addedStations = addedStations.Except(notFoundStations).ToList();
                }


                //СОХРАНИМ ИЗМЕНЕНИЯ
                try
                {
                    //Новые станции добавим в БД.
                    if (addedStations.Any())
                    {
                        var find3 = addedStations.Where(st => st.Name.Length < 1).ToList();//DEBUG
                        _unitOfWork.StationRepository.InsertRange(addedStations);
                        await _unitOfWork.SaveAsync();
                    }


                    //Удалим старое расписание.
                    var currentRegSh = railwayStation.RegulatorySchedules.ToList();
                    _unitOfWork.RegulatoryScheduleRepository.RemoveRange(currentRegSh);


                    //Добавим новое расписание.
                    railwayStation.RegulatorySchedules.Clear();
                    foreach (var regSh in newRegSh)
                    {
                        railwayStation.RegulatorySchedules.Add(regSh);
                    }


                    //Сохраним изменения
                    _unitOfWork.RailwayStationRepository.Update(railwayStation);
                    await _unitOfWork.SaveAsync();
                }
                catch (DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);


                    var fullErrorMessage = string.Join("; ", errorMessages);


                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    StatusString = $"ОШИБКА при сохранении в БД: \"{exceptionMessage}\"";

                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
                catch (Exception)
                {
                    StatusString = $"Ошибка работы с БД.";
                }
            }
        }


    }
}