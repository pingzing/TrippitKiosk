using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace TrippitKiosk.Services
{
    public class UserIdleService
    {
        public event EventHandler<bool> UserIdleChanged;

        private int _isIdleBacking = 0;
        private bool IsIdle
        {
            get => Interlocked.CompareExchange(ref _isIdleBacking, 1, 1) == 1;
            set
            {
                if (value)
                {
                    Interlocked.CompareExchange(ref _isIdleBacking, 1, 0);
                }
                else
                {
                    Interlocked.CompareExchange(ref _isIdleBacking, 0, 1);
                }
            }
        }

        private Timer _idleTimer;

        public UserIdleService()
        {
            _idleTimer = new Timer(UserIdle);

            var coreWindow = CoreWindow.GetForCurrentThread();
            coreWindow.PointerMoved += CoreWindow_PointerMoved;
            coreWindow.PointerPressed += CoreWindow_PointerPressed;
            coreWindow.PointerWheelChanged += CoreWindow_PointerWheelChanged;
            coreWindow.KeyDown += CoreWindow_KeyDown;
        }

        private async void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            await ResetIdleTimer();
        }

        private async void CoreWindow_PointerWheelChanged(CoreWindow sender, PointerEventArgs args)
        {
            await ResetIdleTimer();
        }

        private async void CoreWindow_PointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            await ResetIdleTimer();
        }

        private async void CoreWindow_PointerMoved(CoreWindow sender, PointerEventArgs args)
        {
            await ResetIdleTimer();
        }

        public async Task ReportUserInteraction()
        {
            await ResetIdleTimer();
        }

        private async Task ResetIdleTimer()
        {
            Debug.WriteLine("Resetting the idle timer.");
            await Task.Run(() =>
            {
                if (IsIdle)
                {
                    IsIdle = false;
                    UserIdleChanged?.Invoke(this, false);
                }
            });

            // Reset timer
            _idleTimer.Change(TimeSpan.FromMinutes(2), Timeout.InfiniteTimeSpan);
        }

        private void UserIdle(object _)
        {
            Debug.WriteLine("User went idle.");
            IsIdle = true;
            UserIdleChanged?.Invoke(this, true);
        }
    }
}
