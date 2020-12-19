#nullable enable

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace TrippitKiosk.Services
{
    public class RaspberryPiDisplayService
    {
        public event EventHandler<byte>? BrightnessChanged;

        private const byte DISPLAY_ADDRESS = 0x45;
        private const byte BACKLIGHT_REGISTER_ADDRESS = 0x86;

        private readonly UserIdleService _idleService;

        private byte? _preIdleBrightness = null;
        private Timer _adjustmentThrottle;
        private object _queueLock = new object();
        private ConcurrentStack<byte> _brightnessAdjustQueue = new ConcurrentStack<byte>();
        private I2cDevice? _display = null;

        public RaspberryPiDisplayService(UserIdleService idleService)
        {
            _adjustmentThrottle = new Timer(AdjustmentThrottle_Tick);
            _idleService = idleService;

            _idleService.UserIdleChanged += IdleService_UserIdleChanged;
        }

        public async Task Initialize()
        {
            if (_display == null)
            {
                string i2cDeviceSelector = I2cDevice.GetDeviceSelector();
                DeviceInformation? i2cDisplayConnection = (await DeviceInformation.FindAllAsync(i2cDeviceSelector)).FirstOrDefault();
                if (i2cDisplayConnection == null)
                {
                    Debug.WriteLine("Couldn't find any I2C devices.");
                    return;
                }
                var settings = new I2cConnectionSettings(DISPLAY_ADDRESS);
                _display = await I2cDevice.FromIdAsync(i2cDisplayConnection.Id, settings);
            }
        }

        public async Task AdjustBrightness(byte newBrightnessValue)
        {
            // Run on a background thread--none of this actually touches XAML UI code.
            await Task.Run(async () =>
            {
                await Initialize();

                lock (_queueLock)
                {
                    _brightnessAdjustQueue.Push(newBrightnessValue);
                    _adjustmentThrottle.Change(250, 250);
                }
            });
        }

        public async Task<byte?> GetBrightness()
        {
            if (_display == null)
            {
                return null;
            }

            byte brightness = await Task.Run(async () =>
            {
                await Initialize();

                byte[] readBuffer = new byte[] { 0 };
                try
                {
                    _display.Read(readBuffer);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to read backlight brightness. Details: {ex}");
                }

                return readBuffer[0];
            });

            return brightness;
        }

        private void AdjustmentThrottle_Tick(object state)
        {
            if (_display == null)
            {
                return;
            }

            lock (_queueLock)
            {
                if (_brightnessAdjustQueue.TryPop(out byte newBrightnessValue))
                {
                    byte[] writeBuffer = new byte[] { BACKLIGHT_REGISTER_ADDRESS, newBrightnessValue };
                    try
                    {
                        _display.Write(writeBuffer);
                        BrightnessChanged?.Invoke(this, newBrightnessValue);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Failed to update backlight brightness: {ex}");
                    }
                }
                _brightnessAdjustQueue.Clear();
                _adjustmentThrottle.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        private async void IdleService_UserIdleChanged(object sender, bool isUserIdle)
        {
            // If the user goes idle, save their brightness, then dim to 0.
            if (isUserIdle)
            {
                Debug.WriteLine("User went idle. Dimming the screen.");
                int readAttempts = 0;
                while ((_preIdleBrightness = await GetBrightness()) == null)
                {
                    readAttempts++;
                    if (readAttempts > 5)
                    {
                        Debug.WriteLine($"Failed to read current brightness. Defaulting to 128.");
                        _preIdleBrightness = 128;
                        break;
                    }
                    await Task.Delay(500);
                }

                Debug.WriteLine("User is idle, setting brightness to 0.");
                await AdjustBrightness(10);
            }
            else
            {
                Debug.WriteLine($"User has returned from idle, restoring brightness to {_preIdleBrightness}.");
                await AdjustBrightness(_preIdleBrightness ?? 128);
            }


        }
    }
}
