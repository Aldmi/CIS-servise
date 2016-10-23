using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
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
            const string railwayStationName = "Вокзал 3";
            var railwayStation = _unitOfWork.RailwayStationRepository.Search(r => r.Name == railwayStationName).Include(r => r.Stations).First();
            if (railwayStation != null)
            {
                var editViewModel = new RailwayStationEditViewModel(_unitOfWork, railwayStation);
                _windowManager.ShowWindow(editViewModel);
            }
            else
            {
                MessageBox.Show("Вокзала с именеем {0} не найденно", railwayStationName);
            }












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

            //var stations = _unitOfWork.StationRepository.Get().OrderBy(x => x.Id).ToList();

            //Добавление
            //_unitOfWork.RailwayStationRepository.Insert(new RailwayStation { Name = "Вокзал 3", Stations = new List<Station>(stations.Skip(0)) });
            //await _unitOfWork.SaveAsync();


            // var operativeSh= _unitOfWork.RailwayStationRepository.Search(r => r.Name == "Вокзал 2").Include(r => r.AllStations).First();
            // var operative = _unitOfWork.RailwayStationRepository.Get().Include(r => r.AllStations).ToList();



            //var editStation = stations[0];
            //editStation.Name = "New NAME!!";
            //_unitOfWork.StationRepository.Update(editStation);
            //await _unitOfWork.SaveAsync();




            //Обновление
            //var rs = _unitOfWork.RailwayStationRepository.GetById(2);  //_unitOfWork.RailwayStationRepository.Get().OrderBy(x => x.Id).ToList().First();
            //rs.Stations.Clear();
            //rs.Stations = stations.Skip(0).Take(4).ToList();
            //rs.Name = "gfdgd";
            //_unitOfWork.RailwayStationRepository.Update(rs);
            //await _unitOfWork.SaveAsync();


            //удаление
            //var rs = _unitOfWork.RailwayStationRepository.GetById(1);  //_unitOfWork.RailwayStationRepository.Get().OrderBy(x => x.Id).ToList().First();
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