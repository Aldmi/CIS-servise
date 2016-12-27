using System;
using System.Linq;
using Caliburn.Micro;
using DataExchange.Event;
using DataExchange.InitDb;
using Domain.Entities;
using Server.Event;
using WCFCis2AvtodictorContract.DataContract;


namespace Server.ViewModels
{
    public class DiagnosticViewModel : Screen, IHandle<AutodictorDiagnosticEvent>, IHandle<InitDbFromXmlStatus>
    {
        #region field

        private readonly IEventAggregator _eventAggregator;

        #endregion





        #region prop

        public Station StationOwner { get; private set; }

        public BindableCollection<DiagnosticData> Diagnostics { get; set; } = new BindableCollection<DiagnosticData>();


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


        public BindableCollection<InitDbFromXmlStatus> ImportantMessage { get; set; } =  new BindableCollection<InitDbFromXmlStatus>();
          
        #endregion





        #region ctor

        public DiagnosticViewModel(Station stationOwner, IEventAggregator events)
        {
            StationOwner = stationOwner;
            _eventAggregator = events;
            events.Subscribe(this);

            MaxProcess = (Enum.GetValues(typeof (Status)).Cast<int>().Max() + 1)*10;
            MinProcess = (Enum.GetValues(typeof (Status)).Cast<int>().Min()) *10;
        }

        #endregion





        #region EventHandler

        public void Handle(AutodictorDiagnosticEvent message)
        {
            if (message.NameRailwayStation == StationOwner.Name)
            {
                Diagnostics.Clear();
                Diagnostics.AddRange(message.DiagnosticData);
            }
        }


        public void Handle(InitDbFromXmlStatus message)
        {
            if (ReferenceEquals(StationOwner, message.OwnerStation))
            {
                if (message.Status == Status.Load)
                {
                    ImportantMessage.Clear();
                }

                ValueProcess = message.Status == Status.Ok ? 100 : (int)(message.Status + 1) * 10;
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