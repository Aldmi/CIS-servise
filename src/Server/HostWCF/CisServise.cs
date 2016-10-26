﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Domain.Abstract;
using Domain.Entities;
using WCFCis2AvtodictorContract.Contract;
using WCFCis2AvtodictorContract.DataContract;

namespace Server.HostWCF
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class CisServise : IServerContract
    {
        private readonly IUnitOfWork _unitOfWork;



        public CisServise(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public async Task<ICollection<StationsData>> GetStations(int? count = null)
        {
            try
            {
                var stations = (count != null) ? await _unitOfWork.StationRepository.Get().AsNoTracking().Take(count.Value).ToListAsync() :
                                                 await _unitOfWork.StationRepository.Get().AsNoTracking().ToListAsync();

                return stations.Select(st => new StationsData { Description = st.Description, Id = st.Id, Name = st.Name, EcpCode = st.EcpCode }).ToList();

            }
            catch(Exception ex)
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
                    DispatchStation = new StationsData { Id = op.DispatchStation.Id, Name = op.DispatchStation.Name, Description = op.DispatchStation.Description, EcpCode = op.DispatchStation.EcpCode },
                    StationOfDestination = new StationsData { Id = op.StationOfDestination.Id, Name = op.StationOfDestination.Name, Description = op.StationOfDestination.Description, EcpCode = op.StationOfDestination.EcpCode },
                    ListOfStops = new List<StationsData>(op.ListOfStops.Select(st=> new StationsData { Id = st.Id, Name = st.Name, EcpCode = st.EcpCode, Description = st.Description})),
                    ListWithoutStops = new List<StationsData>(op.ListWithoutStops.Select(st => new StationsData { Id = st.Id, Name = st.Name, EcpCode = st.EcpCode, Description = st.Description }))
                }).ToList();
            }
            catch(Exception ex)
            {
                throw ex;         //TODO: реализовать отправку форматированного исключения на сервер.
            }
        }


        public Task<ICollection<RailwayStationData>> GetRailwayStations(int? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<DiagnosticData>> GetDiagnostics(int? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<InfoData>> GetInfos(int? count = null)
        {
            throw new NotImplementedException();
        }

        //TODO: добавить методы "получить N данных вокзала по id вокзала: станции, расписание,..."
        //TODO: добавить методы "получить N данных из какой-то таблицы: таблица станций, таблица расписаний, ..."
        //TODO: добавить методы "передать данные об состояния автодиктора"

        //public ICollection<OperativeScheduleData> GetOperativeSchedules(int count)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}