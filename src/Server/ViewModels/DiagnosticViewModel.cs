using System.Linq;
using Caliburn.Micro;
using Domain.Entities;
using Server.Event;
using WCFCis2AvtodictorContract.DataContract;


namespace Server.ViewModels
{
    public class DiagnosticViewModel : Screen, IHandle<AutodictorDiagnosticEvent>       //TODO: попробовать IHandleWithTask<>
    {
        public string Name { get; set; }
        public BindableCollection<DiagnosticData> Diagnostics { get; set; } = new BindableCollection<DiagnosticData>();



        public DiagnosticViewModel(string name, IEventAggregator events)
        {
            Name = name;
            events.Subscribe(this);
        }




        public void Handle(AutodictorDiagnosticEvent message)
        {
            if (message.NameRailwayStation == Name)
            {
                Diagnostics.Clear();
                Diagnostics.AddRange(message.DiagnosticData);
            }
        }
    }
}