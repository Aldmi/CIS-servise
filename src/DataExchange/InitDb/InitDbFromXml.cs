using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
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
        Load,
        Processing,
        SaveDbRegSh,
        Error,
        SucsessReadOnDb,
        FindNewStations,
        CorrectionStationNamesSucsess,
        CorrectionStationNamesError,
        SaveNewStationInDb,
        Ok
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
                    OwnerStation = _stationOwner,
                    StatusString = StatusString,
                    Status = Status
                };

                _events.PublishOnUIThread(eventData);

            }
        }

        public Status Status { get; set; }

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
            Status = Status.Load;
            StatusString = "Загрузка регулярного расписания";
            
            //ЗАПРОС РЕГУЛЯРНОГО РАСПИСАНИЯ

            IEnumerable<RegulatorySchedule> newRegSh;
            try
            {
                var regShProt = new XmlRegulatoryShProtokol();
                var xmlDocResp= await sheduleGetter.Get(regShProt.GetRequest(new List<Station> { _stationOwner }));
                if (xmlDocResp == null)
                {
                    Status = Status.Error;
                    StatusString = $"XML файл ответа регулярного расписания для станции: {_stationOwner.Name} не получен";
                    return false;
                }
                else
                {
                    var errorString = regShProt.CheckNotAllowedResponse(xmlDocResp);
                    if (errorString != null)
                    {
                        Status = Status.Error;
                        StatusString = $"XML файл ответа регулярного расписания для станции: {_stationOwner.Name} получен \nи он содержит описание ошибки.Ответ: {errorString}";
                        return false;
                    }               
                }

                newRegSh = regShProt.SetResponse(xmlDocResp, _stationOwner);
                if (newRegSh == null)
                {
                    Status = Status.Error;
                    StatusString = $"ECP код станци в загруженном документе не совпадает с кодом нужной станции: {_stationOwner.EcpCode}";

                    return false;
                }
            }
            catch (Exception ex)
            {
                Status = Status.Error;
                StatusString = $" Вокзал: {_stationOwner.Name}. Ошибка получения XML ответа регулярного расписания {ex}";
                return false;
            }


            Status = Status.SaveDbRegSh;
            StatusString = "Начата загрузка данных в БД ...";

            var res = await DbAcsessRegSh(newRegSh, stationGetter);
            if (res)
            {
                Status = Status.Ok;
                StatusString = "Загрузка данных в БД завершенна успешно";
                return true;
            }

            return false;
        }




        private async Task<bool> DbAcsessRegSh(IEnumerable<RegulatorySchedule> newRegSh, IGetterXml stationGetter)
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
                    return false;


                //Все зарегистрированные станции в БД
                var allStations = _unitOfWork.StationRepository.Get().ToList();

                Status = Status.SucsessReadOnDb;
                StatusString = "Считанно с БД регулярное расписание и все станции данного вокзала";


                //Нашли станции в БД или создали новые
                foreach (var regSh in newRegSh)
                {
                    var stationOfDestinationDb = allStations.FirstOrDefault(st => st.EcpCode == regSh.DestinationStation.EcpCode);
                    if (stationOfDestinationDb != null)
                    {
                        regSh.DestinationStation = stationOfDestinationDb;

                        var doubleStation = railwayStation.Stations.FirstOrDefault(st => st.EcpCode == stationOfDestinationDb.EcpCode);
                        if (doubleStation == null)
                        {
                            railwayStation.Stations.Add(stationOfDestinationDb);
                        }
                    }
                    else
                    {
                        allStations.Add(regSh.DestinationStation);
                    }

                    var dispatchStationDb = allStations.FirstOrDefault(st => st.EcpCode == regSh.DispatchStation.EcpCode);
                    if (dispatchStationDb != null)
                    {
                        regSh.DispatchStation = dispatchStationDb;

                        var doubleStation = railwayStation.Stations.FirstOrDefault(st => st.EcpCode == dispatchStationDb.EcpCode);
                        if (doubleStation == null)
                        {
                            railwayStation.Stations.Add(dispatchStationDb);
                        }
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

                            var doubleStation = railwayStation.Stations.FirstOrDefault(st => st.EcpCode == findStDb.EcpCode);
                            if (doubleStation == null)
                            {
                                railwayStation.Stations.Add(findStDb);
                            }
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

                            var doubleStation = railwayStation.Stations.FirstOrDefault(st => st.EcpCode == findStDb.EcpCode);
                            if (doubleStation == null)
                            {
                                railwayStation.Stations.Add(findStDb);
                            }
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




                if (addedStations.Any())
                {
                    Status = Status.FindNewStations;
                    StatusString = "Найденны новые станции";


                    //выполним запрос для получения имен добавленных станций к сервису
                    IEnumerable<Station> newCorrectNameStation = null;
                    try
                    {
                        var xmlStationProt = new XmlStationProtokol();
                        var xmlDocResp = stationGetter.Get(xmlStationProt.GetRequest(addedStations));
                        newCorrectNameStation = xmlStationProt.SetResponse(await xmlDocResp, addedStations).ToList();
                    }
                    catch (Exception ex)
                    {
                        Status = Status.Error;
                        StatusString = $"Ошибка получения XML ответа названий станций {ex}";
                        return false;
                    }



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
                        }
                    }

                    //удалим не найденные станции из списка добавленных
                    if (notFoundStations == null)
                    {
                        Status = Status.CorrectionStationNamesSucsess;
                        StatusString = "Корректировка имен станций прошла успешно";
                    }
                    else
                    {
                        Status = Status.CorrectionStationNamesError;
                        StatusString = "Выявленны станции для которых не удалось скорректировать имя";

                        addedStations = addedStations.Except(notFoundStations).ToList();

                        // Заменим в расписании все станции для которых не удалось скорректирвоать имя на errorStation (ECP = 0)
                        var errorStation = _unitOfWork.StationRepository.Search(station => station.EcpCode == 0).FirstOrDefault();  //если станция не найденна EF сам создает такую.
                        foreach (var regSh in newRegSh)
                        {
                            var stationOfDestinationDb = notFoundStations.FirstOrDefault(st => st.EcpCode == regSh.DestinationStation.EcpCode);
                            if (stationOfDestinationDb != null)
                            {
                                regSh.DestinationStation = errorStation;
                            }

                            var dispatchStationDb = notFoundStations.FirstOrDefault(st => st.EcpCode == regSh.DispatchStation.EcpCode);
                            if (dispatchStationDb != null)
                            {
                                regSh.DispatchStation = errorStation;
                            }

                            for (int i = 0; i < regSh.ListOfStops.Count; i++)
                            {
                                var findStDb = notFoundStations.FirstOrDefault(st => st.EcpCode == regSh.ListOfStops[i].EcpCode);
                                if (findStDb != null)
                                {
                                    regSh.ListOfStops[i] = errorStation;
                                }
                            }

                            for (int i = 0; i < regSh.ListWithoutStops.Count; i++)
                            {
                                var findStDb = notFoundStations.FirstOrDefault(st => st.EcpCode == regSh.ListWithoutStops[i].EcpCode);
                                if (findStDb != null)
                                {
                                    regSh.ListWithoutStops[i] = findStDb;
                                }
                            }
                        }
                    }
                }



                //СОХРАНИМ ИЗМЕНЕНИЯ
                try
                {
                    //Новые станции добавим в БД.
                    if (addedStations.Any())
                    {
                        _unitOfWork.StationRepository.InsertRange(addedStations);
                        await _unitOfWork.SaveAsync();

                        Status = Status.SaveNewStationInDb;
                        StatusString = "Новые станции сохраненны в БД";
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
                    //await _unitOfWork.SaveAsync();//DEBUG
                    //_unitOfWork.Save();

                    Status = Status.SaveNewStationInDb;
                    StatusString = "Успешно заменено старое регулярное расписание на новое в БД";

                    return true;
                }
                catch (DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);


                    var fullErrorMessage = string.Join("; ", errorMessages);


                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    Status = Status.Error;
                    StatusString = $"ОШИБКА при сохранении в БД: \"{exceptionMessage}\"";

                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors); //TODO: ???
                }
                catch (Exception ex)
                {
                    Status = Status.Error;
                    StatusString = $"Неизвестная Ошибка работы с БД. ОШИБКА: \"{ex}\"";

                    throw new Exception(StatusString);
                }
                finally
                {
                    _windsorContainer.Release(_unitOfWork);         //обязательно вручную чистить, т.к. время жизни в DI контейнере заданно как LifestyleTransient
                }
            }
        }


    }
}