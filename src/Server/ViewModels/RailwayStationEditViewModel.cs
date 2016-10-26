using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
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

        public RailwayStation RailwayStation { get; private set; }

        public ObservableCollection<Station> Stations { get; set; }

        public ObservableCollection<OperativeSchedule> OperativeSchedules { get; set; }

        public Options CurrentOption { get; set; } = Options.OperativeSchedule;




        public RailwayStationEditViewModel(IUnitOfWork unitOfWork, RailwayStation railwayStation)
        {
            _unitOfWork = unitOfWork;
            RailwayStation = railwayStation;

            Stations = new ObservableCollection<Station>(RailwayStation.Stations.ToList());
            OperativeSchedules = new ObservableCollection<OperativeSchedule>(RailwayStation.OperativeSchedules);
        }




        #region Methods

        public void Save()
        {
            //Выполнить Update
            _unitOfWork.RailwayStationRepository.Update(RailwayStation);
            _unitOfWork.SaveAsync();

            
            //TryClose(true);
        }

        public void Clouse()
        {
            _unitOfWork.UndoChanges();
            TryClose(false);
        }

        #endregion
    }
}