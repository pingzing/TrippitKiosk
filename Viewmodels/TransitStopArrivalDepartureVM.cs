using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TrippitKiosk.Models;
using Windows.UI.Xaml;

namespace TrippitKiosk.Viewmodels
{
    public class TransitStopArrivalDepartureVM : INotifyPropertyChanged
    {
        private TransitStopArrivalDeparture _backingData;
        public TransitStopArrivalDeparture BackingData
        {
            get => _backingData;
            set
            {
                if (_backingData == value)
                {
                    return;
                }
                _backingData = value;
                RaisePropertyChanged();
            }
        }

        private int? _minutesTillDeparture = null;
        public int? MinutesTillDeparture
        {
            get => _minutesTillDeparture;
            set
            {
                if (_minutesTillDeparture == value)
                {
                    return;
                }

                _minutesTillDeparture = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(MinutesTillDepartureMessage));
            }
        }

        private string _statusText = "";
        public string StatusText
        {
            get => _statusText;
            set
            {
                if (_statusText == value)
                {
                    return;
                }

                _statusText = value;
                RaisePropertyChanged();
            }
        }

        private ScheduleState _scheduleState = ScheduleState.Normal;
        public ScheduleState ScheduleState
        {
            get => _scheduleState;
            set
            {
                if (_scheduleState == value)
                {
                    return;
                }

                _scheduleState = value;
                RaisePropertyChanged();
            }
        }

        public string MinutesTillDepartureMessage => $"{MinutesTillDeparture} min.";

        private DispatcherTimer _updateMinutesTillTimer = new DispatcherTimer();

        public TransitStopArrivalDepartureVM(TransitStopArrivalDeparture backingData)
        {
            _backingData = backingData;

            _updateMinutesTillTimer.Tick += UpdateMinutesTillTimer_Tick;
            var now = DateTime.Now;
            var nowTime = now.TimeOfDay;
            int secondsTillNextMinute = 60 - nowTime.Seconds;
            if (secondsTillNextMinute == 0) { secondsTillNextMinute = 60; }
            int minutesTillDeparture = Math.Max((int)(BackingData.RealtimeDeparture - now).TotalMinutes, 0);
            MinutesTillDeparture = minutesTillDeparture;
            _updateMinutesTillTimer.Interval = TimeSpan.FromSeconds(secondsTillNextMinute);
            _updateMinutesTillTimer.Start();

            if (backingData.ArrivalDelaySeconds > 0)
            {
                ScheduleState = ScheduleState.Ahead;
                StatusText = "Ahead of schedule";
            }
            if (backingData.ArrivalDelaySeconds < 0)
            {
                ScheduleState = ScheduleState.Delayed;
                StatusText = "Delayed";
            }
        }

        private void UpdateMinutesTillTimer_Tick(object sender, object e)
        {
            if (_updateMinutesTillTimer.Interval != TimeSpan.FromSeconds(60))
            {
                _updateMinutesTillTimer.Interval = TimeSpan.FromSeconds(60);
            }

            var now = DateTime.Now;
            int minutesTillDeparture = Math.Max((int)(BackingData.RealtimeDeparture - now).TotalMinutes, 0);
            MinutesTillDeparture = minutesTillDeparture;
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public enum ScheduleState
    {
        Normal,
        Ahead,
        Delayed
    }
}
