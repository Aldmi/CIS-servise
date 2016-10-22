using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Caliburn.Micro;
using Domain.Abstract;
using Domain.DbContext;
using Domain.Entities;

namespace Server.ViewModels
{
    public class AppViewModel : Conductor<object>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;
        private readonly IUnitOfWork _unitOfWork;


        public IDisposable DispouseOrderStatusProcess { get; set; }
        public IDisposable DispouseOrderStatusAcsess { get; set; }



        public AppViewModel(IEventAggregator events, IWindowManager windowManager, IUnitOfWork unitOfWork)
        {
            _windowManager = windowManager;
            _unitOfWork = unitOfWork;
            _eventAggregator = events;
            events.Subscribe(this);
        }






        public async void RailwayStation1()
        {
            string railwayStationName = "Вокзал 3";
            var railwayStation = _unitOfWork.RailwayStationRepository.Search(r => r.Name == railwayStationName).Include(r => r.Stations).First();
            var editViewModel = new RailwayStationEditViewModel(_unitOfWork, railwayStation);
            _windowManager.ShowWindow(editViewModel);










            //var dialogViewModel= new DialogViewModel();
            //var result= _windowManager.ShowDialog(dialogViewModel);

            //if (result != null && result.Value)
            //{
            //    MessageBox.Show("Ok");
            //}
            //else
            //{
            //    MessageBox.Show("Cancel");
            //}

            // _unitOfWork.StationRepository.Insert(new Station { Description = "hhhhh", EcpCode = 100, Name = "iuyit" });
            var stations = _unitOfWork.StationRepository.Get().OrderBy(x=>x.Id).ToList();
            //_unitOfWork.RailwayStationRepository.Insert(new RailwayStation {Name = "Вокзал 2", AllStations = new List<Station>(stations.Skip(3))});

            //_unitOfWork.OperativeScheduleRepository.Insert(new OperativeSchedule { NumberOfTrain = 12, ArrivalTime = DateTime.Now, DepartureTime = DateTime.Today, RouteName = "Маршрут 3", DispatchStation = stations[2], StationOfDestination = stations[3], ListWithoutStops = new List<Station>(stations)});
            //await _unitOfWork.SaveAsync();

            // var operativeSh= _unitOfWork.RailwayStationRepository.Search(r => r.Name == "Вокзал 2").Include(r => r.AllStations).First();
            // var operative = _unitOfWork.RailwayStationRepository.Get().Include(r => r.AllStations).ToList();

            //var editStation = stations[0];
            //editStation.Name = "New NAME!!";
            //_unitOfWork.StationRepository.Update(editStation);
            //await _unitOfWork.SaveAsync();


            //Обновление
            //var rs = _unitOfWork.RailwayStationRepository.GetById(1);  //_unitOfWork.RailwayStationRepository.Get().OrderBy(x => x.Id).ToList().First();
            //rs.Stations = new List<Station>(stations.Skip(1).ToList());
            //rs.Name = "aaa";

            //_unitOfWork.RailwayStationRepository.Update(rs);
            //await _unitOfWork.SaveAsync();


            //удаление
            //var rs = _unitOfWork.RailwayStationRepository.GetById(2);  //_unitOfWork.RailwayStationRepository.Get().OrderBy(x => x.Id).ToList().First();
            //_unitOfWork.RailwayStationRepository.Remove(rs);
            //await _unitOfWork.SaveAsync();
        }






        protected override void OnDeactivate(bool close)
        {
            _eventAggregator.Unsubscribe(this);
            base.OnDeactivate(close);
        }
    }
}