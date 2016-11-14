using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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


        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }

        private string _messageBusy;

        public string MessageBusy
        {
            get { return _messageBusy; }
            set
            {
                _messageBusy = value;
                NotifyOfPropertyChange(() => MessageBusy);
            }
        }




        public RailwayStationEditViewModel(IUnitOfWork unitOfWork, RailwayStation railwayStation)
        {
            _unitOfWork = unitOfWork;
            RailwayStation = railwayStation;

            Stations = new ObservableCollection<Station>(RailwayStation.Stations);
            OperativeSchedules = new ObservableCollection<OperativeSchedule>(RailwayStation.OperativeSchedules);
        }




        #region Methods

        public async void Save()
        {
            ShowBusyIndicator(true, "Идет сохранение в БД");
            await Task.Delay(500);
            _unitOfWork.RailwayStationRepository.Update(RailwayStation);
            await _unitOfWork.SaveAsync();
            ShowBusyIndicator(false);
        }


        public void Clouse()
        {
            TryClose(false);
        }


        //TODO:вынести все что касается BusyIndicator в базовый класс ViewModel. Наследовать от него все VM.
        private void ShowBusyIndicator(bool? isBusy = null, string message = null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                MessageBusy = message;
            }
            if (isBusy.HasValue)
            {
                IsBusy = isBusy.Value;
            }
        }


        public async void AddNewElement(int number)
        {
            //в зависмости от выбранной таблице CurrentOption
            // вызывается окно добавления нового элемента.
            // созданный элемент добавляется в коллекцию ObservableCollection<Т>, т.е. сразу отображается в списке
            // При нажатии на кнопку "Сохранить" вычисляются элементы которые были добавленны
            //на примере: OperativeSchedules  distinct  RailwayStation.OperativeSchedules
            // вычисленные добавленные элементы добавляются к коллекции (на примере: RailwayStation.OperativeSchedules)
        }



        //public async void NextPage(int number)
        //{
        //    //Получать unit of work надо заново.

        //    //using (var unitOfWork = _windsorContainer.Resolve<IUnitOfWork>())
        //    //{
        //    //    const string railwayStationName = "Вокзал 3";
        //    //    var query = unitOfWork.RailwayStationRepository.Search(r => r.Name == railwayStationName, null, "Stations, OperativeSchedules");
        //    //    var railwayStation = await query.FirstOrDefaultAsync();

        //    //    OperativeSchedules.Clear();
        //    //    OperativeSchedules.Add(railwayStation.OperativeSchedules.ToArray()[1]);
        //    //}
        //}

        #endregion
    }
}