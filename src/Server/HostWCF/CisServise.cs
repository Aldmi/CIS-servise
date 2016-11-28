using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Caliburn.Micro;
using Domain.Abstract;
using Server.Event;
using WCFCis2AvtodictorContract.Contract;
using WCFCis2AvtodictorContract.DataContract;
using WCFCis2AvtodictorContract.DataContract.SimpleData;


namespace Server.HostWCF
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class CisServise : IServerContract, IServerContractSimple
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventAggregator _events;

        private int test = 0;


        public CisServise(IUnitOfWork unitOfWork, IEventAggregator events)
        {
            _unitOfWork = unitOfWork;
            _events = events;
        }




        #region ImplementsIServerContract

        public async Task<ICollection<StationsData>> GetStations(string nameRailwayStation, int? count = null)
        {
            try
            {
                var query = _unitOfWork.RailwayStationRepository.Search(r => r.Name == nameRailwayStation, null, "Stations").AsNoTracking();
                var railwayStation = await query.FirstOrDefaultAsync();
                var stations = (count != null) ? railwayStation.Stations.Take(count.Value) : railwayStation.Stations;


                return
                    stations.Select(
                        st =>
                            new StationsData
                            {
                                Description = st.Description,
                                Id = st.Id,
                                Name = st.Name,
                                EcpCode = st.EcpCode
                            }).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<ICollection<RegulatoryScheduleData>> GetRegulatorySchedules(string nameRailwayStation, int? count = null)
        {
            try
            {
                var query = _unitOfWork.RailwayStationRepository.Search(r => r.Name == nameRailwayStation, null, "RegulatorySchedules").AsNoTracking();
                var railwayStation = await query.FirstOrDefaultAsync();
                if (railwayStation == null)
                {
                    var ex = new ArgumentNullException($"{nameRailwayStation}");
                    throw new FaultException<ArgumentNullException>(ex, $"Вокзал с таким именем не найден \"{nameRailwayStation}\"");
                }

                var regularSchedules = (count != null) ? railwayStation.RegulatorySchedules.Take(count.Value) : railwayStation.RegulatorySchedules;

                return
                    regularSchedules.Select(op => new RegulatoryScheduleData
                    {
                        Id = op.Id,
                        RouteName = op.RouteName,
                        ArrivalTime = op.ArrivalTime,
                        DepartureTime = op.DepartureTime,
                        NumberOfTrain = op.NumberOfTrain,
                        DispatchStation =
                            new StationsData
                            {
                                Id = op.DispatchStation.Id,
                                Name = op.DispatchStation.Name,
                                Description = op.DispatchStation.Description,
                                EcpCode = op.DispatchStation.EcpCode
                            },
                        DestinationStation =
                            new StationsData
                            {
                                Id = op.DestinationStation.Id,
                                Name = op.DestinationStation.Name,
                                Description = op.DestinationStation.Description,
                                EcpCode = op.DestinationStation.EcpCode
                            },
                        DaysFollowing = op.DaysFollowings
                    }).ToList();
            }
            catch (Exception ex)
            {
                throw new FaultException<Exception>(ex, $"Запрос для вокзала\"{nameRailwayStation}\" привел к ошибке на ЦИС сервере");
            }
        }


        public async Task<ICollection<OperativeScheduleData>> GetOperativeSchedules(string nameRailwayStation, int? count = null)
        {
            //DEBUG-----------------------------------------------------
            //return new List<OperativeScheduleData>
            //{
            //    new OperativeScheduleData
            //    {
            //        ArrivalTime = DateTime.Now,
            //        DepartureTime = DateTime.Today,
            //        NumberOfTrain = 10,
            //        Id = 1,
            //        RouteName = "Маршрут 1",
            //        DispatchStation = new StationsData {Id = 1, Description = "описание 1", Name = "станция 1", EcpCode = 10},
            //        DestinationStation = new StationsData {Id = 3, Description = "описание 3", Name = "станция 3", EcpCode = 56}
            //    },
            //    new OperativeScheduleData
            //    {
            //        ArrivalTime = DateTime.Now,
            //        DepartureTime = DateTime.Today,
            //        NumberOfTrain = 25,
            //        Id = 2,
            //        RouteName = "Маршрут 2",
            //        DispatchStation = new StationsData {Id = 2, Description = "описание 2", Name = "станция 2", EcpCode = 23},
            //        DestinationStation = new StationsData {Id = 45, Description = "описание 45", Name = "станция 45", EcpCode = 569}
            //    }
            //};
            //DEBUG-----------------------------------------------------

            try
            {
                var query = _unitOfWork.RailwayStationRepository.Search(r => r.Name == nameRailwayStation, null, "OperativeSchedules").AsNoTracking();
                var railwayStation = await query.FirstOrDefaultAsync();
                if (railwayStation == null)
                {
                    var ex = new ArgumentNullException($"{nameRailwayStation}");
                    throw new FaultException<ArgumentNullException>(ex, $"Вокзал с таким именем не найден \"{nameRailwayStation}\"");
                }

                var operativeSchedules = (count != null) ? railwayStation.OperativeSchedules.Take(count.Value) : railwayStation.OperativeSchedules;

                return
                    operativeSchedules.Select(op => new OperativeScheduleData
                    {
                        Id = op.Id,
                        RouteName = op.RouteName,
                        ArrivalTime = op.ArrivalTime,
                        DepartureTime = op.DepartureTime,
                        NumberOfTrain = op.NumberOfTrain,
                        DispatchStation =
                            new StationsData
                            {
                                Id = op.DispatchStation.Id,
                                Name = op.DispatchStation.Name,
                                Description = op.DispatchStation.Description,
                                EcpCode = op.DispatchStation.EcpCode
                            },
                        DestinationStation =
                            new StationsData
                            {
                                Id = op.DestinationStation.Id,
                                Name = op.DestinationStation.Name,
                                Description = op.DestinationStation.Description,
                                EcpCode = op.DestinationStation.EcpCode
                            },
                        ListOfStops =
                            new List<StationsData>(
                                op.ListOfStops.Select(
                                    st =>
                                        new StationsData
                                        {
                                            Id = st.Id,
                                            Name = st.Name,
                                            EcpCode = st.EcpCode,
                                            Description = st.Description
                                        })),
                        ListWithoutStops =
                            new List<StationsData>(
                                op.ListWithoutStops.Select(
                                    st =>
                                        new StationsData
                                        {
                                            Id = st.Id,
                                            Name = st.Name,
                                            EcpCode = st.EcpCode,
                                            Description = st.Description
                                        }))
                    }).ToList();
            }
            catch (Exception ex)
            {
                throw new FaultException<Exception>(ex, $"Запрос для вокзала\"{nameRailwayStation}\" привел к ошибке на ЦИС сервере");
            }
        }


        public Task<ICollection<DiagnosticData>> GetDiagnostics(string nameRailwayStation, int? count = null)
        {
            throw new NotImplementedException();
        }


        public void SetDiagnostics(string nameRailwayStation, ICollection<DiagnosticData> diagnosticData)
        {
            if (diagnosticData != null)
            {
                var eventData = new AutodictorDiagnosticEvent
                {
                    NameRailwayStation = nameRailwayStation,
                    DiagnosticData = diagnosticData
                };
                _events.Publish(eventData,
                    action =>
                    {
                        Task.Factory.StartNew(action);
                    });
            }
        }


        public Task<ICollection<InfoData>> GetInfos(string nameRailwayStation, int? count = null)
        {
            throw new NotImplementedException();
        }

        #endregion



        #region ImplementsIServerContractSimple

        public Task<ICollection<RegulatoryScheduleDataSimple>> GetSimpleRegulatorySchedules(string nameRailwayStation, int? count = null)
        {
            return null;
        }

        public Task<ICollection<OperativeScheduleDataSimple>> GetSimpleOperativeSchedules(string nameRailwayStation, int? count = null)
        {
            return null;
        }

        #endregion

    }
}