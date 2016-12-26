using System.Linq;
using Caliburn.Micro;
using Domain.Entities;
using Server.Event;
using WCFCis2AvtodictorContract.DataContract;


namespace Server.ViewModels
{
    public class DiagnosticViewModel : Screen, IHandle<AutodictorDiagnosticEvent>       //TODO: попробовать IHandleWithTask<>
    {
        private readonly IEventAggregator _eventAggregator;
        public BindableCollection<DiagnosticData> Diagnostics { get; set; } = new BindableCollection<DiagnosticData>();



        public Station StationOwner { get; private set; }




        public DiagnosticViewModel(Station stationOwner, IEventAggregator events)
        {
            StationOwner = stationOwner;
            _eventAggregator = events;
            events.Subscribe(this);
        }




        #region EventHandler

        public void Handle(AutodictorDiagnosticEvent message)
        {
            if (message.NameRailwayStation == StationOwner.Name)
            {
                Diagnostics.Clear();
                Diagnostics.AddRange(message.DiagnosticData);
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