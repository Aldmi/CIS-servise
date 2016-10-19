using System;
using System.Windows;
using Caliburn.Micro;

namespace Server.ViewModels
{
    public class AppViewModel : Conductor<object>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;


        public IDisposable DispouseOrderStatusProcess { get; set; }
        public IDisposable DispouseOrderStatusAcsess { get; set; }



        public AppViewModel( IEventAggregator events, IWindowManager windowManager)//IEventAggregator events, ColorViewModel colorModel, IWindowManager windowManager, GreenViewModel greenViewModel
        {
            _windowManager = windowManager;

            _eventAggregator = events;
            events.Subscribe(this);
        }






        public void NewWindow()
        {
            var dialogViewModel= new DialogViewModel();
            var result= _windowManager.ShowDialog(dialogViewModel);

            if (result != null && result.Value)
            {
                MessageBox.Show("Ok");
            }
            else
            {
                MessageBox.Show("Cancel");
            }
        }






        protected override void OnDeactivate(bool close)
        {
            _eventAggregator.Unsubscribe(this);
            base.OnDeactivate(close);
        }
 
    }
}