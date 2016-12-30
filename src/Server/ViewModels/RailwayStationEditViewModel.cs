using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using Caliburn.Micro;
using Domain.Abstract;
using Domain.Entities;
using Castle.Facilities.WcfIntegration;
using Castle.Windsor;



namespace Server.ViewModels
{
    public enum Options
    {   
        None,
        OperativeSchedule,
        RegulatorySchedule,
        Diagnostic,
        Info,
        Station
    }


    public sealed class RailwayStationEditViewModel : Screen
    {
        private readonly IWindsorContainer _windsorContainer;
        private readonly Station _stationOwner;
        private IUnitOfWork _unitOfWork;

        public RailwayStation RailwayStation { get; set; }


        private ObservableCollection<Station> _stations;
        public ObservableCollection<Station> Stations
        {
            get { return _stations; }
            set
            {
                _stations = value;
                NotifyOfPropertyChange(() => Stations);
            }
        }

        public Station SelectedItemStation { get; set; }


        private ObservableCollection<OperativeSchedule> _operativeSchedules;
        public ObservableCollection<OperativeSchedule> OperativeSchedules
        {
            get { return _operativeSchedules; }
            set
            {
                _operativeSchedules = value;
                NotifyOfPropertyChange(() => OperativeSchedules);
            }
        }

        public OperativeSchedule SelectedItemOperativeSchedule { get; set; }


        private ObservableCollection<RegulatorySchedule> _regulatorySchedules;
        public ObservableCollection<RegulatorySchedule> RegulatorySchedules
        {
            get { return _regulatorySchedules; }
            set
            {
                _regulatorySchedules = value;
                NotifyOfPropertyChange(() => RegulatorySchedules);
            }
        }

        public RegulatorySchedule SelectedItemRegulatorySchedule { get; set; }
  

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


        private Options _currentOption;
        public Options CurrentOption
        {
            get { return _currentOption; }

            set
            {
                if (value != CurrentOption)
                {
                    if (CurrentOption != Options.None)
                    {
                        var result = MessageBox.Show("Все несохраненные данные будут потерянны. Продолжить переход?",
                            "ВНИМАНИЕ", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.No)
                        {
                            return;
                        }
                    }


                    // переоткрыли контекст
                    if (_windsorContainer != null && _unitOfWork != null)
                    {
                        _unitOfWork.Dispose();
                        _windsorContainer.Release(_unitOfWork);
                    }

                    _unitOfWork = _windsorContainer.Resolve<IUnitOfWork>();
                    _currentOption = value;

                
                        switch (CurrentOption)
                        {
                            case Options.Station:
                                ShowBusyIndicator(true, "Идет загрузка списка станций из БД");
                                GetRailwayStation().ContinueWith(task =>
                                {
                                    try
                                    {
                                        var railwayStation = task.Result;
                                        if (railwayStation != null)
                                        {
                                            RailwayStation = railwayStation;
                                            Stations = new ObservableCollection<Station>(railwayStation.Stations);
                                            CountLine = Stations.Count;
                                            ShowBusyIndicator(false);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        MessageBox.Show($"Ошибка работы с БД.");
                                        ShowBusyIndicator(false);
                                    }

                                });
                                break;


                            case Options.RegulatorySchedule:
                                ShowBusyIndicator(true, "Идет загрузка регулярного расписания из БД");
                                GetRailwayStation().ContinueWith(task =>
                                {
                                    try
                                    {
                                        var railwayStation = task.Result;
                                        if (railwayStation != null)
                                        {
                                            RailwayStation = railwayStation;
                                            Stations = new ObservableCollection<Station>(railwayStation.Stations);
                                            RegulatorySchedules = new ObservableCollection<RegulatorySchedule>(railwayStation.RegulatorySchedules);
                                            CountLine = RegulatorySchedules.Count;
                                            ShowBusyIndicator(false);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        MessageBox.Show($"Ошибка работы с БД.");
                                        ShowBusyIndicator(false);
                                    }
                                });
                                break;


                            case Options.OperativeSchedule:
                                ShowBusyIndicator(true, "Идет загрузка оперативного расписания из БД");
                                GetRailwayStation().ContinueWith(task =>
                                {
                                    try
                                    {
                                        var railwayStation = task.Result;
                                        if (railwayStation != null)
                                        {
                                            RailwayStation = railwayStation;
                                            Stations = new ObservableCollection<Station>(railwayStation.Stations);
                                            OperativeSchedules = new ObservableCollection<OperativeSchedule>(railwayStation.OperativeSchedules);
                                            CountLine = OperativeSchedules.Count;
                                            ShowBusyIndicator(false);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"Ошибка работы с БД.");
                                        ShowBusyIndicator(false);
                                    }
                                });
                                break;
                        }


                }
            }
        }


        private int _countLine;
        public int CountLine
        {
            get { return _countLine; }
            set
            {
                _countLine = value;
                NotifyOfPropertyChange(() => CountLine);
            }
        }


        private DateTime _datFormation;
        public DateTime DatFormation
        {
            get { return _datFormation; }
            set
            {
                _datFormation = value;
                NotifyOfPropertyChange(() => DatFormation);
            }
        }




        #region ctor

        public RailwayStationEditViewModel(IWindsorContainer windsorContainer, Station stationOwner)
        {
            _windsorContainer = windsorContainer;
            _stationOwner = stationOwner;

            CurrentOption = Options.OperativeSchedule;

            DisplayName = stationOwner.Name.ToUpper() + " ВОКЗАЛ";
        }

        #endregion





        #region Methods

        async Task<RailwayStation> GetRailwayStation() //TODO: добавить аргумент для Include нужной таблицы
        {
            return await await Task.Factory.StartNew(async () =>
            {
                var query = _unitOfWork.RailwayStationRepository.Search(r => r.Name == _stationOwner.Name)
                .Include(s => s.Stations)
                .Include(op => op.OperativeSchedules)
                .Include(reg => reg.RegulatorySchedules);
                return await query.FirstOrDefaultAsync();
            });
        }


        public void Add()
        {
            switch (CurrentOption)
            {
                case Options.Station:
                    var newStation = new Station { Name = string.Empty, Description = string.Empty, EcpCode = 0, RailwayStations = new List<RailwayStation> { RailwayStation } };
                    Stations.Add(newStation);
                    CountLine = Stations.Count;
                    break;

                case Options.OperativeSchedule:
                    var newOperSh = new OperativeSchedule {ArrivalTime= DateTime.Now, DepartureTime= DateTime.Now, RouteName = string.Empty, NumberOfTrain = string.Empty, ListOfStops = new ObservableCollection<Station>(), ListWithoutStops = new ObservableCollection<Station>()};
                    OperativeSchedules.Add(newOperSh);
                    CountLine = OperativeSchedules.Count;
                    break;

                case Options.RegulatorySchedule:
                    var newRegSh = new RegulatorySchedule { ArrivalTime = DateTime.Now, DepartureTime = DateTime.Now, RouteName = string.Empty, NumberOfTrain = string.Empty, DaysFollowings = null, ListOfStops = new ObservableCollection<Station>(), ListWithoutStops = new ObservableCollection<Station>() };
                    RegulatorySchedules.Add(newRegSh);
                    CountLine = RegulatorySchedules.Count;
                    break;
            }
        }



        public void Del()
        {
            switch (CurrentOption)
            {
                case Options.Station:
                    Stations.Remove(SelectedItemStation);
                    CountLine = Stations.Count;
                    break;

                case Options.OperativeSchedule:
                    OperativeSchedules.Remove(SelectedItemOperativeSchedule);
                    CountLine = OperativeSchedules.Count;
                    break;

                case Options.RegulatorySchedule:
                    RegulatorySchedules.Remove(SelectedItemRegulatorySchedule);
                    CountLine = RegulatorySchedules.Count;
                    break;
            }


        }



        public async void Save()
        {
            ShowBusyIndicator(true, "Идет сохранение в БД");
            await Task.Delay(500);

            //обновить изменения сушествующих записей
            _unitOfWork.RailwayStationRepository.Update(RailwayStation);


            //изменения в количестве элементов
            switch (CurrentOption)
            {
                case Options.Station:
                    //ДОБАВЛЕННЫЕ
                    var addedStations = Stations.Except(RailwayStation.Stations).ToList();
                    var errorNameStation = addedStations.Where(s => string.IsNullOrEmpty(s.Name)).ToList();
                    if (errorNameStation.Any())
                    {
                        MessageBox.Show("В добавленных станциях не указанно название станции", "ОШИБКА СОХРАНЕНИЯ", MessageBoxButton.OK, MessageBoxImage.Error);
                        ShowBusyIndicator(false);
                        return;
                    }
                    var errorEcpStation = addedStations.Where(s => s.EcpCode < 1).ToList();
                    if (errorEcpStation.Any())
                    {
                        MessageBox.Show("В добавленных станциях ECP код меньше 1", "ОШИБКА СОХРАНЕНИЯ", MessageBoxButton.OK, MessageBoxImage.Error);
                        ShowBusyIndicator(false);
                        return;
                    }
                    foreach (var addStation in addedStations)
                    {
                        _unitOfWork.StationRepository.Insert(addStation);
                    }

                    //УДАЛЕННЫЕ
                    var removedStations = RailwayStation.Stations.Except(Stations).ToList();
                    foreach (var removedStation in removedStations)
                    {
                        //TODO: испроавить EXCEPTION удаления станции
                        _unitOfWork.StationRepository.Remove(removedStation);
                    }
                    break;




                case Options.OperativeSchedule:
                    //ДОБАВЛЕННЫЕ
                    var addedOperShs = OperativeSchedules.Except(RailwayStation.OperativeSchedules).ToList();
                    var errorNumberOfTrainOp = addedOperShs.Where(op=> string.IsNullOrEmpty(op.NumberOfTrain) || op.NumberOfTrain.Length > 10).ToList();
                    if (errorNumberOfTrainOp.Any())
                    {
                        MessageBox.Show("В добавленных строках оперативного расписания не верно указанн номер поезда", "ОШИБКА СОХРАНЕНИЯ", MessageBoxButton.OK, MessageBoxImage.Error);
                        ShowBusyIndicator(false);
                        return;
                    }
                    var errorRouteNameOp = addedOperShs.Where(op => string.IsNullOrEmpty(op.RouteName) || op.RouteName.Length > 100).ToList();
                    if (errorRouteNameOp.Any())
                    {
                        MessageBox.Show("В добавленных строках оперативного расписания не верно указанн маршрут", "ОШИБКА СОХРАНЕНИЯ", MessageBoxButton.OK, MessageBoxImage.Error);
                        ShowBusyIndicator(false);
                        return;
                    }
                    foreach (var addedOperSh in addedOperShs)
                    {
                        RailwayStation.OperativeSchedules.Add(addedOperSh);
                    }

                    //УДАЛЕННЫЕ
                    var removedOperShs = RailwayStation.OperativeSchedules.Except(OperativeSchedules).ToList();
                    foreach (var removedOperSh in removedOperShs)
                    {
                        _unitOfWork.OperativeScheduleRepository.Remove(removedOperSh);
                    }                 
                    _unitOfWork.RailwayStationRepository.Update(RailwayStation);
                    break;




                case Options.RegulatorySchedule:
                    //ДОБАВЛЕННЫЕ
                    var addedRegShs = RegulatorySchedules.Except(RailwayStation.RegulatorySchedules).ToList();
                    var errorNumberOfTrainReg = addedRegShs.Where(op => string.IsNullOrEmpty(op.NumberOfTrain) || op.NumberOfTrain.Length > 10).ToList();
                    if (errorNumberOfTrainReg.Any())
                    {
                        MessageBox.Show("В добавленных строках регулярного расписания не верно указанн номер поезда", "ОШИБКА СОХРАНЕНИЯ", MessageBoxButton.OK, MessageBoxImage.Error);
                        ShowBusyIndicator(false);
                        return;
                    }
                    var errorRouteNameReg = addedRegShs.Where(op => string.IsNullOrEmpty(op.RouteName) || op.RouteName.Length > 100).ToList();
                    if (errorRouteNameReg.Any())
                    {
                        MessageBox.Show("В добавленных строках регулярного расписания не верно указанн маршрут", "ОШИБКА СОХРАНЕНИЯ", MessageBoxButton.OK, MessageBoxImage.Error);
                        ShowBusyIndicator(false);
                        return;
                    }
                    foreach (var addedRegSh in addedRegShs)
                    {
                        RailwayStation.RegulatorySchedules.Add(addedRegSh);
                    }

                    //УДАЛЕННЫЕ
                    var removedRegShs = RailwayStation.RegulatorySchedules.Except(RegulatorySchedules).ToList();
                    foreach (var removedRegSh in removedRegShs)
                    {
                       _unitOfWork.RegulatoryScheduleRepository.Remove(removedRegSh);
                    }
                    _unitOfWork.RailwayStationRepository.Update(RailwayStation);
                    break;
            }



            //СОХРАНИМ ИЗМЕНЕНИЯ В БД одним запросом
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


        public void EditDaysFollowings(RegulatorySchedule param)
        {
            var daysFollowing = param.DaysFollowings;

            // открыть окно редактирования для строки daysFollowing, полученный результат присвоить param.DaysFollowings

            param.DaysFollowings = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }

        #endregion



        #region OvverrideMembers

        protected override void OnDeactivate(bool close)
        {
            if (_windsorContainer != null && _unitOfWork != null)
            {
                _unitOfWork.Dispose();
                _windsorContainer.Release(_unitOfWork);
            }

            base.OnDeactivate(close);
        }

        #endregion

    }
}