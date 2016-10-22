using System.Collections.Generic;
using Caliburn.Micro;
using Domain.Abstract;
using Domain.Entities;

namespace Server.ViewModels
{
    public enum Options
    {
        OperativeSchedule,
        RegulatorySchedule,
        Diagnostic,
        Info
    }


    public class RailwayStationEditViewModel : Screen
    {
        private readonly IUnitOfWork _unitOfWork;


        public RailwayStation RailwayStations { get; private set; }

        public Options CurrentOption { get; set; }

        public RailwayStationEditViewModel(IUnitOfWork unitOfWork, RailwayStation railwayStation)
        {
            _unitOfWork = unitOfWork;
            RailwayStations = railwayStation;

            CurrentOption= Options.Diagnostic;
        }
    }
}