#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using TrippitKiosk.GraphQL;
using TrippitKiosk.Models;
using TrippitKiosk.Models.ApiModels;
using TrippitKiosk.Models.Geocoding;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TrippitKiosk.Extensions;

namespace TrippitKiosk.Services
{
    public class HslApiService
    {

        public string DefaultGqlRequestUrl { get; } = "https://api.digitransit.fi/routing/v1/routers/hsl/index/graphql";
        public string DefaultGeocodingRequestUrl { get; } = "https://api.digitransit.fi/geocoding/v1/";

        private readonly HttpClient _client;

        public HslApiService(HttpClient client)
        {
            _client = client;
        }

        //---GEOCODING REQUESTS---

        public async Task<GeocodingResponse?> SearchAddressAsync(string searchString, CancellationToken token)
        {
            string urlString = $"{DefaultGeocodingRequestUrl}" +
                $"search?text={searchString}" +
                $"&boundary.rect.min_lat={GeocodingConstants.BoundaryRectMinLat.ToString(NumberFormatInfo.InvariantInfo)}" +
                $"&boundary.rect.max_lat={GeocodingConstants.BoundaryRectMaxLat.ToString(NumberFormatInfo.InvariantInfo)}" +
                $"&boundary.rect.min_lon={GeocodingConstants.BoundaryRectMinLon.ToString(NumberFormatInfo.InvariantInfo)}" +
                $"&boundary.rect.max_lon={GeocodingConstants.BoundaryRectMaxLon.ToString(NumberFormatInfo.InvariantInfo)}" +
                $"&focus.point.lat={GeocodingConstants.FocusPointLat.ToString(NumberFormatInfo.InvariantInfo)}" +
                $"&focus.point.lon={GeocodingConstants.FocusPointLon.ToString(NumberFormatInfo.InvariantInfo)}" +
                $"&lang=en";
            Uri uri = new Uri(urlString);

            try
            {
                var response = await _client.GetFromJsonAsync<GeocodingResponse>(uri, token);

                if (response == null)
                {
                    return null;
                }

                if (!response.Features.Any())
                {
                    return null;
                }

                return response;
            }
            catch (Exception ex) when (ex is COMException || ex is HttpRequestException || ex is OperationCanceledException)
            {
                return null;
            }
        }

        //---GRAPHQL REQUESTS---

        public async Task<List<TransitStop>?> GetStopsAsync(string searchString, CancellationToken token)
        {
            Uri uri = new Uri(DefaultGqlRequestUrl);

            GqlQuery query = new GqlQuery(ApiGqlMembers.stops)
                .WithParameters(new GqlParameter(ApiGqlMembers.name, searchString))
                .WithReturnValues(
                    new GqlReturnValue(ApiGqlMembers.id),
                    new GqlReturnValue(ApiGqlMembers.gtfsId),
                    new GqlReturnValue(ApiGqlMembers.lat),
                    new GqlReturnValue(ApiGqlMembers.lon),
                    new GqlReturnValue(ApiGqlMembers.name),
                    new GqlReturnValue(ApiGqlMembers.code),
                    new GqlReturnValue(ApiGqlMembers.routes,
                        new GqlReturnValue(ApiGqlMembers.mode)
                    )
                );
            IEnumerable<ApiStop>? response = (await GetGraphQL<IEnumerable<ApiStop>>(query, token)
                .ConfigureAwait(false))
                .ToList();

            if (response == null || !response.Any())
            {
                return null;
            }

            return new List<TransitStop>(response.Select(x => new TransitStop
            {
                Name = x.Name,
                Code = x.Code,
                Coords = BasicGeopositionExtensions.Create(0.0, x.Lat, x.Lon),
                GtfsId = x.GtfsId,
                Id = Guid.NewGuid()
            }));

        }

        public async Task<List<TransitLine>?> GetLinesAsync(string searchString, CancellationToken token)
        {
            GqlQuery query = new GqlQuery(ApiGqlMembers.routes)
                .WithParameters(new GqlParameter(ApiGqlMembers.name, searchString))
                .WithReturnValues(
                    new GqlReturnValue(ApiGqlMembers.shortName),
                    new GqlReturnValue(ApiGqlMembers.longName),
                    new GqlReturnValue(ApiGqlMembers.mode),
                    new GqlReturnValue(ApiGqlMembers.patterns,
                        new GqlReturnValue(ApiGqlMembers.stops,
                            new GqlReturnValue(ApiGqlMembers.gtfsId),
                            new GqlReturnValue(ApiGqlMembers.name),
                            new GqlReturnValue(ApiGqlMembers.lat),
                            new GqlReturnValue(ApiGqlMembers.lon)
                        ),
                        new GqlReturnValue(ApiGqlMembers.geometry,
                            new GqlReturnValue(ApiGqlMembers.lat),
                            new GqlReturnValue(ApiGqlMembers.lon)
                        )
                    )
                );

            IEnumerable<ApiRoute>? response = await GetGraphQL<IEnumerable<ApiRoute>>(query, token).ConfigureAwait(false);
            if (response == null || !response.Any())
            {
                return null;
            }

            return response.Select(x => new TransitLine(x)).ToList();
        }

        public async Task<List<TransitLine>?> GetLinesAsync(IEnumerable<string> gtfsIds, CancellationToken token)
        {
            GqlQuery query = new GqlQuery(ApiGqlMembers.routes)
                .WithParameters(new GqlParameter(ApiGqlMembers.ids, gtfsIds))
                .WithReturnValues(
                    new GqlReturnValue(ApiGqlMembers.shortName),
                    new GqlReturnValue(ApiGqlMembers.longName),
                    new GqlReturnValue(ApiGqlMembers.mode),
                    new GqlReturnValue(ApiGqlMembers.patterns,
                        new GqlReturnValue(ApiGqlMembers.stops,
                            new GqlReturnValue(ApiGqlMembers.gtfsId),
                            new GqlReturnValue(ApiGqlMembers.name),
                            new GqlReturnValue(ApiGqlMembers.lat),
                            new GqlReturnValue(ApiGqlMembers.lon)
                        ),
                        new GqlReturnValue(ApiGqlMembers.geometry,
                            new GqlReturnValue(ApiGqlMembers.lat),
                            new GqlReturnValue(ApiGqlMembers.lon)
                        )
                    )
                );

            IEnumerable<ApiRoute>? response = (await GetGraphQL<IEnumerable<ApiRoute>>(query, token).ConfigureAwait(false));

            if (response == null || !response.Any())
            {
                return null;
            }

            return response.Select(x => new TransitLine(x)).ToList();
        }

        public async Task<List<TransitStop>?> GetStopsByBoundingRadius(float lat, float lon, int radiusMeters, CancellationToken token)
        {
            GqlQuery query = new GqlQuery(ApiGqlMembers.stopsByRadius)
                .WithParameters(
                    new GqlParameter(ApiGqlMembers.lat, lat),
                    new GqlParameter(ApiGqlMembers.lon, lon),
                    new GqlParameter(ApiGqlMembers.radius, radiusMeters)
                )
                .WithReturnValues(
                    new GqlReturnValue(ApiGqlMembers.edges,
                        new GqlReturnValue(ApiGqlMembers.node,
                            new GqlReturnValue(ApiGqlMembers.stop,
                                new GqlReturnValue(ApiGqlMembers.gtfsId),
                                new GqlReturnValue(ApiGqlMembers.name),
                                new GqlReturnValue(ApiGqlMembers.code),
                                new GqlReturnValue(ApiGqlMembers.lat),
                                new GqlReturnValue(ApiGqlMembers.lon),
                                new GqlReturnValue(ApiGqlMembers.patterns,
                                    new GqlReturnValue(ApiGqlMembers.name),
                                    new GqlReturnValue(ApiGqlMembers.route,
                                        new GqlReturnValue(ApiGqlMembers.shortName),
                                        new GqlReturnValue(ApiGqlMembers.longName)
                                    )
                                )
                            )
                        )
                    )
                );

            ApiStopsByRadius? response = await GetGraphQL<ApiStopsByRadius>(query, token).ConfigureAwait(false);
            if (response == null || response?.StopsByRadius?.Edges?.Any() != true)
            {
                return null;
            }

            return response?.StopsByRadius?.Edges?.Select(x => new TransitStop
            {
                GtfsId = x.Node.Stop.GtfsId,
                Coords = BasicGeopositionExtensions.Create(0.0, x.Node.Stop.Lat, x.Node.Stop.Lon),
                Code = x.Node.Stop.Code,
                Name = x.Node.Stop.Name,
                Id = Guid.NewGuid()
            }).ToList();
        }

        public async Task<TransitStopDetails?> GetStopDetails(string stopId, DateTime forDate, CancellationToken token)
        {
            GqlQuery query = new GqlQuery(ApiGqlMembers.stop)
                .WithParameters(
                    new GqlParameter(ApiGqlMembers.id, stopId)
                )
                .WithReturnValues(
                    new GqlReturnValue(ApiGqlMembers.gtfsId),
                    new GqlReturnValue(ApiGqlMembers.name),
                    new GqlInlineMethodReturnValue(ApiGqlMembers.stoptimesForServiceDate, new List<GqlParameter> { new GqlParameter(ApiGqlMembers.date, forDate) },
                        new GqlReturnValue(ApiGqlMembers.pattern,
                            new GqlReturnValue(ApiGqlMembers.route,
                                new GqlReturnValue(ApiGqlMembers.gtfsId),
                                new GqlReturnValue(ApiGqlMembers.mode),
                                new GqlReturnValue(ApiGqlMembers.shortName),
                                new GqlReturnValue(ApiGqlMembers.longName)
                            )
                        ),
                        new GqlReturnValue(ApiGqlMembers.stoptimes,
                            new GqlReturnValue(ApiGqlMembers.realtimeState),
                            new GqlReturnValue(ApiGqlMembers.scheduledArrival),
                            new GqlReturnValue(ApiGqlMembers.scheduledDeparture),
                            new GqlReturnValue(ApiGqlMembers.realtimeArrival),
                            new GqlReturnValue(ApiGqlMembers.realtimeDeparture),
                            new GqlReturnValue(ApiGqlMembers.realtime),
                            new GqlReturnValue(ApiGqlMembers.stopHeadsign)
                        )
                    )
                );

            ApiStopResponse? response = await GetGraphQL<ApiStopResponse>(query, token);
            if (response == null)
            {
                return null;
            }

            return new TransitStopDetails(response.Stop, forDate);
        }

        private async Task<T?> GetGraphQL<T>(GqlQuery query, CancellationToken token) where T : class
        {
            string parsedQuery = query.ParseToJsonString(DateParsingStrategy.NoSeparators);
            StringContent stringContent = new StringContent(parsedQuery, Encoding.UTF8, "application/json");
            Uri uri = new Uri(DefaultGqlRequestUrl);

            try
            {
                HttpResponseMessage response = await _client.PostAsync(uri, stringContent, token).ConfigureAwait(false);
                if (response == null || !response.IsSuccessStatusCode)
                {
                    return null;
                }

                T? result = await UnwrapGqlResposne<T>(response, token).ConfigureAwait(false);
                return result;
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is COMException || ex is OperationCanceledException)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to unwrap GQL. Exception: {ex}");
                return null;
            }
        }

        private async Task<T?> UnwrapGqlResposne<T>(HttpResponseMessage response, CancellationToken token) where T : class
        {
            Stream? stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            ApiDataContainer<T>? apiDataContainer = await JsonSerializer.DeserializeAsync<ApiDataContainer<T>>(stream, null, token);

            return apiDataContainer?.Data;
        }
    }
}
