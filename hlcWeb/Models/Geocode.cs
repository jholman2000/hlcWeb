using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hlcWeb.Models
{
    public class Geocode
    {
        public string Status { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }

    public class GeocodeRequest
    {
        public string location { get; set; }
        public GeocodeRequestOptions options;
    }

    public class GeocodeRequestOptions
    {
        public string thumbMaps { get; set; }
    }
}