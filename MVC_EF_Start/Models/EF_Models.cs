using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;


namespace MVC_EF_Start.Models
{
    public class Company
    {
        public string Id { get; set; }
        public string name { get; set; }
        public string date { get; set; }
        public bool isEnabled { get; set; }
        public string type { get; set; }
        public string iexId { get; set; }
        public List<Quote> Quotes { get; set; }
    }

    public class Quote
    {
        public int Id { get; set; }
        public string date { get; set; }
        public float open { get; set; }
        public float high { get; set; }
        public float low { get; set; }
        public float close { get; set; }
        public int volume { get; set; }
        public int unadjustedVolume { get; set; }
        public float change { get; set; }
        public float changePercent { get; set; }
        public float vwap { get; set; }
        public string label { get; set; }
        public float changeOverTime { get; set; }
        public string ClassDemo { get; set; }
        public Company Company { get; set; }
    }

    public class ChartRoot
    {
        public Quote[] chart { get; set; }
    }
    public class Stations
    {
        [Key]
        public int station_id { get; set; }
        public string station_name { get; set; }
        public string? station_phone { get; set; }
        public string? street_address { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? country { get; set; }
        public string? zip { get; set; }
        public string? latitude { get; set; }
        public string? longitude { get; set; }
        public string? fuel_type_code { get; set; }
        public DateTime Date_Updated { get; set; }
    }

    public class station_details
    {
        public List<Stations> Stations { get; set; }
    }
    public class StationCountByCityViewModel
    {
        public string State { get; set; }
        public int StationCount { get; set; }
        public string FuelTypeCode { get; set; }
    }
}