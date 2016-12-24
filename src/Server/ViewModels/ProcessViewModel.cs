using System;
using System.Linq;
using Caliburn.Micro;
using DataExchange.Event;
using DataExchange.InitDb;
using Domain.Entities;


namespace Server.ViewModels
{
    public sealed class ProcessViewModel : Screen, IHandle<InitDbFromXmlStatus>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly Station _stationOwner;


        public ProcessViewModel(IEventAggregator events, Station stationOwner)
        {
            _stationOwner = stationOwner;
            _eventAggregator = events;
            events.Subscribe(this);

            MaxProcess = (Enum.GetValues(typeof(Status)).Cast<int>().Max() + 1 ) * 10;
            MinProcess = (Enum.GetValues(typeof(Status)).Cast<int>().Min() + 1 ) * 10;

            DisplayName = "Загрузка данных...";
        }






        #region prop

        private string _statusString;

        public string StatusString
        {
            get { return _statusString; }
            set
            {
                _statusString = value;
                NotifyOfPropertyChange(() => StatusString);
            }
        }


        private int _maxProcess;

        public int MaxProcess
        {
            get { return _maxProcess; }
            set
            {
                _maxProcess = value;
                NotifyOfPropertyChange(() => MaxProcess);
            }
        }


        private int _minProcess;

        public int MinProcess
        {
            get { return _minProcess; }
            set
            {
                _minProcess = value;
                NotifyOfPropertyChange(() => MinProcess);
            }
        }


        private int _valueProcess;

        public int ValueProcess
        {
            get { return _valueProcess; }
            set
            {
                _valueProcess = value;
                NotifyOfPropertyChange(() => ValueProcess);
            }
        }


        public BindableCollection<InitDbFromXmlStatus> ImportantMessage { get; set; } = new BindableCollection<InitDbFromXmlStatus>();

        #endregion




        #region EventHandler

        public void Handle(InitDbFromXmlStatus message)
        {
            if (ReferenceEquals(_stationOwner, message.OwnerStation))
            {
                StatusString = message.StatusString;
                ValueProcess = message.Status == Status.Ok ? 100: (int)message.Status * 10;
                ImportantMessage.Add(message);
            }
        }

        #endregion



        protected override void OnDeactivate(bool close)
        {
            _eventAggregator?.Unsubscribe(this);

            base.OnDeactivate(close);
        }
    }
}