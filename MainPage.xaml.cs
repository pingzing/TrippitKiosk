#nullable enable

using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TrippitKiosk.Extensions;
using TrippitKiosk.Models;
using TrippitKiosk.Services;
using TrippitKiosk.Viewmodels;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;

namespace TrippitKiosk
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private const string MqttEndpoint = "mqtt.hsl.fi";

        private const int BaselineZIndex = 0;
        private const int MouseoverZIndex = 5;
        private const int ClickedZIndex = 10;

        private readonly HslApiService _hslApiService;
        private readonly HslMqttService _hslMqttService;
        private readonly UserIdleService _idleService;
        private readonly RaspberryPiDisplayService _displayService;

        private double _mapCenterLat;
        private double _mapCenterLon;
        private double _youAreHereLat;
        private double _youAreHereLon;
        private List<TransitStop>? _visibleStops;
        private Dictionary<int, MapVehicleIcon> _vehicleIcons = new Dictionary<int, MapVehicleIcon>();

        private string _selectedStopName;
        public string SelectedStopName
        {
            get => _selectedStopName;
            set
            {
                if (_selectedStopName == value)
                {
                    return;
                }
                _selectedStopName = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<TransitStopArrivalDepartureVM> SelectedStopDetails { get; } = new ObservableCollection<TransitStopArrivalDepartureVM>();

        public MainPage()
        {
            _idleService = new UserIdleService();
            _displayService = new RaspberryPiDisplayService(_idleService);
            this.InitializeComponent();
            _hslApiService = new HslApiService(new System.Net.Http.HttpClient());

            var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithClientId($"trippit-kiosk-{Guid.NewGuid()}")
                    .WithTcpServer(MqttEndpoint, 8883)
                    .WithTls()
                    .WithCleanSession())
                .Build();
            var mqttClient = new MqttFactory().CreateManagedMqttClient();
            _hslMqttService = new HslMqttService(mqttClient, options);
            _hslMqttService.VehiclePositionsUpdated += VehiclePositionsUpdated;

            StopDetailsListView.ContainerContentChanging += StopDetailsListView_ContainerContentChanging;

            MainMapControl.MapElementClick += MainMapControl_MapElementClick;
            MainMapControl.MapElementPointerEntered += MainMapControl_MapElementPointerEntered;
            MainMapControl.MapElementPointerExited += MainMapControl_MapElementPointerExited;
            MainMapControl.Tapped += MainMapControl_Tapped;

            StartClock();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TrippitKioskOptions? options = await LoadApplicationOptions();
            if (options == null)
            {
                throw new ArgumentNullException("settings.json could not be read. Does it exist?");
            }

            _mapCenterLat = options.DefaultCenterLat;
            _mapCenterLon = options.DefaultCenterLon;
            _youAreHereLat = options.YouAreHereLat;
            _youAreHereLon = options.YouAreHereLon;
            MainMapControl.MapServiceToken = options.MapApiKey;

            // Set up map stylesheet stuff
            string stylesheetText = @"
{
    ""version"": ""1.*"",
    ""extensions"": {
        ""trippitKiosk"": {
            ""selectedUserElement"": {
                ""scale"": 1.3, 
                ""strokeColor"": ""#8F0003FF""
            }
        }
    }
}";
            MainMapControl.StyleSheet = MapStyleSheet.Combine(new List<MapStyleSheet>
            {
                MapStyleSheet.RoadLight(),
                MapStyleSheet.ParseFromJson(stylesheetText)
            });

            MainMapControl.Center = new Geopoint(BasicGeopositionExtensions.Create(0.0, _mapCenterLat, _mapCenterLon));

            await RefreshStops();
            await _displayService.Initialize();
        }


        private async void MainMapControl_MapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            foreach (var layer in sender.Layers)
            {
                if (layer is MapElementsLayer elementLayer)
                {
                    foreach (var mapElement in elementLayer.MapElements)
                    {
                        mapElement.ZIndex = BaselineZIndex;
                        mapElement.MapStyleSheetEntryState = "";
                    }
                }
            }

            SelectedStopDetails.Clear();

            // TODO: Show loading state for side panel

            MapElement? firstClicked = args.MapElements.First();
            firstClicked.ZIndex = ClickedZIndex;
            firstClicked.MapStyleSheetEntryState = "trippitKiosk.selectedUserElement";
            var clickedStop = _visibleStops.FirstOrDefault(x => x.Id == (Guid)firstClicked.Tag);
            if (clickedStop == null)
            {
                return;
            }

            SelectedStopName = clickedStop.NameAndCode.ToUpperInvariant();

            var arrivalsAndDepartures = await _hslApiService.GetUpcomingStopArrivalsAndDepartures(clickedStop.GtfsId, CancellationToken.None);
            if (arrivalsAndDepartures == null)
            {
                // TODO: Display error in side panel
                return;
            }

            // TODO: Remove loading state here
            foreach (var detailVM in arrivalsAndDepartures
                .Select(x => new TransitStopArrivalDepartureVM(x))
                .OrderBy(x => x.BackingData.RealtimeArrival))
            {
                SelectedStopDetails.Add(detailVM);
            }
        }

        private void MainMapControl_MapElementPointerEntered(MapControl sender, MapElementPointerEnteredEventArgs args)
        {
            if (args.MapElement.ZIndex < MouseoverZIndex)
            {
                args.MapElement.ZIndex = MouseoverZIndex;
            }
        }

        private void MainMapControl_MapElementPointerExited(MapControl sender, MapElementPointerExitedEventArgs args)
        {
            int mouseoverZindex = args.MapElement.ZIndex;
            if (mouseoverZindex > BaselineZIndex && mouseoverZindex < ClickedZIndex)
            {
                args.MapElement.ZIndex = BaselineZIndex;
            }
        }

        // Apparently interacting with the map doesn't get picked up by the XAML event handling system, so report it ourselves.
        private async void MainMapControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await _idleService.ReportUserInteraction();
        }

        private async void VehiclePositionsUpdated(object sender, VehiclePositionsUpdatedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                foreach (var newPos in e.Positions)
                {
                    if (_vehicleIcons.TryGetValue(newPos.VehicleNumber, out MapVehicleIcon icon))
                    {
                        if (newPos.HeadingDegrees != null)
                        {
                            icon.RotationDegrees = newPos.HeadingDegrees.Value;
                        }
                        MapControl.SetLocation(icon, new Geopoint(BasicGeopositionExtensions.Create(0, newPos.Latitude.Value, newPos.Longitude.Value)));
                    }
                    else
                    {
                        MapVehicleIcon newIcon = new MapVehicleIcon();
                        newIcon.RouteText = newPos.FriendlyRouteNumber;
                        if (newPos.HeadingDegrees != null)
                        {
                            newIcon.Rotation = newPos.HeadingDegrees.Value;
                        }
                        _vehicleIcons.Add(newPos.VehicleNumber, newIcon);
                        MainMapControl.Children.Add(newIcon);
                        MapControl.SetLocation(newIcon, new Geopoint(BasicGeopositionExtensions.Create(0, newPos.Latitude.Value, newPos.Longitude.Value)));
                        MapControl.SetNormalizedAnchorPoint(newIcon, new Point(0.5, 0.5));
                    }
                }
            });
        }

        private async Task RefreshStops()
        {
            MainMapControl.Layers.Clear();

            _visibleStops = await _hslApiService.GetStopsByBoundingRadius((float)_mapCenterLat, (float)_mapCenterLon, 750, CancellationToken.None);
            if (_visibleStops == null)
            {
                // TODO: Report failure, add button to retry somewhere.
                Debug.WriteLine("Got null stops. =(");
                return;
            }

            var stopsList = new List<MapElement>();
            stopsList.AddRange(_visibleStops.Select(x =>
            {
                return new MapIcon
                {
                    CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible,

                    NormalizedAnchorPoint = new Point(0.5, 1.0),
                    Location = new Geopoint(x.Coords),
                    Title = x.NameAndCode,
                    ZIndex = 0,
                    Tag = x.Id,
                };
            }));

            var stopsLayer = new MapElementsLayer
            {
                MapElements = stopsList,
                ZIndex = 5,
            };

            MainMapControl.Layers.Add(stopsLayer);

            // Jiggle the map to force the text to appear
            await MainMapControl.TryPanAsync(1, 1);
            await MainMapControl.TryPanAsync(-1, -1);

            // Get the routes we're aware of, and subscribe to their vehicle via MQTT:
            var allStopDetails = await GetAllStopDetails(_visibleStops, CancellationToken.None);
            var routeIds = allStopDetails.SelectMany(x => x.LinesThroughStop)
                .Select(x => x.GtfsId);
            await _hslMqttService.Start(routeIds);
        }

        private async Task<List<TransitStopDetails>?> GetAllStopDetails(List<TransitStop> stops, CancellationToken token)
        {
            var tasks = stops.Select(x =>
            {
                return _hslApiService.GetStopDetails(x.GtfsId, DateTime.Today, token);
            });

            var completedTasks = await Task.WhenAll(tasks.Where(x => x != null));

            if (completedTasks == null)
            {
                return null;
            }

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            return completedTasks.ToList(); // nulls are filtered out by .Where() clause above
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
        }

        private async Task<TrippitKioskOptions?> LoadApplicationOptions()
        {
            var storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///settings.json"));
            string contents = await FileIO.ReadTextAsync(storageFile);
            return JsonSerializer.Deserialize<TrippitKioskOptions>(contents, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        private async void RelayoutMapButton_Click(object sender, RoutedEventArgs e)
        {
            MainMapControl.Width = 601;
            MainMapControl.ZoomLevel = 16.11;
            MainMapControl.InvalidateMeasure();
            await Task.Delay(1000);
            MainMapControl.Width = 600;
            MainMapControl.ZoomLevel = 16.10;
        }

        private async void BrightnessSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            // Prevents a user from accidentally going lower than 15, but still allows it to be set
            // programmatically
            byte newValue = (byte)e.NewValue;
            if (newValue <= 15)
            {
                newValue = 15;
            }
            await _displayService.AdjustBrightness(newValue);
        }

        private void StopDetailsListView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            // Set a clone of an EntranceViewTransition
            if (!args.InRecycleQueue)
            {
                var listViewItem = args.ItemContainer as ListViewItem;
                var panel = sender.FindDescendant<ItemsStackPanel>();
                var visual = ElementCompositionPreview.GetElementVisual(listViewItem);
                ElementCompositionPreview.SetIsTranslationEnabled(listViewItem, true);
                visual.Properties.InsertVector3("Translation", new Vector3(30f, 0, 0));
                visual.Opacity = 0;

                var index = sender.IndexFromContainer(listViewItem);

                var translateAnimation = visual.Compositor.CreateScalarKeyFrameAnimation();
                var decelerateEase = visual.Compositor.CreateCubicBezierEasingFunction(new Vector2(0.1f, 0.9f), new Vector2(0.2f, 1.0f));
                translateAnimation.Duration = TimeSpan.FromMilliseconds(333);
                translateAnimation.DelayTime = TimeSpan.FromMilliseconds(50 * index);
                translateAnimation.InsertKeyFrame(0.0f, 30f);
                translateAnimation.InsertKeyFrame(1.0f, 0f, decelerateEase);
                translateAnimation.Target = "Translation.X";

                var fadeAnimation = visual.Compositor.CreateScalarKeyFrameAnimation();
                fadeAnimation.Duration = TimeSpan.FromMilliseconds(333);
                fadeAnimation.DelayTime = TimeSpan.FromMilliseconds(50 * index);
                fadeAnimation.InsertKeyFrame(0.0f, 0f);
                fadeAnimation.InsertKeyFrame(1.0f, 1.0f);
                translateAnimation.Target = "Opacity";

                visual.StartAnimation("Translation.X", translateAnimation);
                visual.StartAnimation("Opacity", fadeAnimation);
            }
        }

        private DispatcherTimer _clockTimer;
        private DispatcherTimer _clockBlinkTimer;
        private void StartClock()
        {
            _clockTimer = new DispatcherTimer();
            _clockBlinkTimer = new DispatcherTimer();

            _clockTimer.Interval = TimeSpan.FromMinutes(1);
            _clockTimer.Tick += (s, e) =>
            {
                var now = DateTime.Now;
                ClockHours.Text = now.ToString("hh");
                ClockMinutesAndAmPm.Text = now.ToString("mmtt").ToUpperInvariant();

                int secondsTillNextMinute = 60 - now.Second;
                _clockTimer.Stop();
                _clockTimer.Interval = TimeSpan.FromSeconds(secondsTillNextMinute);
                _clockTimer.Start();
            };
            _clockTimer.Start();
            var now = DateTime.Now;
            ClockHours.Text = now.ToString("hh");
            ClockMinutesAndAmPm.Text = now.ToString("mmtt").ToUpperInvariant();

            _clockBlinkTimer.Interval = TimeSpan.FromSeconds(1);
            _clockBlinkTimer.Tick += (s, e) =>
            {
                if (ClockColon.Foreground == ClockDefaultBrush)
                {
                    ClockColon.Foreground = ClockDimBrush;
                }
                else
                {
                    ClockColon.Foreground = ClockDefaultBrush;
                }
            };
            _clockBlinkTimer.Start();

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
