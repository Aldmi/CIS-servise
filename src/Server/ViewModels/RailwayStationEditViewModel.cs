using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
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
        Info,
        Station
    }


    public class RailwayStationEditViewModel : Screen
    {
        private readonly IUnitOfWork _unitOfWork;


        public RailwayStation RailwayStations { get; private set; }

        public ICollection<Station> Stations { get; set; }

        public Options CurrentOption { get; set; }

        public RailwayStationEditViewModel(IUnitOfWork unitOfWork, RailwayStation railwayStation)
        {
            _unitOfWork = unitOfWork;
            RailwayStations = railwayStation;

            Stations = new List<Station>( railwayStation.Stations);

            CurrentOption = Options.Station;
        }
    }
}