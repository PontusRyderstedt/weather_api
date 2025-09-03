using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WeatherApi.Models
{
    // For multiple stations response
    public class SmhiStationsResponse
    {
        [JsonPropertyName("station")]
        public List<SmhiStation> Stations { get; set; } = new();
    }

    public class SmhiStation
    {
        [JsonPropertyName("key")]
        public string Key { get; set; } = "";
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
        [JsonPropertyName("value")]
        public List<SmhiObservation> Observations { get; set; } = new();
    }

    public class SmhiObservation
    {
        [JsonPropertyName("date")]
        public long Date { get; set; }
        [JsonPropertyName("value")]
        public string? Value { get; set; } // comes as string
    }

    // For single station response
    public class SmhiSingleStationResponse
    {
        [JsonPropertyName("station")]
        public SmhiSingleStation Station { get; set; } = new();
        [JsonPropertyName("value")]
        public List<SmhiObservation> Observations { get; set; } = new();
    }

    public class SmhiSingleStation
    {
        [JsonPropertyName("key")]
        public string Key { get; set; } = "";
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
    }

    public static class SmhiExtensions
    {
        public static List<ObservationDto> ToList(this SmhiStationsResponse? response)
        {
            var list = new List<ObservationDto>();
            if (response == null) return list;

            foreach (var station in response.Stations)
            {
                foreach (var obs in station.Observations)
                {
                    if (double.TryParse(obs.Value, out var value))
                    {
                        list.Add(new ObservationDto
                        {
                            StationId = station.Key,
                            StationName = station.Name,
                            Date = DateTimeOffset.FromUnixTimeMilliseconds(obs.Date).LocalDateTime,
                            Value = value
                        });
                    }
                }
            }   

            return list;
        }

        public static List<ObservationDto> ToList(this SmhiSingleStationResponse? response)
        {
            var list = new List<ObservationDto>();
            if (response == null) return list;

            var station = response.Station;

            foreach (var obs in response.Observations)
            {
                if (double.TryParse(obs.Value, out var value))
                {
                    list.Add(new ObservationDto
                    {
                        StationId = station.Key,
                        StationName = station.Name,
                        Date = DateTimeOffset.FromUnixTimeMilliseconds(obs.Date).LocalDateTime,
                        Value = value
                    });
                }
            }

            return list;
        }
    }

}