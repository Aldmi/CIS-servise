using System;
using System.Collections.Generic;
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






        public async void NewWindow()
        {
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
            var stations= _unitOfWork.StationRepository.Get().OrderBy(x=>x.Id).ToList();
            _unitOfWork.OperativeScheduleRepository.Insert(new OperativeSchedule { NumberOfTrain = 12, ArrivalTime = DateTime.Now, DepartureTime = DateTime.Today, RouteName = "Маршрут 3", DispatchStation = stations[2], StationOfDestination = stations[3], ListWithoutStops = new List<Station>(stations)});
            await _unitOfWork.SaveAsync();

             var operativeSh= _unitOfWork.OperativeScheduleRepository.Get().ToList();

        }






        protected override void OnDeactivate(bool close)
        {
            _eventAggregator.Unsubscribe(this);
            base.OnDeactivate(close);
        }
 
    }
}