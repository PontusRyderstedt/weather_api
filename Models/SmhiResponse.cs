using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WeatherApi.Models
{
    public class SmhiStationResponse
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

}