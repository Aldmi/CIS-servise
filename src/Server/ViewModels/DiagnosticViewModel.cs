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
        public string Name { get; set; }  //TODO: заменить на StationOwner
        public BindableCollection<DiagnosticData> Diagnostics { get; set; } = new BindableCollection<DiagnosticData>();




        public DiagnosticViewModel(string name, IEventAggregator events)
        {
            Name = name;
            _eventAggregator = events;
            events.Subscribe(this);
        }




        #region EventHandler

        public void Handle(AutodictorDiagnosticEvent message)
        {
            if (message.NameRailwayStation == Name)
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