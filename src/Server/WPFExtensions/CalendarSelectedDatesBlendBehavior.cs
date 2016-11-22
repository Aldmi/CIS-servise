using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;


namespace Server.WPFExtensions
{
    public class CalendarSelectedDatesBlendBehavior : Behavior<Calendar>
    {
        public static readonly DependencyProperty SelectedDatesProperty =
         DependencyProperty.Register("SelectedDates", typeof(ObservableCollection<DateTime>),
         typeof(CalendarSelectedDatesBlendBehavior),
         new FrameworkPropertyMetadata(null, OnSelectedDatesChanged) { BindsTwoWayByDefault = true });



        public ObservableCollection<DateTime> SelectedDates
        {
            get { return (ObservableCollection<DateTime>)GetValue(SelectedDatesProperty); }
            set { SetValue(SelectedDatesProperty, value); }
        }



        private static Calendar _calendar;
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SelectedDatesChanged += OnCalendarSelectedDatesChanged;
            _calendar = this.AssociatedObject;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (this.AssociatedObject != null)
                this.AssociatedObject.SelectedDatesChanged -= OnCalendarSelectedDatesChanged;
            if (_sourceCollection != null)
                _sourceCollection.CollectionChanged -= dates_CollectionChanged;
        }


        private void OnCalendarSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_collectionSetFromSource)
            {
                if (e.AddedItems != null && e.AddedItems.Count > 0)
                {
                    IEnumerable<DateTime> addedDates = e.AddedItems.OfType<DateTime>();
                    foreach (DateTime dt in addedDates)
                        this.SelectedDates.Add(dt);
                }

                if (e.RemovedItems != null && e.RemovedItems.Count > 0)
                {
                    IEnumerable<DateTime> removedDates = e.RemovedItems.OfType<DateTime>();
                    foreach (DateTime dt in removedDates)
                        this.SelectedDates.Remove(dt);
                }
            }
        }

        private static bool _collectionSetFromSource;
        private static ObservableCollection<DateTime> _sourceCollection;
        private static void OnSelectedDatesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            _sourceCollection = e.NewValue as ObservableCollection<DateTime>;
            if (_sourceCollection != null)
            {
                _sourceCollection.CollectionChanged += dates_CollectionChanged;
                foreach (DateTime dt in _sourceCollection)
                {
                    _collectionSetFromSource = true;
                    _calendar.SelectedDates.Add(dt);
                    _collectionSetFromSource = false;
                }
            }
        }


        private static void dates_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null && e.NewItems.Count > 0)
                    {
                        IEnumerable<DateTime> addedDates = e.NewItems.OfType<DateTime>();
                        foreach (DateTime dt in addedDates)
                        {
                            if (!_calendar.SelectedDates.Contains(dt))
                            {
                                _collectionSetFromSource = true;
                                _calendar.SelectedDates.Add(dt);
                                _collectionSetFromSource = false;
                            }
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null && e.OldItems.Count > 0)
                    {
                        IEnumerable<DateTime> removedDates = e.OldItems.OfType<DateTime>();
                        foreach (DateTime dt in removedDates)
                        {
                            if (_calendar.SelectedDates.Contains(dt))
                            {
                                _collectionSetFromSource = true;
                                _calendar.SelectedDates.Remove(dt);
                                _collectionSetFromSource = false;
                            }
                        }
                    }
                    break;
            }
        }
    }
}