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



        public CisServise(IUnitOfWork unitOfWork, IEventAggregator events)
        {
            _unitOfWork = unitOfWork;
            _events = events;
        }




        #region ImplementsIServerContract

        public async Task<ICollection<StationsData>> GetStations(int? count = null)
        {
            try
            {
                var stations = (count != null)
                    ? await _unitOfWork.StationRepository.Get().AsNoTracking().Take(count.Value).ToListAsync()
                    : await _unitOfWork.StationRepository.Get().AsNoTracking().ToListAsync();

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


        public Task<ICollection<RegulatoryScheduleData>> GetRegulatorySchedules(int? count = null)
        {
            throw new NotImplementedException();
        }


        public async Task<ICollection<OperativeScheduleData>> GetOperativeSchedules(int? count = null)
        {
            try
            {
                var operativeSchedules = (count != null)
                    ? await
                        _unitOfWork.OperativeScheduleRepository.Get()
                            .Include(op => op.DispatchStation)
                            .Include(op => op.StationOfDestination)
                            .Include(op => op.ListOfStops)
                            .Include(op => op.ListWithoutStops)
                            .AsNoTracking()
                            .Take(count.Value)
                            .ToListAsync()
                    : await
                        _unitOfWork.OperativeScheduleRepository.Get()
                            .Include(op => op.DispatchStation)
                            .Include(op => op.StationOfDestination)
                            .Include(op => op.ListOfStops)
                            .Include(op => op.ListWithoutStops)
                            .AsNoTracking()
                            .ToListAsync();

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
                        StationOfDestination =
                            new StationsData
                            {
                                Id = op.StationOfDestination.Id,
                                Name = op.StationOfDestination.Name,
                                Description = op.StationOfDestination.Description,
                                EcpCode = op.StationOfDestination.EcpCode
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
                throw ex; //TODO: реализовать отправку форматированного исключения на сервер.
            }
        }



        public async Task<RailwayStationData> GetRailwayStationByName(string name)
        {
            if (name == null)
                throw new Exception("имя вокзала не указанно");

            var railwayStation = await _unitOfWork.RailwayStationRepository.Search(r => r.Name == name)
                .Include(r => r.Stations)
                .Include(op => op.OperativeSchedules)
                //TODO: Добавить Infos, Diagnostics, RegulatorySchedules
                .FirstOrDefaultAsync();


            if (railwayStation == null)
                throw new Exception("такого имени нету");


            return new RailwayStationData
            {
                Name = railwayStation.Name,
                Id = railwayStation.Id,
                Stations =
                    railwayStation.Stations.Select(
                        r =>
                            new StationsData
                            {
                                Id = r.Id,
                                Name = r.Name,
                                EcpCode = r.EcpCode,
                                Description = r.Description
                            }).ToList(),
                OperativeSchedules = railwayStation.OperativeSchedules.Select(op => new OperativeScheduleData
                {
                    Id = op.Id,
                    DispatchStation =
                        new StationsData
                        {
                            Id = op.DispatchStation.Id,
                            Description = op.DispatchStation.Description,
                            EcpCode = op.DispatchStation.EcpCode,
                            Name = op.DispatchStation.Name
                        },
                    StationOfDestination =
                        new StationsData
                        {
                            Id = op.StationOfDestination.Id,
                            Description = op.StationOfDestination.Description,
                            EcpCode = op.StationOfDestination.EcpCode,
                            Name = op.DispatchStation.Name
                        },
                    RouteName = op.RouteName,
                    ArrivalTime = op.ArrivalTime,
                    DepartureTime = op.DepartureTime,
                    NumberOfTrain = op.NumberOfTrain,
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
                }).ToList(),
                //TODO: Добавить Infos, Diagnostics, RegulatorySchedules
            };
        }

        public Task<ICollection<DiagnosticData>> GetDiagnostics(int? count = null)
        {
            throw new NotImplementedException();
        }

        public void SetDiagnostics(string nameRailwayStation, ICollection<DiagnosticData> diagnosticData)
        {
            //DEBUG-----------------------------------------------------
            diagnosticData = new List<DiagnosticData>
            {
                new DiagnosticData {DeviceNumber = 10, Fault = "sadsad", Status = 458},
                new DiagnosticData {DeviceNumber = 11, Fault = "gfg", Status = 78},
                new DiagnosticData {DeviceNumber = 12, Fault = "jh", Status = 65}
            };
            //DEBUG-----------------------------------------------------

            var eventData = new AutodictorDiagnosticEvent {NameRailwayStation = nameRailwayStation, DiagnosticData = diagnosticData};
            _events.Publish(eventData,
                      action =>
                      {
                          Task.Factory.StartNew(action);
                      });
        }

        public Task<ICollection<InfoData>> GetInfos(int? count = null)
        {
            throw new NotImplementedException();
        }

        #endregion





        #region ImplementsIServerContractSimple

        public Task<ICollection<RegulatoryScheduleDataSimple>> GetSimpleRegulatorySchedules(int? count = null)
        {
            return null;
        }

        public Task<ICollection<OperativeScheduleDataSimple>> GetSimpleOperativeSchedules(int? count = null)
        {
            return null;
        }

        #endregion

    }
}