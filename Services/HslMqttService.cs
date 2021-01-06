using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TrippitKiosk.Models;

namespace TrippitKiosk.Services
{
    public class HslMqttService
    {
        public event EventHandler<VehiclePositionsUpdatedEventArgs> VehiclePositionsUpdated;

        private readonly ManagedMqttClientOptions _options;
        private readonly IManagedMqttClient _client;

        private Timer _batchTimer = null;
        private ConcurrentDictionary<int, VehiclePosition> _batchedPositions = new ConcurrentDictionary<int, VehiclePosition>();

        public HslMqttService(IManagedMqttClient client, ManagedMqttClientOptions options)
        {
            _options = options;
            _client = client;
            _client.UseApplicationMessageReceivedHandler(MessageReceived);
            _batchTimer = new Timer(BatchTimerTick);
        }

        public async Task Start(IEnumerable<string> routeGtfsIds)
        {
            if (_client.IsStarted)
            {
                return;
            }

            var topicFilters = routeGtfsIds.Select(x =>
            {
                string headlessRouteId = x.Substring(x.IndexOf(':') + 1);

                return new MqttTopicFilterBuilder().WithTopic(
                    "/hfp" + //prefix
                    "/v2" + // version
                    "/journey" + // journey type
                    "/ongoing" + // temporal type
                    "/vp" + // event type (vp = vehicle position)
                    "/bus" + // transport mode
                    "/+" + // operator ID
                    "/+" + // vehicle number
                    $"/{headlessRouteId}" + // route ID (matches GTFS ID)
                    "/+" + // direction ID
                    "/+" + // headsign
                    "/+" + // start time   
                    "/+" + // next stop
                    "/+" + // gehoash level
                    "/+" + // geohash 
                    "/#" // everything else
                    ).Build();
            });

            await _client.SubscribeAsync(topicFilters).ConfigureAwait(false);
            await _client.StartAsync(_options).ConfigureAwait(false);
            _batchTimer.Change(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2));
        }

        private async Task MessageReceived(MqttApplicationMessageReceivedEventArgs arg)
        {
            if (arg.ProcessingFailed || arg.ApplicationMessage?.Payload == null)
            {
                return;
            }

            // Deserialize from byte array -> JSON -> object in memory
            using MemoryStream stream = new MemoryStream(arg.ApplicationMessage.Payload);
            var position = await JsonSerializer.DeserializeAsync<VehiclePositionResponse>(stream).ConfigureAwait(false);

            // Filter out any messages that don't include a lat/long. If we can't place it on the map, who cares?
            if (position.VehiclePosition.Latitude == null || position.VehiclePosition.Longitude == null
                || position.VehiclePosition.Latitude == 0 || position.VehiclePosition.Longitude == 0)
            {
                return;
            }

            // Build up a batch of updates that only get dispatched occasionally by the timer, so we're not drowning the UI thread in updates
            _batchedPositions.AddOrUpdate(position.VehiclePosition.VehicleNumber, position.VehiclePosition,
                (vehicleNum, newPos) => newPos);
        }

        private void BatchTimerTick(object state)
        {
            List<VehiclePosition> _batchToSend = null;

            _batchToSend = new List<VehiclePosition>();
            foreach (var pair in _batchedPositions)
            {
                if (_batchedPositions.TryRemove(pair.Key, out VehiclePosition pos))
                {
                    _batchToSend.Add(pos);
                }
            }

            if (_batchToSend != null)
            {
                VehiclePositionsUpdated?.Invoke(this, new VehiclePositionsUpdatedEventArgs { Positions = _batchToSend });
            }
        }

    }
}
